using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAttack; 
    
    private Enemy enemy;
    private int damage;
    public static Action<int> onPlayerTakeDamage;    


    private void Start()
    {
        enemy = GetComponent<Enemy>();
        damage = enemy.enemyModel.damage;
        enemy.onAttack += Attack;
      
    }

   

    private void OnDestroy()
    {       
        enemy.onAttack -= Attack;
    }

    private void Attack()
    {
        if (!enemy.IsDead() && PlayerStates.isAlive)
        {
           Invoke("AttackRay",0.5f);           
        }
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;       
        Ray ray = new Ray(transform.position + transform.up , transform.forward*2.5f);
        Gizmos.DrawRay(ray);
    }

    private void AttackRay()
    {
        Ray ray = new Ray(transform.position + transform.up, transform.forward);
        /*aycastHit hit;
        if(Physics.Raycast(ray, out hit, 2.5f))
        {
            PlayerHealth playerHealth = hit.transform.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null && !enemy.IsDead())
            {
                onPlayerTakeDamage?.Invoke(damage);
                OnAttack?.Invoke();
            }
        }*/


        
        RaycastHit[] hits = Physics.RaycastAll(ray, 2.5f);

        foreach (var hit in hits)
        {
            PlayerHealth playerHealth = hit.transform.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null && !enemy.IsDead())
            {
                onPlayerTakeDamage?.Invoke(damage);
                OnAttack?.Invoke();
            }
        }

        /*
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position + transform.forward, 0.1f, Vector3.forward, 0.05f);
        
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            PlayerHealth  playerHealth= hit.transform.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null&&!enemy.IsDead())
            {
                onPlayerTakeDamage?.Invoke(damage);
                OnAttack?.Invoke();
            }            
        }     */

    }

  
}
