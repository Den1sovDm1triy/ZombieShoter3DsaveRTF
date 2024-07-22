using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public bool isWorking=false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if(isWorking)
            other.GetComponent<IItem>().TakeItem(false);            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if(isWorking)
            other.GetComponent<IItem>().TakeItem(false);            
        }
    }
}
