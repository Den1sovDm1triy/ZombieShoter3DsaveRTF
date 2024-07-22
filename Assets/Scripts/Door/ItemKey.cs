using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemKey : MonoBehaviour, IItem
{
    [SerializeField] private Outline outline;
    public int keyId;
    public static Action<Vector3, ItemKey> onTakeKey;
    public string description;
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

    public void TakeItem(bool isSound)
    {
        onTakeKey?.Invoke(transform.position, this);
        Destroy(this.gameObject);
    }

    
    

   
}
