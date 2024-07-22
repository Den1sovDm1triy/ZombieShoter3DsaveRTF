using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    private Enemy enemy;   

   
    

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();
        
        enemy.onWalk += Walk;
        enemy.onRun += Run;
        enemy.onStop += Stop;
        enemy.onAttack += AttackAnimation;
        enemy.onBiting += Biting;
        enemy.onDeath += Dead;
        enemy.onTakeDamage += Damage;       
       
    }




    private void OnDisable()
    {
        enemy.onWalk -= Walk;
        enemy.onRun -= Run;
        enemy.onStop -= Stop;
        enemy.onAttack -= AttackAnimation;
        enemy.onBiting -= Biting;
        enemy.onDeath -= Dead;
        enemy.onTakeDamage -= Damage;
    }
  

    private void AttackAnimation()
    {
        if (!enemy.isDead)
        {
            anim.SetTrigger("Attack");
        }            
    }

    private void Biting()
    {
        anim.SetBool("Biting", true);
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
        anim.ResetTrigger("Damage");
    }    

    private void Stop()
    {
        anim.SetBool("Biting", false);
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", true);
    }

    private void Walk()
    {
        anim.SetBool("Biting", false);
        anim.SetBool("Run", false);
        anim.SetBool("Walk", true);
        anim.SetBool("Idle", false);
    }
    private void Run()
    {
        anim.SetBool("Biting", false);
        anim.SetBool("Run", true);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
    }
   

    private void Damage()
    {
        anim.SetTrigger("Damage");
    }

    private void Dead()
    {
        anim.ResetTrigger("Damage");
        anim.ResetTrigger("Attack");       
        anim.SetTrigger("Die");

    }
}
