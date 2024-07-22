using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehaviour : MonoBehaviour
{
    public Enemy enemy;
    public NavAgent navAgent;

    private void Start()
    {
        enemy.onDeath += Death;
    }
    private void OnDestroy()
    {
        enemy.onDeath -= Death;
    }
    public virtual void Init()
    {


    }
    public virtual void DeInit()
    {
    }

    private void Death()
    {
        navAgent.agent.enabled = false;     
    }
}
