using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine.PlayerLoop;

public  class PlayerUpgradeManager: MonoBehaviour 
{
    public static PlayerUpgradeManager Instance;
    public static Action<int> onHealthUpUpgrade;
    public static Action<int> onHealAfterDamageUpgrade;
    public static Action<float> onSpeedUpUpgrade;
    public static Action<float> onVampireUpgrade;
    public static Action<float> onCollectionRadiusUpgrade;
  
    public   List<PlayerUgradeLevel> playerUgradeLevels=new List<PlayerUgradeLevel>();

    public  float speedX=0;
    public float speedStep=0.5f;    
    public  int speedLevel=0;

    public  int healthPlus=0;
    public  int healthStep=10;
    public  int healthLevel=0;

    public  int healPoints=0;
    public  int healPointsStep=1;
    public  int healLevel=0;

    public  float vampireX=0;
    public  float vampireStep=1;
    public  int vampireLevel=0;


    public int moneyX=0;
    public int moneyStep=1;
    public int moneyLevel=0;

    public int collectionRadius = 0;
    public int collectionRadiusStep = 3;
    public int collectionRadiusLevel = 0;


    public PlayerUgradeLevel speedUpUpgrade;
    public PlayerUgradeLevel healthUpUpgrade;
    public PlayerUgradeLevel healUpgrade;
    public PlayerUgradeLevel vampireUpgrade;
    public PlayerUgradeLevel moneyUpgrade;
    public PlayerUgradeLevel collectionRadiusUpgrade;


    public void Awake(){
         if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
        InitUpgrades();
    }

   

    public void Upgrade(PlayerUpgrade playerUpgrade)
    {
        foreach (var p in playerUgradeLevels)
        {
            if (p.typeUpgrade == playerUpgrade.typeUpgrade)
            {
                p.LevelUp();
            }
        }
        switch (playerUpgrade.typeUpgrade)
        {
            case TypeUpgrade.SpeedUp:
                SpeedUp();
                break;
            case TypeUpgrade.HealthUp:
                HealthUp();
                break;
            case TypeUpgrade.HealUpgrade:
                HealUpgrade();
                break;
            case TypeUpgrade.VampireUp:
                VampireUp();
                break;
            case TypeUpgrade.MoneyUp:
                MoneyUp();
                break;
            case TypeUpgrade.CollectionRadiusUp:
                CollectionRadiusUp();
                break;
        }
    }

    public  void InitUpgrades(){
        speedUpUpgrade = new PlayerUgradeLevel(0, TypeUpgrade.SpeedUp);
        healthUpUpgrade = new PlayerUgradeLevel(0, TypeUpgrade.HealthUp);
        healUpgrade = new PlayerUgradeLevel(0, TypeUpgrade.HealUpgrade);
        vampireUpgrade = new PlayerUgradeLevel(0, TypeUpgrade.VampireUp);
        moneyUpgrade = new PlayerUgradeLevel(0, TypeUpgrade.MoneyUp);
        collectionRadiusUpgrade = new PlayerUgradeLevel(0, TypeUpgrade.CollectionRadiusUp);
        playerUgradeLevels.Add(speedUpUpgrade);  
        playerUgradeLevels.Add(healthUpUpgrade);  
        playerUgradeLevels.Add(healUpgrade);
        playerUgradeLevels.Add(vampireUpgrade);
        playerUgradeLevels.Add(moneyUpgrade);
        playerUgradeLevels.Add(collectionRadiusUpgrade);
    }


    public  void SpeedUp(){        
        speedX=speedStep*speedUpUpgrade.level;
        onSpeedUpUpgrade?.Invoke(speedX);

    }
    public  void HealthUp(){        
        healthPlus=healthStep*healthUpUpgrade.level;
        onHealthUpUpgrade?.Invoke(healthPlus);
        Debug.Log("HealthUpdate"+healthPlus+healthUpUpgrade.level);
    }
    public  void HealUpgrade()
    {        
        healPoints=healPointsStep*healUpgrade.level; 
        onHealAfterDamageUpgrade?.Invoke(healPoints);       
              
    }
    public   void VampireUp()
    {        
        vampireX=vampireStep*vampireUpgrade.level;
        onVampireUpgrade?.Invoke(vampireX);
    }
    public   void MoneyUp()
    {        
        moneyX=moneyStep*moneyUpgrade.level;
        onVampireUpgrade?.Invoke(moneyX);
    }
    

    public void CollectionRadiusUp()
    {
        collectionRadius = collectionRadiusStep*collectionRadiusUpgrade.level;
        onCollectionRadiusUpgrade?.Invoke(collectionRadius);
        Debug.Log("CollectionUpdate " +  collectionRadius + collectionRadiusUpgrade.level);
    }

    public  int GetPrice(PlayerUpgrade playerUpgrade)
    {
        return GetCurrLevel(playerUpgrade.typeUpgrade)+1;
    }   

    public  int GetCurrLevel(TypeUpgrade typeUpgrade){
        int currentLevel=0;
        foreach(var l in playerUgradeLevels){
            if(typeUpgrade==l.typeUpgrade){
                currentLevel=l.level;
            }
        }
        return currentLevel;
    } 

}
[Serializable]
public class PlayerUgradeLevel{

    public PlayerUgradeLevel (int level, TypeUpgrade typeUpgrade)
    {
        this.level = level;
        this.typeUpgrade = typeUpgrade;
    }
    public int level;
    public TypeUpgrade typeUpgrade;

    public void LevelUp(){
        level++;
    }
}
