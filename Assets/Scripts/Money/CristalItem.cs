using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class CristalItem :MonoBehaviour, IItem
{
    public static Action<Vector3, int> onTakeCristal;   
    [SerializeField] private Outline outline;
    
    [SerializeField] private int amount;
    bool first;
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
        if (!first)
        {
            first = true;
            onTakeCristal?.Invoke(transform.position, amount);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
          transform.DORotate(new Vector3(0, 360, 0), 3.0f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
}

    
