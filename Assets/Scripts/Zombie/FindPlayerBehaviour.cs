using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindPlayerBehaviour : EnemyBehaviour
{
    public override void Init()
    {
        navAgent.Speed = enemy.enemyModel.runSpeed;
        Debug.Log("Состояние преследования");
        StartCoroutine(Pursuit());
    }

    public override void DeInit()
    {
        StopAllCoroutines();
    }

    private IEnumerator Pursuit()
    {
        enemy.onRun?.Invoke();
        while (true)
        {

            NavMeshHit hit;
            Vector3 targetPosition = PlayerInstance.Instance.transform.position;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                navAgent.agent.SetDestination(hit.position);
            }
            else
            {
                enemy.onLoosePlayer?.Invoke();
            }

            yield return new WaitForSeconds(0.5f);

        }
    }
}
