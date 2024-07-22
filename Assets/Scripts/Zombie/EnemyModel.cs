using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

     public int maxHealth;
     public int damage;
    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float runSpeed;
    [HideInInspector] public float attackDistance;
    [HideInInspector] public float findTargetDistance;


    private void Awake()
    {
        walkSpeed = enemyData.walkSpeed;
        runSpeed = enemyData.runSpeed;
        attackDistance = enemyData.attackDistance;
        findTargetDistance = enemyData.findTargetDistance;
    }

    public void Setup(float  Xhealth, float Xdamage)
    {
        maxHealth = (int)(enemyData.health*Xhealth);
        damage = (int)(enemyData.damage*Xdamage);        
    }
}
