using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class WeaponCharacter : MonoBehaviour
{
    public GameObject weaponOnBack;    
    public GameObject weaponOnHand;

    public UnityEvent OnShot;
    public Action onShot;
    bool isActive = false;

    public AnimatorIKTeammate animatorIK;


    private void Start()
    {
        animatorIK.onShot += Shot;
    }


    private void Shot()
    {
        OnShot?.Invoke();
        onShot?.Invoke();
    }

    public void Activate()
    {
        isActive = true;
        weaponOnBack.SetActive(false);
        weaponOnHand.SetActive(true);
    }
    public void Deactivate()
    {
        isActive = false;
        weaponOnBack.SetActive(true);
        weaponOnHand.SetActive(false);
    }
}
