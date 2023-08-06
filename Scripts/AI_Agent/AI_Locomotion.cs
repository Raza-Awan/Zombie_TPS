using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Locomotion : MonoBehaviour
{
    Animator aiController;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        aiController = GetComponent<Animator>();
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
    }
}
