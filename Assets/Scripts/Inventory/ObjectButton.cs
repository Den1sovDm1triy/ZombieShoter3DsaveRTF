using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class ObjectButton : MonoBehaviour
{
    public static Action <ObjectButton> onClickObjectButton;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Image objectImage;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Button button;
    [SerializeField] public InteractObj curObj;    
    [SerializeField] private TextMeshProUGUI countText;
    public bool isPressed = false;


    private void Start()
    {
        button.onClick.AddListener(ClickOnButton);
    }

    private void ClickOnButton()
    {
       onClickObjectButton?.Invoke(this);
    }


    public void AddCount(int count)
    {
        curObj.Add(count);
        countText.text = curObj.count.ToString();
    }

   

    public void Press()
    {
        if (isPressed)
        {
            UnpressButton();
        }
        else
        {
            PressButton();
        }
    }

    public void PressButton()
    {
        isPressed = false;
        buttonImage.sprite = pressedSprite;
    }

    public void UnpressButton()
    {
        buttonImage.sprite = normalSprite;
        isPressed = true;
    }



    public void Init(InteractibleObjects interactibleObject, int count)
    {
        if (interactibleObject == null) return;
        curObj = new InteractObj(interactibleObject, count);
        if(curObj.objSprite!=null)
        objectImage.sprite = curObj.objSprite;
        objectImage.color = new Color(255, 255, 255, 255);
        countText.text = curObj.count.ToString();

    }

    public void Deinit()
    {
        curObj.typeInteractible = TypeInteractible.none;
        objectImage.sprite = null;
        objectImage.color = new Color(0, 0, 0, 0);
        countText.text = "";
    }



}

[Serializable]
public class InteractObj
{
    public Sprite objSprite;
    public TypeInteractible typeInteractible;
    public int count;

    public InteractObj(InteractibleObjects interactibleObjects, int count)
    {
        if(interactibleObjects!=null)
        objSprite = interactibleObjects.objectsSprite;
        typeInteractible = interactibleObjects.typeInteractible;
        this.count = count;
    }

    public void Add(int count)
    {
        this.count += count;
        if (count <= 0) count = 0;
    }
}
