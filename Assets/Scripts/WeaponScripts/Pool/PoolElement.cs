using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolElement : MonoBehaviour
{
    internal Transform poolParent;
   
    [SerializeField] internal float lifetime = 2f;

    

    public void ParentSet(Transform parent)
    {
        this.poolParent = parent;
        this.transform.SetParent(parent);
    }
}
