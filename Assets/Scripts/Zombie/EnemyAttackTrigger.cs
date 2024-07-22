using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    Enemy enemy;
    private Coroutine attackCorotine;
    private Coroutine coolDownCorotine;
    private SphereCollider col;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        col = GetComponent<SphereCollider>();
        col.radius = enemy.enemyModel.attackDistance;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            StopAllCoroutines();
            enemy.onStop?.Invoke();
            attackCorotine = StartCoroutine(Attack());
            enemy.onCantAttack?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && PlayerStates.isAlive && !enemy.IsDead())
        {
            coolDownCorotine = StartCoroutine(CoolDown());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerStates.isAlive && !enemy.IsDead())
        {            
            enemy.onBiting?.Invoke();
        }
    }

    IEnumerator Attack()
    {
        while (!enemy.IsDead())
        {
            yield return new WaitForSeconds(0.3f);
            if (PlayerStates.isAlive)
            {
                enemy.onAttack?.Invoke();                
            }
            yield return new WaitForSeconds(1.7f);
        }
    }  



    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.3f);        
        if(attackCorotine!=null)
        StopCoroutine(attackCorotine);
        enemy.onMoveToPlayer?.Invoke();
    }

    
}
