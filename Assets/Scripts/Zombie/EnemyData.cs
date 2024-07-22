using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public int health;
    public int damage;
    public float walkSpeed;
    public float runSpeed;
    public float attackDistance;
    public float findTargetDistance;
}
