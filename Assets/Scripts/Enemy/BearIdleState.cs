using UnityEngine;
using UnityEngine.AI;

public class BearIdleState : StateMachineBehaviour
{

    float timer;
    public float idleTime = 4f;

    Transform player;

    public float detectionAreaRadius = 18f;
    NavMeshAgent agent;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerState.Instance == null || PlayerState.Instance.isDead)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);

            // Stop movement
            if (agent != null && agent.enabled)
            {
                agent.isStopped = true;
            }

            return;
        }
        // -- Transition to Walk -- //

        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isWalking", true);
        }

        // -- Transition to Chase -- //

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
