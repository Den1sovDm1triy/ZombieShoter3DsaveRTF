using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopButton : MonoBehaviour
{
    public  Button button;
    [SerializeField] GameObject mark;
    [SerializeField] Color activeColor;
    [SerializeField] Color passiveColor;
    [SerializeField] TextMeshProUGUI buttonText;
    public static Action<ShopButton> onClickMark;


    private void Awake()
    {
        button.onClick.AddListener(ClickMark);
    }

    private void ClickMark()
    {
        onClickMark?.Invoke(this);
    }

    public void Mark()
    {
        buttonText.color = activeColor;
        mark.SetActive(true);
    }

    public void UnMark()
    {
        buttonText.color = passiveColor;
        mark.SetActive(false);
    }
}
