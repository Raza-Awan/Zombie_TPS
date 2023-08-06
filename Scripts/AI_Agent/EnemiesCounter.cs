using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesCounter : MonoBehaviour
{
    public int enemiesCount;

    private void Update()
    {
        //AiAgent[] aiAgents = FindObjectsOfType<AiAgent>();
        //enemiesCount = aiAgents.Length;
    }

    private void FixedUpdate()
    {
        AiAgent[] aiAgents = FindObjectsOfType<AiAgent>();
        enemiesCount = aiAgents.Length;
    }
}
