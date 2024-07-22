using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private UnityEvent OnMuzzleShoot;
    [SerializeField] private UnityEvent OnReload;
    [SerializeField] private AudioSource weaponSource;
    Weapon weapon;
       

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        weapon.onWeaponShoot += Shoot;
        weapon.onWeaponReload += Reload;
        weapon.onEmptyWeapon += Empty;
    }

    private void OnDestroy()
    {
        weapon.onWeaponShoot -= Shoot;
        weapon.onWeaponReload -= Reload;
        weapon.onEmptyWeapon -= Empty;
    }

    private void Shoot(int capasity)
    {
        OnMuzzleShoot?.Invoke();        
        weaponSource.PlayOneShot(weapon.weaponModel.shootClip);
    }

    private void Reload()
    {
        weaponSource.PlayOneShot(weapon.weaponModel.reloadClip);
    }
    private void Empty()
    {
        weaponSource.PlayOneShot(weapon.weaponModel.emptyClip);
    }

}
