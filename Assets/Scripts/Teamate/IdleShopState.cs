using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu]
public class IdleShopState : State
{
    [SerializeField] private Vector2 waittime;
    private Transform target;
    private Coroutine waiterCor;     
    private Transform currentPoint;
    [SerializeField] private float stopDistance;

    private bool ismove;
    public override void Init()
    {
        character.navMeshAgent.speed = character.teammate.mateData.moveSpeed;
        Move();
        character.onAfterWait += Move;
    }

    public void Move()
    {
        ismove = true;
        target = character.shopwayPoint[UnityEngine.Random.Range(0, character.shopwayPoint.Count)];
        if(target==currentPoint)
            target = character.shopwayPoint[UnityEngine.Random.Range(0, character.shopwayPoint.Count)];
        currentPoint = target;
        character.navMeshAgent.SetDestination(target.position);
        character.anim.SetBool("walk", true);
        character.anim.SetBool("idle", false);
        int r = UnityEngine.Random.Range(0, 100);
        if (r > 80)
            character.PlayIdleWalkAudio();
        character.navMeshAgent.isStopped = false;
    }

    public override void DeInit()
    {
        if(waiterCor!=null)
        character.StopCoroutine(waiterCor);
        character.onAfterWait -= Move;
        character.anim.SetBool("walk", false);
        character.anim.SetBool("idle", true);
    }



    public override void Run()
    {
        if(Vector3.Distance(character.transform.position, target.position) < stopDistance&&ismove)
        {
            ismove = false;
            int r = UnityEngine.Random.Range(0, 100);
            if (r > 80)
                character.PlayIdleWStayAudio();
            character.anim.SetBool("walk", false);
            character.anim.SetBool("idle", true);
            character.navMeshAgent.isStopped = true;
            waiterCor = character.StartCoroutine(character.Waiter((int)waittime.x, (int)waittime.y));
        }
    }

    








}
