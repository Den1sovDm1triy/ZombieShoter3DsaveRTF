using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealthUI : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int startHealth;
    private void Awake()
    {
        PlayerHealth.onBarUpdate += ChangeHealth;
        PlayerHealth.onBarCalibrate+= CalibrateHealth;        
    }

    private void OnDestroy()
    {
        PlayerHealth.onBarUpdate -= ChangeHealth;
        PlayerHealth.onBarCalibrate -= CalibrateHealth;
    }


    private void CalibrateHealth(float health)
    {
        slider.maxValue = health;
        startHealth = (int)health;
        slider.value = health;
        healthText.text = ((int)health).ToString() + "/" + startHealth.ToString();
    }


    private void ChangeHealth(float health)
    {
        slider.value = health;
        healthText.text = ((int)health).ToString() + "/" + startHealth.ToString();
    }
}
