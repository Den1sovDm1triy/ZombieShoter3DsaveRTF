using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHitPoolElement : PoolElement
{
      
    private void OnEnable()
    {
        this.StartCoroutine(LifeRoutine());
    }
    private void OnDisable()
    {
        this.StopCoroutine(LifeRoutine());
    }    

    private IEnumerator LifeRoutine()
    {
        yield return null;
        transform.SetParent(poolParent);
        yield return new WaitForSeconds(lifetime);
        this.Deactivate();
    }

    private void Deactivate()
    {
        transform.SetParent(poolParent);
        this.gameObject.SetActive(false);
    }
}
