using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Line : MonoBehaviour
{    
    public int maxcapacity;
    public int curCaracity;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private GameObject emptyCard;


    public void SetMaxCapacity(int capacity)
    {
        maxcapacity = capacity;
    }
   
    public void Setup(Good good)
    {
        if (good != null)
        {
            if (curCaracity < maxcapacity)
            {
                var card = Instantiate(cardPrefab, transform);
                card.Setup(good);
                curCaracity++;
            }
        }
        else
        {
            if (curCaracity < maxcapacity)
            {
                var card = Instantiate(emptyCard, transform);
            }
        }
    }


    public bool isEmpty()
    {
        return curCaracity < maxcapacity;
    }
        

}
