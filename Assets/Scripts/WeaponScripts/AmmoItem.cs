using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class AmmoItem : MonoBehaviour, IItem
{
    public static Action <WeaponName, int, GameObject> onTakeAmmo;
    
   
    [SerializeField] private int ammoCount;        
    [SerializeField] public WeaponName weaponName;
    [SerializeField] private Outline outline;
    
    public void Init(int ammo)
    {
        ammoCount = ammo;
    }


    public void TakeItem(bool isSound)
    {
        onTakeAmmo?.Invoke(weaponName, ammoCount, this.gameObject);       
        this.gameObject.SetActive(false);        
    }

    public void DropItem()
    {
        
    }

    public void Indicate()
    {
        outline.enabled = true;
    }

    public void StopIndicate()
    {
        outline.enabled = false;
    }
}
