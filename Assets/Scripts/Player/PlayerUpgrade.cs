using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerUpgrade 
{
    public Sprite upgradeSprite;
    public string info;
    public TypeUpgrade typeUpgrade;
    public  string nameUpgrade;
    public string nameUpgradeTitle;
    public string description;

    public PlayerUpgrade(PlayerUpgradeData playerUpgradeData){
        if(playerUpgradeData.upgradeSprite != null) 
        upgradeSprite = playerUpgradeData.upgradeSprite;
        if(playerUpgradeData.info!=null)
        info = playerUpgradeData.info;
        if(playerUpgradeData.typeUpgrade!=TypeUpgrade.none)
        typeUpgrade = playerUpgradeData.typeUpgrade;
        if(playerUpgradeData.nameUpgrade!=null)
        nameUpgrade = playerUpgradeData.nameUpgrade;
        if(playerUpgradeData.nameUpgradeTitle!=null)
        nameUpgradeTitle = playerUpgradeData.nameUpgradeTitle;
        if(playerUpgradeData.description!=null)
        description = playerUpgradeData.description;

    }

}
