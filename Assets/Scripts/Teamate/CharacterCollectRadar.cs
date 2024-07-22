using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CharacterCollectRadar : MonoBehaviour
{
    public Action<VegetablesItem> onFindVegetables;
    public Action onRelax;
    public List<VegetablesItem> vegetables = new List<VegetablesItem>();
    public VegetablesItem nearestTarget;
    public SphereCollider sphereCollider;
    public bool isActive;


    private void Awake()
    {
        Deactivate();
    }

    private void Start()
    {
        VegetablesItem.onTakeVegetable += CheckVegetables;        
    }

    private void OnDestroy()
    {
        VegetablesItem.onTakeVegetable -= CheckVegetables;       
    }

    private void CheckVegetables(VegetablesItem veg, GameObject vegObject, bool isSound)
    {
        if (vegetables.Contains(veg))
        {
            vegetables.Remove(veg);
        }
    }

    public void GrowSphere(int I)
    {
        vegetables.Clear();       
    }

   

    public void Activate()
    {
        isActive = true;
        sphereCollider.enabled = true;
        GrowSphere(0);
        StartCoroutine(targetFinder());
    }

    public void Deactivate()
    {
        isActive = true;
        sphereCollider.enabled = false;
        vegetables.Clear();
        StopAllCoroutines();
    }
    public void Stop()
    {
        isActive = true;       
        StopAllCoroutines();
    }


    IEnumerator targetFinder()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(0.1f);

            if (vegetables.Count > 0)
            {
                float closestDistance = float.MaxValue;
                VegetablesItem closestVegetable = null;

                foreach (var e in vegetables)
                {
                    if (e != null) 
                    { 
                        float distance = Vector3.Distance(transform.position, e.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestVegetable = e;
                        }
                    }
                }
                nearestTarget = closestVegetable;
                if (nearestTarget != null)
                {                    
                    onFindVegetables?.Invoke(nearestTarget);                       
                }
            }
            else onRelax?.Invoke();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            VegetablesItem veg = other.GetComponent<VegetablesItem>();
            if (veg != null)
            {               
               if (!vegetables.Contains(veg))
                        vegetables.Add(veg);
               
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            VegetablesItem veg = other.GetComponent<VegetablesItem>();
            if (veg != null)
            {
                if (!vegetables.Contains(veg))
                    vegetables.Add(veg);

            }
        }
    }
}
