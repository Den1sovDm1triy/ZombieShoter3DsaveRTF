using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class MoneyItem : MonoBehaviour, IItem
{
    public static Action<Vector3, int> onTakeMoney;
    [SerializeField] private int amount;
    [SerializeField] private Outline outline;
    bool first;
    public void DropItem()
    {
        
    }

    public void Setup(int count)
    {
        amount = count;  
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
        if (!first)
        {
            first = true;
            onTakeMoney?.Invoke(transform.position, amount);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
          transform.DORotate(new Vector3(0, 360, 0), 1.0f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }

    
    
}
