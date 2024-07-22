using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class WeaponItem : Saver,IItem
{
    public static Action<WeaponName, int, GameObject> onTakeWeapon;
    public Action onTake;
    [SerializeField] private UnityEvent OnTake;
    [SerializeField] public WeaponName weaponName;
    [SerializeField] private int ammoCount;
    [SerializeField] private Outline outline;
    

   


    public void TakeItem(bool isSound)
    {
        onTake?.Invoke();
        OnTake?.Invoke();
        this.gameObject.SetActive(false);
        onTakeWeapon?.Invoke(weaponName, ammoCount, this.gameObject);        
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

public enum WeaponName{

    ShotGun, Revolver, Axe, Uzi, AKM, Pistol, ChainSaw, nothing,
}
