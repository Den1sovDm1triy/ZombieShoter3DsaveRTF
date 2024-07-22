using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    
    public List<MarkButton> buttons;


    private void Awake()
    {
        MarkButton.onClickMark += ButtonClicked;
    }

    private void OnDestroy()
    {
        MarkButton.onClickMark -= ButtonClicked;
    }

    
    
    private void ButtonClicked(MarkButton markButton)
    {
        foreach (var b in buttons)
        {
            if (b == markButton)
            {
                b.Mark();
            }
            else b.UnMark();
        }
    }

}
