using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackState : AiState
{
    bool inAttackRange;

    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 1.0f;
    }

    public void Exit(AiAgent agent)
    {
        //agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position;

        inAttackRange = Physics.CheckSphere(agent.transform.position, agent.config.attackRange, agent.config.whatIsPlayer);

        if (inAttackRange)
        {
            agent.transform.LookAt(agent.playerTransform);
            agent.animator.Play("Zombie_Attack");
        }

        if (agent.playerHealth.currentHealth <= 0)
        {
            agent.transform.LookAt(agent.playerTransform);
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }
    }
}
