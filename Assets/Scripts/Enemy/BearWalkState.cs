using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearWalkState : StateMachineBehaviour
{

    float timer;
    public float walkingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionAreaRadius = 18f;
    public float walkSpeed = 2f;

    List<Transform> waypointsList = new List<Transform>();


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // -- Initialization -- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = walkSpeed;
        timer = 0f;

        // -- Get all waypoints and Move to the first waypoint -- //
        //GameObject.FindGameObjectWithTag("Waypoints");
        GameObject waypointsCluster = animator.GetComponent<NPCWaypoints>().npcWaypointsCluster;
        foreach (Transform t in waypointsCluster.transform)
        {
            waypointsList.Add(t);
        }

        Vector3 firstPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(firstPosition);
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

        // -- Normal Walking Logic -- //
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > walkingTime)
        {
            animator.SetBool("isWalking", false);
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
