using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    

    //public float chaseSpeed = 6f;

    //public float stopChasingDistance = 22f;
    public float stopAttackingDistance = 2.5f;

    public float attackRate = 1f; // attack each sec
    private float attackTimer;
    public int damageToInflict = 5; // hitpoint per sec



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerState.Instance == null || PlayerState.Instance.isDead)
        {
            // Matikan flag "isAttacking" supaya balik ke Idle/Chase
            animator.SetBool("isAttacking", false);
            return;
        }

        LookAtPlayer();
        
        if (attackTimer <= 0)
        {
            Attack();
            
            attackTimer = 1.3f / attackRate;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

            // -- Check if agent should stop attacking -- //

            float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Attack()
    {
        if (PlayerState.Instance == null || PlayerState.Instance.isDead)
        {
            return;
        }

        PlayerState.Instance.TakeDamage(damageToInflict);
    }



}
