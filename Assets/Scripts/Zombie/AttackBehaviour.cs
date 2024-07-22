using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : EnemyBehaviour
{

    private IEnumerator RotateTowardsTarget()
    {
        Vector3 targetDirection = PlayerInstance.Instance.transform.position - enemy.transform.position;
        targetDirection.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        while (Quaternion.Angle(enemy.transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, 100 * Time.deltaTime);
            yield return null;
        }

        enemy.transform.rotation = targetRotation; // Убедитесь, что поворот точно совпадает с целевым углом      
    }





    public override void Init()
    {
        navAgent.agent.enabled = false;
        StartCoroutine(RotateTowardsTarget());
    }
    public override void DeInit()
    {
        navAgent.agent.enabled = true;
        StopAllCoroutines();
        navAgent.agent.stoppingDistance = 1;
    }
}
