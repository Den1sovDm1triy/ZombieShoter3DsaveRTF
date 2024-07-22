using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOutLine : MonoBehaviour
{    
    ZombieModel zombieModel;
    bool isneed=true;
    Enemy enemy;
    private void Start()
    {
        zombieModel = GetComponentInParent<ZombieModel>();
        enemy = GetComponentInParent<Enemy>();
        enemy.onDeath += DontneedIndicate;
    }
    private void OnDestroy()
    {
        enemy.onDeath -= DontneedIndicate;
    }

    private void DontneedIndicate()
    {
        isneed = false;
        zombieModel.outline.enabled = false;
    }

    public void Indicate()
    {
        if(isneed)
        zombieModel.outline.enabled = true;
    }
    public  void StopIndicate()
    {
        zombieModel.outline.enabled = false;
    }
}
