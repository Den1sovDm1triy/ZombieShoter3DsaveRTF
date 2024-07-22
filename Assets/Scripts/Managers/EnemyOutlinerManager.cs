using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOutlinerManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyOutLine>().Indicate();            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyOutLine>().Indicate();            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyOutLine>().StopIndicate();           
        }
    }
}
