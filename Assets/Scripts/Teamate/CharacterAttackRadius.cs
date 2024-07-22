using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAttackRadius : MonoBehaviour
{
    public Action <Enemy> onAttack;
    public Action onRelax;
    public List<Enemy> enemies = new List<Enemy>();
    public Enemy nearestTarget;
    public SphereCollider sphereCollider;
    public bool isActive;


    private void Awake()
    {
        Deactivate();
    }

    private void Start()
    {       
        Enemy.onDeadZombie += CheckEnemy;
    }

    private void OnDestroy()
    {
        Enemy.onDeadZombie -= CheckEnemy;
    }

    private void CheckEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public void Activate()
    {
        isActive = true;
        sphereCollider.enabled = true;
        StartCoroutine(targetFinder());
    }

    public void Deactivate()
    {
        isActive = true;
        sphereCollider.enabled = false;
        enemies.Clear();
        StopAllCoroutines();
    }


    IEnumerator targetFinder()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(0.3f);

            if (enemies.Count > 0)
            {
                float closestDistance = float.MaxValue;
                Enemy closestEnemy = null;

                foreach (var e in enemies)
                {
                    float distance = Vector3.Distance(transform.position, e.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = e;
                    }
                }
                nearestTarget = closestEnemy;
                if (nearestTarget != null)
                {
                    if (!nearestTarget.isDead)
                    {
                        onAttack?.Invoke(nearestTarget);
                        Debug.Log("ONATTACK");
                    }
                }
            }
            else onRelax?.Invoke();           
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                if (!enemy.isDead)
                {
                    if(!enemies.Contains(enemy))
                    enemies.Add(enemy);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {                
               enemies.Remove(enemy);              
            }
        }
    }
}
