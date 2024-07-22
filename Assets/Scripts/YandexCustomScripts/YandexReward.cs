using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class YandexReward : MonoBehaviour
{

    public static Action checkUnblock;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI cristalsText;

    private int money;
    private int cristals;
    [SerializeField] private Button reward100CoinsID1;
    [SerializeField] private Button rewardRewiew200coins;
    

    [SerializeField] private Button rewardCristalButton;

    [SerializeField] private TextMeshProUGUI recordText;
    private int record;

    [SerializeField] private TextMeshProUGUI debug;
    public bool isDebug;
     

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        YandexGame.ReviewSentEvent += RewardRewiew;       
        YandexGame.GetDataEvent += SDKEnabled;
    

    }

    // 
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
        YandexGame.ReviewSentEvent -= RewardRewiew;      
        YandexGame.GetDataEvent-= SDKEnabled;
    }

    private void SDKEnabled()
    {
        money = YandexGame.savesData.money;
        cristals = YandexGame.savesData.cristals;
        moneyText.text = money.ToString();
        cristalsText.text = cristals.ToString();
         record = YandexGame.savesData.kills;
        recordText.text = "Рекорд " + record.ToString();
    }




    private void Start()
    {    
        if(isDebug) debug.text= "isMobile"+YandexGame.EnvironmentData.isMobile.ToString()+ " isDesctop"+YandexGame.EnvironmentData.isDesktop.ToString();
        money = YandexGame.savesData.money;
        cristals = YandexGame.savesData.cristals;
        moneyText.text = money.ToString();
        cristalsText.text = cristals.ToString();
        reward100CoinsID1.onClick.AddListener(() => RewardVideoShow(1));
        rewardRewiew200coins.onClick.AddListener(() => Rewiew());       
        rewardCristalButton.onClick.AddListener(() =>RewardVideoShow(3));
        record = YandexGame.savesData.kills;
        recordText.text = "Рекорд " + record.ToString();
        if (YandexGame.EnvironmentData.reviewCanShow)
        {
            rewardRewiew200coins.gameObject.SetActive(true);
        }
        else
        {
            rewardRewiew200coins.gameObject.SetActive(false);
        }        

    }

    private void Rewarded(int id){
         if (id == 1){
            money+=100;
            YandexGame.savesData.money = money;
            moneyText.text=money.ToString();
         }    
         if(id==3){
            cristals+=1;
            YandexGame.savesData.cristals = cristals;
            cristalsText.text=cristals.ToString();
            checkUnblock?.Invoke();
         }
        YandexGame.SaveProgress();
    }

    private void RewardRewiew(bool isSend)
    {
        if (isSend)
        {
            money += 200;
            YandexGame.savesData.money = money;
            moneyText.text = money.ToString();
            rewardRewiew200coins.gameObject.SetActive(false);
        }
        YandexGame.SaveProgress();
    }

    

    

    void RewardVideoShow(int id)
    {		
		YandexGame.RewVideoShow(id);
    }

    private void Rewiew(){
        
        YandexGame.ReviewShow(true);
    }

    private void PromptShow(){

        YandexGame.PromptShow();
    }   
    
    



}
