using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutlinerManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<IItem>().Indicate();            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<IItem>().Indicate();            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<IItem>().StopIndicate();           
        }
    }
}
