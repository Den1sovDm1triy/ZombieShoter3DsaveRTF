
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCounterUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI magazineProgressor;
    [SerializeField] private TextMeshProUGUI ammoProgressor;
    [SerializeField] private GameObject infoObject;

    [SerializeField] WeaponModel weaponModel;
    private void Awake()
    {
        infoObject.SetActive(false);
        WeaponManager.onChangeWeapon += ChangeWeapon;          
    }

    private void OnDestroy()
    {
        WeaponManager.onChangeWeapon -= ChangeWeapon;
    }


    private void ChangeWeapon(WeaponName weaponName, WeaponModel _weaponModel)
    {

        if (_weaponModel == null)
        {
            infoObject.SetActive(false);
            return;
        }
       
        if (_weaponModel.weaponType == WeaponType.range)
        {
            infoObject.SetActive(true);
            if (weaponModel != null && weaponModel.onChangeMagazineCount != null)
            {
                weaponModel.onChangeMagazineCount -= OnBulletAmountChange;
            }
            if (weaponModel != null && weaponModel.onChangeAmmoCount != null)
            {
                weaponModel.onChangeAmmoCount -= OnAmmoAmoutChange;
            }
            if (weaponModel != null && weaponModel.onTakeAmmo != null)
            {
                weaponModel.onTakeAmmo -= OnTakeAmmo;
            }

            this.weaponModel = _weaponModel;
            weaponModel.onChangeMagazineCount += OnBulletAmountChange;
            weaponModel.onChangeAmmoCount += OnAmmoAmoutChange;
            weaponModel.onTakeAmmo += OnTakeAmmo;
        }
        else
        {
            infoObject.SetActive(false);
        }
    }

     private void OnDisable()
    {
        if (weaponModel != null && weaponModel.onChangeMagazineCount != null)
        {
            weaponModel.onChangeMagazineCount -= OnBulletAmountChange;            
        }
        if (weaponModel != null && weaponModel.onChangeAmmoCount != null)
        {
            weaponModel.onChangeAmmoCount -= OnAmmoAmoutChange;
        }
    }

    private void OnBulletAmountChange(int amount)
    {       
        magazineProgressor.text =  amount.ToString();
    }

    private void OnAmmoAmoutChange(int amount)
    {
        ammoProgressor.text = amount.ToString();
    }
    private void OnTakeAmmo(int amount)
    {
        ammoProgressor.text = amount.ToString();
    }
}

