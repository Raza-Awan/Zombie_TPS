using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public AiAgentConfig config;
    public UiManager uiManager;
    public TimeManager timer2;

    public NavMeshAgent navMeshAgent;
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer mesh;
    public UIHealthBar ui;
    public Transform playerTransform;
    public Animator animator;

    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.ChangeState(initialState);
    }

    void Update()
    {
        if (uiManager.buttonPressed == true)
        {
            stateMachine.Update();
        }
    }
}
