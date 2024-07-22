using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponAnimation : MonoBehaviour
{
    private readonly int hashTriggerShoot = Animator.StringToHash("Shoot");   
    private readonly int hashTriggerReload = Animator.StringToHash("Reload");
    private readonly int hashTriggerChange= Animator.StringToHash("Change");   
    private Weapon weapon;

    private Animator anim;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        anim = GetComponentInChildren<Animator>();        
    }

    private void Start()
    {
        weapon.onWeaponShoot += Shoot;      
        weapon.onWeaponReload += Reload;
        weapon.onWeaponChange += Change;

    }

    private void OnDestroy()
    {
        weapon.onWeaponShoot -= Shoot;       
        weapon.onWeaponReload -= Reload;
        weapon.onWeaponChange -= Change;      
    }


    private void LooseWeapon()
    {
        anim.enabled = false;
    }

    private void Shoot(int capasity)
    {       
        anim.SetTrigger(hashTriggerShoot);            
    }
   
    private void Reload()
    {       
      anim.SetTrigger(hashTriggerReload);
      anim.ResetTrigger(hashTriggerShoot);
    }

    private void Change()
    {
        anim.SetTrigger(hashTriggerChange);         
    }



}
