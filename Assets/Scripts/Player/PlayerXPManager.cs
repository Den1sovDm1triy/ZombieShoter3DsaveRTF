using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Xml.XPath;

public class PlayerXPManager : MonoBehaviour
{
   public static Action<int> onLevelUp;
   public static Action<int> onLevelXpUpdate;
   public static Action<int, int> onXpUpdate;
   [SerializeField] private UnityEvent OnLevelUp;
   
   [SerializeField] private int startXP;
   [SerializeField] private int currentXP;
   [SerializeField] private int currentLevel=0;
  

   [SerializeField] private int levelPeriod=100;

   [SerializeField] private int levelXP;
    
    private int headMeleeXpCount=10, headRangeXpCount=7, bodyMeleeXpCount=6, bodyRangeXpCount=5;


    private void Start()
    {
        startXP = 0;
        currentXP = startXP;
        levelXP = levelPeriod * (currentLevel+1);
        onLevelXpUpdate?.Invoke(levelXP);
        onLevelUp?.Invoke(currentLevel);  
        onXpUpdate?.Invoke(currentXP, 0);
        WeaponAttacker.onHitBodyMelee+=HitBodyMelee;
        WeaponAttacker.onHitBodyRange+=HitBodyRanged;
        WeaponAttacker.onHitHeadMelee+=HitHeadMelee;
        WeaponAttacker.onHitHeadRange+=HitHeadRange;
    }

    private void OnDestroy(){
        WeaponAttacker.onHitBodyMelee-=HitBodyMelee;
        WeaponAttacker.onHitBodyRange-=HitBodyRanged;
        WeaponAttacker.onHitHeadMelee-=HitHeadMelee;
        WeaponAttacker.onHitHeadRange-=HitHeadRange;
    }

    private void HitBodyMelee(){
        XpPlus(bodyMeleeXpCount);

    }
    private void HitBodyRanged(){
       XpPlus(bodyRangeXpCount);
    }
    private void HitHeadMelee(){
       XpPlus(headMeleeXpCount);
    }
    private void HitHeadRange(){  
        XpPlus(headRangeXpCount);
    }

   private void XpPlus(int XP){
     currentXP+=XP;
     Debug.Log(currentXP+ " "+ XP);
     if(currentXP<levelXP){
          onXpUpdate?.Invoke(currentXP, XP);
     }      
     CheckLevel();
   }

   private void CheckLevel(){
     if(currentXP>=levelXP){
        int addTonewLevel = currentXP-levelXP;
        LevelUp(addTonewLevel);
     }     
   }


   private void LevelUp(int addXP){
     currentLevel++;     
     levelXP = levelPeriod * (currentLevel+1);
     onLevelXpUpdate?.Invoke(levelXP);
     currentXP=addXP;
     onXpUpdate?.Invoke(currentXP, addXP);
     onLevelUp?.Invoke(currentLevel);
     OnLevelUp?.Invoke();
   }


}
