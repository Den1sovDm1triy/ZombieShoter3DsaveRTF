using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerPoolElement : PoolElement
{
    public TrailRenderer trailRenderer;        
    
    private void OnEnable()
    {
        this.StartCoroutine(LifeRoutine());
    }
    private void OnDisable()
    {
        trailRenderer.Clear();
        this.StopCoroutine(LifeRoutine());
    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(lifetime);
        this.Deactivate();
    }

    private void Deactivate()
    {
        transform.SetParent(poolParent);
        this.gameObject.SetActive(false);
    }
}
