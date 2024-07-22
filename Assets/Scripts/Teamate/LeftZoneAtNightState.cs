using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LeftZoneAtNightState : State
{    
    private Transform target;
    private Coroutine waiterCor;
    private bool isFirstTime=true;
    private int point;
    public override void Init()
    {
        point = 0;
        character.navMeshAgent.speed = character.teammate.mateData.moveSpeed;
        character.onAfterWait += Hide;
        Move();       
    }
    public override void DeInit()
    {
        if (waiterCor != null)
            character.StopCoroutine(waiterCor);        
        character.anim.SetBool("walk", false);
        character.anim.SetBool("idle", true);
        character.onAfterWait -= Hide;
    }

    public void Move()
    {
        target = character.exitPoint;
        character.navMeshAgent.SetDestination(target.position);
        character.anim.SetBool("walk", true);
        character.anim.SetBool("idle", false);        
        character.navMeshAgent.isStopped = false;
    }

    private void Hide()
    {
        isFirstTime = true;
        target = character.exitPoint2;
        character.navMeshAgent.SetDestination(target.position);
        character.anim.SetBool("walk", true);
        character.anim.SetBool("idle", false);
        character.navMeshAgent.isStopped = false;
        character.health.SwithOffCollider();       
    }
       

    public override void Run()
    {
        if (Vector3.Distance(character.transform.position, target.position) < 1f&&isFirstTime&&point==0)
        {
            isFirstTime = false;
            character.anim.SetBool("walk", false);
            character.anim.SetBool("idle", true);
            character.navMeshAgent.isStopped = true;  
            waiterCor = character.StartCoroutine(character.Waiter(1, 1));
            point++;
        }
        else if (Vector3.Distance(character.transform.position, target.position) < 1f && isFirstTime && point == 1)
        {
            isFirstTime = false;
            character.anim.SetBool("walk", false);
            character.anim.SetBool("idle", true);
            character.navMeshAgent.isStopped = true;          
        }
    }
}
