using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUpgradeShop : MonoBehaviour
{
    public static Action onShowUpgradeShop;
    public static Action onCloseUpgradeShop;
   

    public static Action <PlayerUpgrade> onBuyUpgrade;
    [SerializeField] private Transform hook;
    [SerializeField] private ButtonUpgrade buttonUpgrade;
    [SerializeField] private List<ButtonUpgrade> curButtons=new();

    [SerializeField] private int upgradeCoins=0;


    [SerializeField] private TextMeshProUGUI upgradeCoinsText;    
    
    [SerializeField] private PlayerUpgradesList playerUpgradesList;
    [SerializeField] private PlayerUpgrade currentplayerUpgrade;
    [SerializeField] private GameObject shopObject;
    [SerializeField] private Button closeButton;
    public List<GameObject> hideObjects;
    public GameObject headMark;

    public static bool isUpgradeShopOpen=false;
    private Coroutine CheckCor;

    private void Start(){
        PlayerXPManager.onLevelUp+=ShowUpgradeShop;
        ButtonUpgrade.onClickUpgrade+=ClickUpgrade;
        closeButton.onClick.AddListener(Close);    
        Time.timeScale=1;          
    }

    private void HideObjects(bool isHide){
        foreach(var v in hideObjects){
            v.SetActive(isHide);
        }
    }

    private void OnDestroy(){
        PlayerXPManager.onLevelUp-=ShowUpgradeShop;
        ButtonUpgrade.onClickUpgrade-=ClickUpgrade;
    }


    private void ClearButtons(){
        for(int i=0; i<curButtons.Count; i++){
            Destroy(curButtons[i].gameObject);
        }
        curButtons.Clear();
    }

    

    private void Init(){ 
        ClearButtons();       
        for(int i=0; i<playerUpgradesList.playerUpgradeDatas.Count; i++){
            ButtonUpgrade curbuttonUpgrade = Instantiate(buttonUpgrade, hook);
            curbuttonUpgrade.Init(playerUpgradesList.playerUpgradeDatas[i]);
            curButtons.Add(curbuttonUpgrade);
        }

    }

    private void ClickUpgrade(ButtonUpgrade buttonUpgrade){
        if(buttonUpgrade.GetPrice()<=upgradeCoins){
            PlayerUpgradeManager.Instance.Upgrade(buttonUpgrade.playerUpgrade);
            BuyUpgrade(buttonUpgrade);
        }
    }

    private void BuyUpgrade(ButtonUpgrade buttonUpgrade){
        upgradeCoins-=buttonUpgrade.GetPrice();
        upgradeCoinsText.text="Очков улучшений: "   + upgradeCoins.ToString();
        ClearButtons();
        Init();
    }

    private void ShowUpgradeShop(int level){
       if(level>0)
       StartCoroutine(ShowShop(level));
    }

    private IEnumerator ShowShop(int level){
        yield return new WaitForSeconds(0.8f);
        if (!PlayerHealth.Instance.IsDead())
        {
            CheckCor = StartCoroutine(Checker());
            isUpgradeShopOpen=true;
            onShowUpgradeShop?.Invoke();
            HideObjects(false);
            headMark.SetActive(false);
            shopObject.SetActive(true);
            Time.timeScale = 0.05f;
            if (level > 0)
            {
                upgradeCoins += level;
                upgradeCoinsText.text = "Очков улучшений: " + upgradeCoins.ToString();
                Init();
            }
        }
    }

    private IEnumerator Checker()
    {
        while (true)
        {
            if(PlayerHealth.Instance.IsDead()) Close();
            yield return null;
        }
    }

    private void Close(){
        StopAllCoroutines();
        CheckCor = null;
        isUpgradeShopOpen=false;
        onCloseUpgradeShop?.Invoke();   
        shopObject.SetActive(false);
        HideObjects(true);
        Time.timeScale=1;
    }
}
