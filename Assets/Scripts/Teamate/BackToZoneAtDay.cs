using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class BackToZoneAtDay : State
{
    private Transform target;
    private Coroutine waiterCor;
    private bool isFirstTime = true;
    private int point;
   
    public override void Init()
    {
        point = 0;
        character.navMeshAgent.speed = character.teammate.mateData.runSpeed;
        character.onAfterWait += MoveNextPoint;
        Move(character.enterPoint);
    }
    public override void DeInit()
    {
        if (waiterCor != null)
            character.StopCoroutine(waiterCor);
        character.anim.SetBool("run", false);
        character.anim.SetBool("idle", true);
        character.onAfterWait -= MoveNextPoint;
    }

    public void MoveNextPoint()
    {
        isFirstTime = true;
        Move(character.exitPoint);
    }


    public void Move(Transform target)
    {
        this.target =  target;
        character.navMeshAgent.SetDestination(target.position);
        character.anim.SetBool("run", true);
        character.anim.SetBool("idle", false);
        character.navMeshAgent.isStopped = false;
    }

   


    public override void Run()
    {
        if (Vector3.Distance(character.transform.position, target.position) < 1f && isFirstTime && point == 0)
        {
            isFirstTime = false;
            character.anim.SetBool("run", false);
            character.anim.SetBool("idle", true);
            character.navMeshAgent.isStopped = true;           
            waiterCor = character.StartCoroutine(character.Waiter(1, 1));
            point++;
        }
        else if (Vector3.Distance(character.transform.position, target.position) < 1f && isFirstTime && point == 1)
        {
            isFirstTime = false;
            character.health.SwithOnCollider();
            character.InitBeforeHire();
           
        }
    }
}
