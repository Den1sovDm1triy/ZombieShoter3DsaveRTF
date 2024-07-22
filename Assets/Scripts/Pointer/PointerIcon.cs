using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PointerIcon : MonoBehaviour
{
    [SerializeField] Image image;
    bool isShown = true;
    private void Awake()
    {
        image.enabled = false;
        isShown = false;
    }

    public void SetPosition(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Show()
    {
        if (isShown) return;
        isShown = true;
        StopAllCoroutines();
        StartCoroutine(ShowProcess());
    }

    public void Hide()
    {
        if (!isShown) return;
        isShown = false;
        StopAllCoroutines();
        StartCoroutine(HideProcess());
    }

    private IEnumerator ShowProcess()
    {
        image.enabled = true;
        transform.localScale = Vector3.zero;
        for(float t=0; t<1; t += Time.deltaTime * 4f)
        {
            transform.localScale = Vector3.one * t;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
    private IEnumerator HideProcess()
    {
        transform.localScale = Vector3.zero;
        for(float t=0; t < 0.95
            ; t += Time.deltaTime * 4f)
        {
            transform.localScale = Vector3.one * (1f - t);
            yield return null;
        }
        //image.enabled = false;
    }





}
