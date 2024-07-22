using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ReadyToFight : State
{
    Transform target;
    Coroutine waiterCor;
    public float stoppingDistance;

    public override void Init()
    {        
        character.ActivateWeapon();       
        target = PlayerInstance.Instance.transform;
        character.onAfterWait += OnAfterWait;
        Move();
    }

    public void Move()
    {       
        waiterCor = null;
        character.navMeshAgent.SetDestination(target.position);
        character.anim.SetBool("rifleWalk", true);
        character.anim.SetBool("idleRifle", false);
        character.navMeshAgent.isStopped = false;
    }

    public void OnAfterWait()
    {
        if (Vector3.Distance(character.transform.position, target.position) > stoppingDistance)
        Move();
        else
        {
            waiterCor = character.StartCoroutine(character.Waiter(5, 10));
        }
    }

    public override void DeInit()
    {
        if (waiterCor != null)
            character.StopCoroutine(waiterCor);
        character.onAfterWait -= OnAfterWait;
        character.anim.SetBool("idleRifle", false);
        character.anim.SetBool("rifleWalk", false);
    }

    public override void Run()
    {
        if (Vector3.Distance(character.transform.position, target.position) <= stoppingDistance && waiterCor == null)
        {            
            character.anim.SetBool("idleRifle", true);
            character.anim.SetBool("rifleWalk", false);
            character.navMeshAgent.isStopped = true;
            waiterCor = character.StartCoroutine(character.Waiter(5, 10));
        }
        else
        {
            character.navMeshAgent.SetDestination(target.position);
        }
    }
}

