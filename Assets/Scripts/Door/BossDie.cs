using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    public static Action<Vector3> onBoosDie;
    [SerializeField] private Enemy enemy;


    private void Start(){
        enemy = GetComponent<Enemy>();
        if(enemy!=null){
            enemy.onDeath += BossDead;
        }
    }

    private void BossDead()
    {
        onBoosDie?.Invoke(this.transform.position);   
        enemy.onDeath -= BossDead;
    }

    private void OnDestroy(){
        if(enemy!=null&&enemy.onDeath!=null){
           enemy.onDeath -= BossDead;
        }
    }
}
