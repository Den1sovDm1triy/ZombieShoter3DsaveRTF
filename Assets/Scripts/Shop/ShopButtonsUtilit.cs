using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonsUtilit : MonoBehaviour
{
    public List<ShopButton> buttons;


    private void Awake()
    {
        ShopButton.onClickMark += ButtonClicked;

    }

    private void OnEnable()
    {
        buttons[0].Mark();
        buttons[1].UnMark();
    }

    private void OnDestroy()
    {
        ShopButton.onClickMark -= ButtonClicked;
    }



    private void ButtonClicked(ShopButton shopButton)
    {
        foreach (var b in buttons)
        {
            if (b == shopButton)
            {
                b.Mark();
            }
            else b.UnMark();
        }
    }

}
