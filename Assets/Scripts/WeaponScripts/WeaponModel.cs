using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WeaponModel : MonoBehaviour
{
    public Action <int> onChangeMagazineCount;
    public Action<int> onChangeAmmoCount;
    public Action<int> onTakeAmmo;
    [SerializeField] private WeaponData weaponData;
    [HideInInspector] public WeaponAmmo weaponAmmo;
    private int damage;
    private int maxCapasityOfMagazine;
    private int currentCapasityOfMagazine;
    private float shootDelay;
    private float reloadDelay;
    private float shootDistance;
    private bool isfirstInit = true;
    public WeaponType weaponType;
    public WeaponName weaponName;
    [HideInInspector]public AudioClip reloadClip, shootClip, emptyClip, activateClip;
    private void Awake()
    {
        weaponAmmo = GetComponent<WeaponAmmo>();
        damage = weaponData.damage;
        maxCapasityOfMagazine = weaponData.capacityMagazine;
        shootDelay = weaponData.shootDelay;
        reloadDelay = weaponData.reloadDelay;
        shootDistance = weaponData.shootDistance;
        currentCapasityOfMagazine = maxCapasityOfMagazine;
        reloadClip = weaponData.reloadClip;
        shootClip = weaponData.shootClip;
        emptyClip = weaponData.emptyClip;
        activateClip = weaponData.activateClip;
        weaponType = weaponData.weaponType;
        weaponName = weaponData.weaponName;
        if (weaponAmmo.GetAmmoCount() > currentCapasityOfMagazine)
        {
            currentCapasityOfMagazine = maxCapasityOfMagazine;
            weaponAmmo.RelodaAmmo(currentCapasityOfMagazine);
        }
        else
        {
            currentCapasityOfMagazine = weaponAmmo.GetAmmoCount();
            weaponAmmo.RelodaAmmo(currentCapasityOfMagazine);
        }
    }  

    


    public void Init()
    {
        if (isfirstInit)
        {
            isfirstInit = false;
            if (weaponAmmo.GetAmmoCount() > maxCapasityOfMagazine)
            {
                currentCapasityOfMagazine = maxCapasityOfMagazine;
                weaponAmmo.RelodaAmmo(currentCapasityOfMagazine);
            }
            else
            {
                currentCapasityOfMagazine = weaponAmmo.GetAmmoCount();
                weaponAmmo.RelodaAmmo(currentCapasityOfMagazine);
            }
        }
        onChangeMagazineCount?.Invoke(currentCapasityOfMagazine);
        onChangeAmmoCount?.Invoke(weaponAmmo.GetAmmoCount());
    }

    public void Shoot()
    {
        currentCapasityOfMagazine--;
        onChangeMagazineCount?.Invoke(currentCapasityOfMagazine);
    }

    public int CurrentCapacity()
    {
        return currentCapasityOfMagazine;
    }

    public void ReloadCapasity()
    {
        if (weaponAmmo.GetAmmoCount() > currentCapasityOfMagazine)
        {
            currentCapasityOfMagazine = maxCapasityOfMagazine;
            weaponAmmo.RelodaAmmo(maxCapasityOfMagazine);
            onChangeAmmoCount?.Invoke(weaponAmmo.GetAmmoCount());
        }
        else
        { 
            currentCapasityOfMagazine = weaponAmmo.GetAmmoCount();
            weaponAmmo.RelodaAmmo(currentCapasityOfMagazine);
            onChangeAmmoCount?.Invoke(weaponAmmo.GetAmmoCount());
        }
        onChangeMagazineCount?.Invoke(currentCapasityOfMagazine);
    }

    public float DelayOfShoot()
    {
        return shootDelay;
    }
    public float DelayOfReload()
    {
        return reloadDelay;
    }
    public int GetDamage()
    {
        return damage;
    }
    public float GetShootDistance()
    {
        return shootDistance;
    }

    public int GetMaxCapasity()
    {
        return maxCapasityOfMagazine;
    }
    public void TakeAmmo(int ammo)
    {
        weaponAmmo.TakeAmmo(ammo);
        Debug.Log("weapon model take ammo" + ammo);
        onTakeAmmo?.Invoke(weaponAmmo.GetAmmoCount());
        Debug.Log("weapon model take ammo גסודמ אללמ" + weaponAmmo.GetAmmoCount());
    }

}
