using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WeaponAmmoManger : MonoBehaviour
{
    [SerializeField] private List<AmmoItem> ammos;
    [SerializeField] private MoneyItem moneyItem;
    [SerializeField] private GameObject cristalItem;
    private void Start()
    {
        Enemy.onDeadZombie += SpawnAmmo;
        BossDie.onBoosDie += SpawnCristal;
    }
    private void OnDestroy()
    {
        Enemy.onDeadZombie -= SpawnAmmo;
        BossDie.onBoosDie -= SpawnCristal;
    }

    private void SpawnAmmo(Enemy enemy)        
    {                         
        var a = Instantiate(moneyItem, new Vector3(enemy.transform.position.x, 0.5f, enemy.transform.position.z), Quaternion.identity);
        a.transform.localScale=new Vector3(0,0,0);
        a.transform.DOScale(new Vector3(10, 10, 10), 1f);
        moneyItem.Setup(1*ZombieManager.curNumberWave);             
    }

    private void SpawnCristal(Vector3 pos){
         var a = Instantiate(cristalItem, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity);
        a.transform.localScale=new Vector3(0,0,0);
        a.transform.DOScale(new Vector3(1, 1, 1), 1f);
    }
}
