using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu]
public class AttackState : State
{   
    public float rotationSpeed;
    public Action OnShoot;
    public override void Init()
    {
        character.navMeshAgent.isStopped = true;       
        character.anim.SetBool("rifleReady", true);
    }
    public override void DeInit()
    {
        character.navMeshAgent.isStopped = false;
        character.anim.SetBool("rifleReady", false);
        character.anim.ResetTrigger("shot");
    }




    public override void Run()
    {
        if (character.target != null)
        {
            if (character.target.isDead == false)
            {
                Vector3 direction = (character.target.transform.position - character.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                float angleToTarget = Quaternion.Angle(character.transform.rotation, lookRotation);
                if (angleToTarget < 5f)
                {                  
                    OnShoot?.Invoke();
                    character.anim.SetTrigger("shot");                   
                }
            }
        }
    }
 
}
