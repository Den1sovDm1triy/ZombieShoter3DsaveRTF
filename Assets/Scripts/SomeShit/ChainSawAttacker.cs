using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ChainSawAttacker : MonoBehaviour
{
    [SerializeField] private bool isNeedFire = false;
    private Coroutine fireCor;
    [SerializeField] private int damage=50;
    [SerializeField] private List<GameObject> bloodPool;
    [SerializeField] Transform chainsawObject;
    private Tween shakeTween;
    private void OnTriggerEnter(Collider other)
    {
        if (!isNeedFire) return;
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHead")) 
        {
            EnemyHealth curEnemyHealth = other.transform.GetComponentInParent<EnemyHealth>();
            if (curEnemyHealth != null)
            {
                curEnemyHealth.TakeDamage(damage);
                foreach(var v in bloodPool)
                {
                    v.SetActive(true);
                }
                
            }
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isNeedFire) return;
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHead"))
        {
            EnemyHealth curEnemyHealth = other.transform.GetComponentInParent<EnemyHealth>();
            if (curEnemyHealth != null)
            {
                curEnemyHealth.TakeDamage(damage);
                foreach (var v in bloodPool)
                {
                    v.SetActive(true);
                }
            }
            
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHead"))
        {
            foreach (var v in bloodPool)
            {
                v.SetActive(false);
            }
        }
    }

    public void Activate()
    {
        if (fireCor == null)
            fireCor = StartCoroutine(FireCor());
        Vibrate();
    }

    private void Vibrate()
    {
        shakeTween =chainsawObject.DOShakePosition(0.1f, new Vector3(0.001f, 0.001f, 0.001f), 1, 1f).SetLoops(-1);
    }
    private void StopVibrate()
    {
        shakeTween.Kill();
    }

    public void Deactivate()
    {
        if (fireCor != null)
        {
            StopCoroutine(fireCor);
            fireCor = null;
        }
        StopVibrate();
    }

    private IEnumerator FireCor()
    {
        while (true)
        {
            isNeedFire = true;
            yield return new WaitForSeconds(0.1f);
            isNeedFire = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
