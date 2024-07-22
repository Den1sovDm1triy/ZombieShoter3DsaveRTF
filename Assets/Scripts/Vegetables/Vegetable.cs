using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Vegetable : MonoBehaviour
{
    public UnityEvent OnUse;
    public static Action <VegetablesType, int>onChangeCount;
    public int count;
    public float healPoints;
    [SerializeField] GameObject vegetableGameObject;
    public  VegetablesType type;

    private void Start()
    {
        count = 0;
    }

    public void ChangeCount(int newCount)
    {
        count = count + newCount;
        onChangeCount?.Invoke(type, count);
    }

    public  void Active()
    {
        Invoke(nameof(Activate), 0.1f);
    }

    public void Activate()
    {
        Debug.Log("activate");
        vegetableGameObject.transform.position = vegetableGameObject.transform.position + vegetableGameObject.transform.up * 2;    
    }

    public void Use()
    {
        count--;        
        PlayerHealth.onHeal(healPoints);
        OnUse?.Invoke();
        onChangeCount?.Invoke(type, count);
    }

    public void Deactivate()
    {
        Debug.Log("DEactivate");
        vegetableGameObject.transform.position = vegetableGameObject.transform.position - vegetableGameObject.transform.up* 2;       
    }

}
