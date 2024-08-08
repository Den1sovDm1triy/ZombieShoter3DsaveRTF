using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgrade : MonoBehaviour
{
    public static Action <ButtonUpgrade> onClickUpgrade;
    [SerializeField] private Button button;   
   
    [SerializeField] private Image upgradeImage;
    private int price;  

    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI upgradeDescription;


    public TextMeshProUGUI priceText;      
    public PlayerUpgrade playerUpgrade;


    public int GetPrice(){
        return price;
    }

    private void Start(){
        button.onClick.AddListener(ClickButton);
    }

    private void ClickButton(){
        onClickUpgrade?.Invoke(this);
    }

   
    public void Init(PlayerUpgradeData playerUpgradeData)
    {
        playerUpgrade = new PlayerUpgrade(playerUpgradeData);  
        price = PlayerUpgradeManager.Instance.GetPrice(playerUpgrade);
        priceText.text = "Цена: " +price.ToString();
        upgradeImage.sprite = playerUpgrade.upgradeSprite;
        upgradeText.text = playerUpgrade.nameUpgradeTitle;
        upgradeDescription.text = playerUpgrade.description;
    }

}

public enum TypeUpgrade{
     SpeedUp, HealthUp, HealUpgrade, VampireUp, MoneyUp, CollectionRadiusUp, none,
}

