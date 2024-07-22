using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePanel : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    private void Start()
    {
        PlayerHealth.onTakeDamage += ShowDamage;
    }
    private void OnDestroy()
    {
        PlayerHealth.onTakeDamage -= ShowDamage;
    }

    private void ShowDamage()
    {
        canvasGroup.alpha = 1;
        StopAllCoroutines();       
        StartCoroutine(DamageCor());
    }

    private IEnumerator DamageCor()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1.5f * Time.deltaTime;
            if (canvasGroup.alpha < 0) canvasGroup.alpha = 0;
            yield return null;
        }
    }
}
