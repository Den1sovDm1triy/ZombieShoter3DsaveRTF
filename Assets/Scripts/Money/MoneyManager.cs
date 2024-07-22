using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YG;

public class MoneyManager : MonoBehaviour
{
    public Action <int> onEarnMoney;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip takeClip, spendClip;
    [SerializeField] private AudioClip takeCristallClip;
    public static Action <int, int> onMoneyChange;
    public static Action <int, int> onCristalChange;
    private int moneycount;
    private int cristallcount;
    public static MoneyManager Instance;
    public List<GameObject> takemoneyEffect;
    public List<GameObject> takeCristallEffect;
    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        MoneyItem.onTakeMoney += TakeMoney;
        CristalItem.onTakeCristal+=TakeCristal;

        Instance = this;
        moneycount =  YandexGame.savesData.money;
        cristallcount = YandexGame.savesData.cristals;
        
        onMoneyChange?.Invoke(moneycount, 0);  
        onCristalChange?.Invoke(cristallcount, 0);
        ShopManager.onCloseShop+=SaveMoney; 
        PlayerHealth.onDeath+=SaveMoney;  
        WeaponAttacker.onGiveDamage+=AddMoneyUpgrade;
    }



    private void OnDestroy()
    {
        MoneyItem.onTakeMoney -= TakeMoney;
        ShopManager.onCloseShop-=SaveMoney;  
        PlayerHealth.onDeath-=SaveMoney; 
        WeaponAttacker.onGiveDamage-=AddMoneyUpgrade; 
        CristalItem.onTakeCristal-=TakeCristal;
    }

    private void TakeCristal(Vector3 pos, int amount)
    {
        AddCristal(amount);
        foreach(var t in takemoneyEffect)
        {
            if (!t.activeInHierarchy)
            {
                t.transform.position = pos;
                t.SetActive(true);
                return;
            }
        }
    }

    public void AddCristal(int amount){
        audio.PlayOneShot(takeCristallClip);
        cristallcount += amount;       
        onCristalChange?.Invoke(cristallcount, amount);
        YandexGame.savesData.cristals = cristallcount;
        YandexGame.SaveProgress();
    }

    private void AddMoneyUpgrade(int i)
    {
        int amount = PlayerUpgradeManager.Instance.moneyX;
        moneycount += amount;
        onMoneyChange?.Invoke(moneycount, amount);
    }

    private void SaveMoney(){
        YandexGame.savesData.money=moneycount;
        YandexGame.savesData.cristals=cristallcount;
        YandexGame.SaveProgress();
    }

    [ContextMenu("Add 10000")]
    private void AdMoney()
    {
        AddMoney(10000);
    }

    [ContextMenu("REset")]
    private void Reset(){
        YandexGame.ResetSaveProgress();
    }
    private void TakeMoney(Vector3 pos, int amount)
    {       
        AddMoney(amount);
        foreach(var t in takemoneyEffect)
        {
            if (!t.activeInHierarchy)
            {
                t.transform.position = pos;
                t.SetActive(true);
                return;
            }
        }
        
    }

    public void SpendMoney(int amount)
    {
        moneycount -= amount;
        audio.PlayOneShot(spendClip);
        onMoneyChange?.Invoke(moneycount, -amount);
        /*
        if (moneycount >= amount)
        {
            moneycount -= amount;
            onMoneyChange?.Invoke(moneycount, -amount);
        }*/
    }

    public void AddMoney(int amount)
    {
        audio.PlayOneShot(takeClip);
        moneycount += amount;
        onMoneyChange?.Invoke(moneycount, amount);
        onEarnMoney?.Invoke(amount);
    }

    public int GetMoneyCount()
    {
        return moneycount;
    }

    public bool IsEnough(int amount)
    {
        if (moneycount >= amount) return true;
        else return false;
    }

    public int HowManyCanBuy(int price)
    {
        Debug.Log(price);
        int maxQuantity = (int)moneycount / price;
        return maxQuantity;
    }

    
}
