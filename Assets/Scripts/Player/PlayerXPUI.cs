using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PlayerXPUI : MonoBehaviour
{
    public static Action<int> onLevelUp;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI currXpText;
    [SerializeField] private int periodXp;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI changeXPText;
    [SerializeField] private GameObject changeXPobject;
    [SerializeField] private GameObject levelUpObject;



    private void Awake(){
        PlayerXPManager.onXpUpdate += XpChanged;
        PlayerXPManager.onLevelXpUpdate += XpPeriodChanged;
        PlayerXPManager.onLevelUp += LevelChanged;
    }

    
    private void OnDestroy(){
        PlayerXPManager.onXpUpdate -= XpChanged;
        PlayerXPManager.onLevelXpUpdate -= XpPeriodChanged;
        PlayerXPManager.onLevelUp  -= LevelChanged;
    }
    private void XpChanged(int curXp, int changeXP){
        xpSlider.value=curXp;
        currXpText.text = curXp.ToString()+"/"+periodXp.ToString();
        //changeXPText.text = changeXP.ToString();
        //changeXPobject.SetActive(true);
    }
    private void XpPeriodChanged(int period){
       xpSlider.maxValue = period;    
       periodXp = period;  
    }
    private void LevelChanged(int level){
        Action levelUpAction=()=>LevelUp(level);
        AnimTweenManager.DoScaleShake(levelUpObject.transform, 1.1f, 0.2f, levelUpAction);       
        currentLevelText.text = "Уровень " + level.ToString();       
    }

    private void LevelUp(int level){
        onLevelUp?.Invoke(level);
    }
 

}
