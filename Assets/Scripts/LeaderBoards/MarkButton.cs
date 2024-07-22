using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MarkButton : MonoBehaviour
{
    [SerializeField] private GameObject mark;
    public Button button;
    public static Action<MarkButton> onClickMark;

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
        mark.SetActive(true);
    }

    public void UnMark()
    {
        mark.SetActive(false);
    }
}
