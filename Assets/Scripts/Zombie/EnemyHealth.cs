using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyHealth : MonoBehaviour
{
    public Action <float>onNewHealth;
    private Enemy enemy;
    public float heath;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private CapsuleCollider headCollider;
    [SerializeField] private Rigidbody rb;
     public bool readyToDamage = false;
    private void Awake()
    {
        if (capsuleCollider != null)
            capsuleCollider.enabled = false;
        if (headCollider != null)
            headCollider.enabled = false;
    }

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();      
        heath = enemy.enemyModel.maxHealth;
        StartCoroutine(ReadytorakeDamage());
    }

    private IEnumerator ReadytorakeDamage()
    {
        yield return new WaitForSeconds(1f);
        if (capsuleCollider!=null)
        capsuleCollider.enabled = true;
        if(headCollider!=null)
        headCollider.enabled = true;
        readyToDamage = true;
    }

   

    public void TakeDamage(int damage)
    {
        Debug.Log("+++++" + damage);
        if (!readyToDamage) return;
            heath -= damage;
            heath = Mathf.Clamp(heath, 0, enemy.enemyModel.maxHealth);
            onNewHealth?.Invoke(heath);
            if (heath <= 0)
            {
                heath = 0;
                enemy.onDeath?.Invoke();
                enemy.isDead = true;
            /*
                capsuleCollider.height = 0.35f;
                capsuleCollider.center = new Vector3(0, 0, 0);
                capsuleCollider.radius = 0.35f;*/
                capsuleCollider.enabled = false;
            if(headCollider!=null)
                headCollider.enabled = false;
            }
            else
            {
                enemy.onTakeDamage?.Invoke();
            }
    }

    private void SwitchOffCapsule()
    {
        capsuleCollider.enabled = false;       
    }
}
