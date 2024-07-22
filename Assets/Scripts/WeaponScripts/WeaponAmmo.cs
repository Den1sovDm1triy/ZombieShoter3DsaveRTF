using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponAmmo : MonoBehaviour
{
    [SerializeField] private int ammo;

    private void Start()
    {
        ammo = 0;
    }
    
   

    public void TakeAmmo(int _ammo)
    {
        ammo += _ammo;    
    }

    public int GetAmmoCount()
    {
        return ammo;
    }

    public void RelodaAmmo(int ammocount)
    {
        ammo = ammo - ammocount;
    }
}
