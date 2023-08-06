using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Config")]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime;
    public float maxDistance;
    public float dieForce = 5f;
    public float maxSightDistance = 5f;
    public float attackRange = 1f;
    public LayerMask whatIsPlayer;
}
