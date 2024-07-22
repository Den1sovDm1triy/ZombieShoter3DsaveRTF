using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class FlameThrower : InteractibleObjects
{
    public static Action<float> onUpdateFuel;
    [SerializeField] private int damage;
    [SerializeField] private List<GameObject> flamePool;
    [SerializeField] private Transform pointOfShoot;
    public float flameDistance = 70f;
    public bool isNeedFire;
    private Coroutine flameCor;
    private Coroutine fuelCor;
    public float fuelCount;
    private bool canUse=true;
    [SerializeField] private TextMeshProUGUI fuelText;

    private void Start()
    {
        fuelCount = ObjectManager.Instance.GetFuelCountForThrower();
    }
    public override void Activate()
    {
        
        if (!IsHasFuel()) 
        {            
            return; 
        }
        base.Activate();
        if (flameCor == null)
            flameCor = StartCoroutine(FireCor());
        if (fuelCor == null)
            fuelCor = StartCoroutine(FuelCor());
    }

    public override void InHand()
    {
        base.InHand();
        fuelCount = ObjectManager.Instance.GetFuelCountForThrower();
        fuelText.text = ((int)fuelCount).ToString();
    }
    public bool IsHasFuel()
    {
        if (fuelCount > 0) return true;
        else return false;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (flameCor != null)
        {
            StopCoroutine(flameCor);
            flameCor = null;
        }
        if (fuelCor != null)
        {
            StopCoroutine(fuelCor);
            fuelCor = null;
        }
    }

    private IEnumerator FuelCor()
    {
        while (fuelCount>0)
        {
            fuelCount -= 0.01f;
            onUpdateFuel?.Invoke(fuelCount);
            fuelText.text = ((int)fuelCount).ToString();
            if (fuelCount <= 0)
            { 
                fuelCount = 0;                
                canUse = false;
                Deactivate();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }




    private IEnumerator FireCor()
    {
        while (true)
        {
            isNeedFire = true;
            yield return null; 
            isNeedFire = false;
            yield return new WaitForSeconds(0.7f);
        }
    }


    private void Update()
    {
        if (!isActive) return;

        Ray ray = new Ray(pointOfShoot.position, pointOfShoot.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, flameDistance))
        {
            if (isNeedFire)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy+++++++++++");
                    EnemyHealth curEnemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
                    if (curEnemyHealth != null)
                        curEnemyHealth.TakeDamage(damage);
                    if (curEnemyHealth.heath > 0)
                    {
                        foreach (var f in flamePool)
                        {
                            if (!f.activeInHierarchy)
                            {
                                f.transform.position = hit.point;
                                f.transform.rotation = Quaternion.LookRotation(hit.normal);
                                f.transform.SetParent(hit.collider.transform);
                                f.SetActive(true);
                                return;
                            }
                        }
                    }
                }

            }
        }

    }
}
