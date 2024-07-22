using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PoolMono<T> where T : PoolElement
{ 

    public List <T> prefabList
    {
        get;
    }
    public bool autoExpand
    {
        get; set;
    }
    public Transform container
    {
        get;
    }

    private List<T> pool;
   
    public PoolMono(List<T> prefabList, int count)
    {
        this.prefabList = prefabList;
        this.container = null;
        this.CreatePool(count);
    }

    public PoolMono(List<T> prefabList, int count, Transform container)
    {
        this.prefabList = prefabList;
        this.container = container;
        this.CreatePool(count);
    }


  

    private void CreatePool(int count)
    {
        this.pool = new List<T>();
        for(int i=0; i<count; i++)
        {
            this.CreateObject();
        }        
    }
  
    private T CreateObject(bool isActiveByDefault=false)
    {
        var createdObject = UnityEngine.Object.Instantiate(this.prefabList[UnityEngine.Random.Range(0, prefabList.Count)], this.container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        this.pool.Add(createdObject);       
        createdObject.ParentSet(container);
        return createdObject;
    }

   



    public bool HasFreeElement(out T element)
    {
        foreach(var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }
        element = null;
        return false;
    }
   

    public T GetFreeElement()
    {
        if (this.HasFreeElement(out var element))
            return element;
        if (this.autoExpand)
        {
            return this.CreateObject(true);
        }
        throw new Exception($"There is no free element in pool of type {typeof(T)}");
    }

    

}
