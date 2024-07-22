using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Slider slider;

    private void Start()
    {
        Invoke(nameof(Init), 1);
        enemyHealth.onNewHealth += onChangeValue;
    }
    private void OnDestroy()
    {
        enemyHealth.onNewHealth -= onChangeValue;
    }


    private void Init()
    {
        slider.maxValue = enemyHealth.heath;
        slider.value = enemyHealth.heath;
    }

    private void onChangeValue(float newHealth)
    {
        slider.value = newHealth;
        if (newHealth <= 0)
            slider.gameObject.SetActive(false);

    }
}
