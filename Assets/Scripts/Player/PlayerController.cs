using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public static Action onAttack;
    public static Action <float> onAttackSound;
    public static Action onUse;
    private bool isActive = true;
    public float currentWeaponDistance=40;
    private bool canAttack = true;
    private float attackDelay = 0.2f;
    private bool hasHitEnemy=false;
    private bool islock = false;
    private void Start()
    {
        PlayerHealth.onDeath += Deactivate;
        WeaponManager.onChangeWeapon += SetDistance;
        /*
#if UNITY_EDITOR
        Cursor.visible = false;
               
        Cursor.lockState = CursorLockMode.Locked;
#endif*/

    }

    private void OnDestroy()
    {
        PlayerHealth.onDeath -= Deactivate;
        WeaponManager.onChangeWeapon -= SetDistance;
    }

    public void Use()
    {
        if (isActive)
            onUse?.Invoke();
    }

    public void Attack()
    {
        if (isActive)
        {
            onAttack?.Invoke();
            onAttackSound?.Invoke(currentWeaponDistance+10f);
        }
    }

    private void Deactivate()
    {
        isActive = false;
    }

    private void SetDistance(WeaponName weaponName, WeaponModel weaponModel)
    {
        if (weaponModel != null)
        {
            currentWeaponDistance = weaponModel.GetShootDistance();
            if (weaponName == WeaponName.Revolver && weaponName == WeaponName.ShotGun)
            {
                attackDelay = 0.2f;
            }
            else attackDelay = 0;
        
        }
        else currentWeaponDistance = 0;
    }

    private void Update()
    {
        if (currentWeaponDistance != 0)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, currentWeaponDistance))
            {
                if (hit.collider.CompareTag("Enemy")|| hit.collider.CompareTag("EnemyHead"))
                {
                    if (!hasHitEnemy)
                    {
                        hasHitEnemy = true;
                        StartCoroutine(AttackAfterDelay(attackDelay)); 
                    }
                    else if (canAttack)
                    {
                        Attack();
                    }
                }
                else
                {
                    hasHitEnemy = false;
                    StopAllCoroutines();
                }
            }
            else
            {
                hasHitEnemy = false;
                StopAllCoroutines();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Use();
        }        
    }

    private IEnumerator AttackAfterDelay(float delay)
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        Attack();
        canAttack = true;
    }



}


