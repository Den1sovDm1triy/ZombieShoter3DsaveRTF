using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : EnemyBehaviour
{
    [Tooltip("¬рем€ ожидани€ в каждой точке патрулировани€ (мин, макс)")]
    [SerializeField] private Vector2 idleTimeRange = new Vector2(0f, 0f);
    private Vector3 originPosition;
    private int moveAttempts = 30;    
    [Tooltip("–адиус партулировани€ относительно начального положени€")]
    [SerializeField] private float patrolRadius = 10f;
    [Tooltip("ћинимальное рассто€ние перемещени€ (относительно радиуса партрулировани€)")]
    [SerializeField] private float minDistanceRatio = 0.5f;
    public override void Init()
    {
        Debug.Log("—осто€ние патрулировани€");
        originPosition = transform.position;
        navAgent.Speed = enemy.enemyModel.walkSpeed;
        StartCoroutine(PatrolCoroutine());
    }

    public override void DeInit()
    {
        StopAllCoroutines();
    }



    private IEnumerator PatrolCoroutine()
    {
        enemy.onWalk?.Invoke();
        while (true)
        {          
            if (patrolRadius > 0f)
            {                
                enemy.onWalk?.Invoke();
                navAgent.MoveToPositionAsync(GetPatrolPoint());
                yield return new WaitWhile(() => navAgent.IsBusy);
                enemy.onStop?.Invoke();
            }
            else
            {
                if (idleTimeRange.y > 0f)
                {                    
                    navAgent.TurnToDirectionAsync(Vector3.ProjectOnPlane(Random.onUnitSphere, transform.up));
                    yield return new WaitWhile(() => navAgent.IsBusy);
                }
            }
            yield return new WaitForSeconds(Random.Range(idleTimeRange.x, idleTimeRange.y));
        }
    }
    private Vector3 GetPatrolPoint()
    {
        Vector3 point = transform.position;

        for (int i = 0; i < moveAttempts; ++i)
        {
            point = originPosition + Vector3.ProjectOnPlane(Random.onUnitSphere * patrolRadius, Vector3.up);

            // ≈сли нова€ точка слишком близко - не используем ее
            if (Vector3.Distance(transform.position, point) < minDistanceRatio * patrolRadius) continue;
            // ћожно ли попасть в точку
            if (navAgent.CanMoveTo(point)) break;
        }

        return point;
    }
}
