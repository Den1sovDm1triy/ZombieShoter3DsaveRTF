using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractibleTeammate : MonoBehaviour
{
    public SphereCollider sphereCollider;
    public float recuitRadius, firedRadius;
    public Action onEnter;
    public Action onExit;

    public bool isEnter = false;


    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();        
    }

    public void RecuitRadius()
    {
        sphereCollider.radius = recuitRadius;
    }

    public void FiredRadius()
    {
        sphereCollider.radius = firedRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isEnter)
            {
                isEnter = true;
                onEnter?.Invoke();
               
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isEnter)
            {
                isEnter = false;
                onExit?.Invoke();                
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isEnter)
            {
                isEnter = true;
                onEnter?.Invoke();                
            }
        }
    }
}
