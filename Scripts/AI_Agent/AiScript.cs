using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator aiController;

    public Transform playerTransform;

    public LayerMask whatIsPlayer;

    public float maxTime = 1f;
    public float maxDistance = 1f;

    float timer;

    bool alreadyAttacked;

    public float attackRange = 5f;
    public bool playerInAttackRange;

    int isAttacking;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        aiController = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        isAttacking = Animator.StringToHash("Zombie_Attack");
    }

    void Update()
    {
        if (agent.hasPath)
        {
            aiController.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            aiController.SetFloat("Speed", 0);
        }

        //Check for attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange)
        {
            Chasing();
        }
        if (playerInAttackRange)
        {
            Attack();
        }
    }

    private void Chasing()
    {
        if (!agent.enabled)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (!agent.hasPath)
        {
            agent.destination = playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (playerTransform.position - agent.destination);
            if (direction.sqrMagnitude > maxDistance * maxDistance)
            {
                if (agent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.destination = playerTransform.position;
                }
            }
            timer = maxTime;
        }
    }

    private void Attack()
    {
        if (playerInAttackRange && !alreadyAttacked)
        {
            gameObject.transform.LookAt(playerTransform);
            aiController.Play(isAttacking);
        }
    }

    public void AttackPlayer()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
