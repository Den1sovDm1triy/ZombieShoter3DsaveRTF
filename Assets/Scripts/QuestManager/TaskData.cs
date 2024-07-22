using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TaskData", menuName = "TAskData", order = 2)]
public class TaskData: ScriptableObject
{    
    public int taskCount;
    public string description1, description2;
    public TypeTask typeTask;
    public SubTask subTask;

    public string GetIdTask()
    {
        return name;
    }
   
}

public enum TypeTask
{
    Vegetables, Kills, HeadShots, BuyWeapon, BuyAmmo, EnterPlace, FindItem, AliveDate, UseItem, KillBoss, EarnMoney, SpendMoney
}
public enum SubTask
{
    Carrot, Onion, Tomato, Pumpkin, Squash, Zombie, HeadShot, Revolver, Pistol, AKM, ammoRevolver, Uzi, Non
}
