using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu]
public class CollectVegetables : State
{
    NavMeshPath path;
    public override void Init()
    {
        character.navMeshAgent.isStopped = false;
        character.navMeshAgent.speed = character.teammate.mateData.runSpeed;
        path = new NavMeshPath();
        character.navMeshAgent.SetDestination(character.item.gameObject.transform.position);
        character.anim.SetBool("run", true);
        character.anim.SetBool("walk", false);
        character.anim.SetBool("idle", false);
    }
    public override void DeInit()
    {

        character.navMeshAgent.speed = character.teammate.mateData.moveSpeed;
        character.anim.SetBool("run", false);
        character.anim.SetBool("walk", false);
        character.anim.SetBool("idle", true);
    }
    

    public override void Run()
    {
        if (character.item != null)
        {
            if (Vector3.Distance(character.transform.position, character.item.gameObject.transform.position) <= 0.5)
            {
                character.anim.SetBool("run", false);
                character.anim.SetBool("walk", false);
                character.anim.SetBool("idle", true);
            }
            else
            {
                if (character.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    character.navMeshAgent.SetDestination(character.item.gameObject.transform.position);
                    character.anim.SetBool("run", true);
                    character.anim.SetBool("walk", false);
                    character.anim.SetBool("idle", false);
                }
                else character.FindAllVeg();
            }
        }
        else character.FindAllVeg();

    }
}
