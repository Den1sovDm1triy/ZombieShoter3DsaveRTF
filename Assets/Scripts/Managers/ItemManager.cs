using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemManager : Saver
{   
    public UnityEvent OnActivateItem, OnDeactivateItem, OnTakeItem;

    public List<Vegetable> vegetables = new List<Vegetable>();   
    public static Action <VegetablesItem> onActivateItem;
    public Vegetable currentVegetable;
    public static Action <VegetablesType> onEndEmptyItem;
    public Button eatButton;
    [SerializeField] private List<VegetablesItem> itemsForRecovery;

    public static ItemManager Instance;

    int count = 0;
    public override void Save()
    {
        foreach(var v in vegetables)
        {
            SaveData.SetInt(v.type.ToString() + "count", v.count);
        }
    }

    public override void Load()
    {
        if (count != 0) return;
        Debug.Log("!!!!!!!!!!!");
        foreach (var v in vegetables)
        {
            int count = SaveData.GetInt(v.type.ToString() + "count");  
            if(count>0)
                foreach(var i in itemsForRecovery)
                {
                    if (i.type == v.type)
                    {
                        InventoryManager.onTakeItem(i, false);
                        v.ChangeCount(count - 1);
                    }                   
                }
           
        }
        count++;
    }

    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }



    private void Start()
    {


        InventoryManager.onTakeItem += TakeItem;
        UIManager.onChooseItem += ChooseItem;
        foreach (var w in vegetables)
        {
            w.Deactivate();
        }
        currentVegetable = null;
        PlayerController.onUse += UseItem;        
    }

    


    public override void OnDestroy()
    {
        base.OnDestroy();
        PlayerController.onUse -= UseItem;
        InventoryManager.onTakeItem -= TakeItem;
        UIManager.onChooseItem -= ChooseItem;
    }

    

    private void TakeItem(VegetablesItem vegetablesItem, bool isSound)
    {
        StartCoroutine(Take(vegetablesItem, isSound));
    }


   

    private IEnumerator Take(VegetablesItem vegetablesItem, bool isSound)
    {        
        yield return null;
        foreach (var i in vegetables)
        {
            if (vegetablesItem.type == i.type)
            {
                i.ChangeCount(1);
                if (isSound)
                {
                    Debug.Log("isSound" + isSound);
                    OnTakeItem?.Invoke();
                }              
            }
        }
    }
        

    public bool HasItem(VegetablesType vegetablesType)
    {
        bool ishas = false;
        foreach(var i in vegetables)
        {
            if (vegetablesType == i.type)
            {                
                if (i.count == 0)
                {                    
                    ishas = false;
                    break;
                }
                else { ishas = true;
                    break;
                }
            }
        }
        return ishas;
       
    }



    private void ChooseItem(VegetablesType vegetablesType)
    {
        if (currentVegetable == null) 
        {
            OnActivateItem?.Invoke();
            Activate(vegetablesType);
            if(SaveData.Has("educationshop"))
            eatButton.gameObject.SetActive(true);
            
        }
        else if (vegetablesType == currentVegetable.type)
        {
            Deactivate(vegetablesType);
            currentVegetable = null;
            eatButton.gameObject.SetActive(false);
        }
        else if (vegetablesType != currentVegetable.type)
        {
            currentVegetable.Deactivate();
            OnActivateItem?.Invoke();
            Activate(vegetablesType);
            if(SaveData.Has("educationshop"))
                eatButton.gameObject.SetActive(true);
        }
    }

    private void UseItem()
    {
        if (currentVegetable != null)
        {           
            currentVegetable.Use();
            Deactivate(currentVegetable.type);
            if (currentVegetable.count > 0)
            {
                Activate(currentVegetable.type);
            }
            else
            {
                onEndEmptyItem?.Invoke(currentVegetable.type);
                currentVegetable = null;
                eatButton.gameObject.SetActive(false);
            }
        }
    }


    public void SellItem(VegetablesType vegetablesType, int count)
    {
        foreach(var v in vegetables)
        {
            if (v.type == vegetablesType)
            {
                v.ChangeCount(count);
                if (v.count == 0)
                {
                    onEndEmptyItem?.Invoke(v.type);
                }

            }
        }
    }


    private void Activate(VegetablesType vegetablesType) 
    { 
        foreach (var i in vegetables)
        {
            if (vegetablesType == i.type)
            {

                i.Active();
                currentVegetable = i;                
            }
        }
    }

    private void Deactivate(VegetablesType vegetablesType)
    {
        foreach (var i in vegetables)
        {
            if (vegetablesType == i.type)
            {
                i.Deactivate();               
                OnDeactivateItem?.Invoke();
            }
        }
    }


    
}
