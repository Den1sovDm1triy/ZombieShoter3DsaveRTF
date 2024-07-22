using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Health : MonoBehaviour
{
    public Action onDead;
    public Action onTakeDamage;
    public int currentHealth;
    public int maxHealth;
    public bool isDead;
    public CapsuleCollider capsuleCollider;
    private void Start()
    {
        isDead = false;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void  SwithOffCollider()
    {
        capsuleCollider.enabled = false;
    }
    public void SwithOnCollider()
    {
        capsuleCollider.enabled = true;
    }

    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {      
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0,maxHealth);        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onDead?.Invoke();
            isDead = true;          
            capsuleCollider.enabled = false;           
        }
        else
        {
            onTakeDamage?.Invoke();
        }
    }
}
