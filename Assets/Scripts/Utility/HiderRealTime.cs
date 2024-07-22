using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiderRealTime : MonoBehaviour
{
    [SerializeField] private float timer;
    public void OnEnable()
    {       
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSecondsRealtime(timer);
        this.gameObject.SetActive(false);
    }
}
