using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Card : MonoBehaviour
{
    public static Action<Card> onInitCard;
    public static Action<Card> onClickCard;
    [SerializeField] private Button cardButton;
    public Good good;
    [SerializeField] Image clickMark;



    [SerializeField] private Image goodImage;
    [SerializeField] private Image secondImage;
    [SerializeField] private TextMeshProUGUI nameGoodsText;
    [SerializeField] private TextMeshProUGUI priceText;    
   
    private void Start()
    {
        cardButton.onClick.AddListener(ClickOnCard);
    }


    private void OnDestroy()
    {
        cardButton.onClick.RemoveAllListeners();
    }

    public void ClickOnCard()
    {
        onClickCard?.Invoke(this);
        clickMark.enabled = true;
    }

    public void NotInteractibleButton()
    {
        cardButton.interactable = false;
    }
    public void InteractibleButton()
    {
        cardButton.interactable = true;
    }


    public void UnClick()
    {
        clickMark.enabled = false;
    }


    public void Setup(Good good)
    {
        this.good = good;
        goodImage.sprite = good.sprite;
        if (good.typegood == Typegood.Ammo)
        {
            secondImage.sprite = good.secondImage;
            secondImage.color = new Color(1, 1, 1, 1);
        }
        nameGoodsText.text = good.name;
        priceText.text = good.price.ToString();        
        onInitCard?.Invoke(this);
    }


    public void Trade(int _count)
    {
        good.count -= _count;        
    }


}
[Serializable]
public class Good
{
    public  string name;
    public string nameForTitle;
    public Sprite sprite;
    public Sprite secondImage;
    public int price;
    public int count;
    public Typegood typegood;
    public int specification;
    public Good(GoodData gooddata, int _count)
    {
        name = gooddata.nameGoods;
        nameForTitle = gooddata.nameTitle;
        sprite = gooddata.spriteGoods;
        if(gooddata.typegood==Typegood.Ammo)
            secondImage = gooddata.secondimage;
        price = gooddata.price;
        count =_count;
        typegood = gooddata.typegood;
        specification = gooddata.specification;
    }
    public Good(GoodData gooddata)
    {
        name = gooddata.nameGoods;
        nameForTitle = gooddata.nameTitle;
        sprite = gooddata.spriteGoods;
        if (gooddata.typegood == Typegood.Ammo)
            secondImage = gooddata.secondimage;
        price = UnityEngine.Random.Range((int)gooddata.pricePeriod.x, (int)gooddata.pricePeriod.y);
        count = 1;
        typegood = gooddata.typegood;
        specification = gooddata.specification;
    }
    public Good(GoodData gooddata, int todayprice, int _count)
    {
        name = gooddata.nameGoods;
        nameForTitle = gooddata.nameTitle;
        sprite = gooddata.spriteGoods;
        if (gooddata.typegood == Typegood.Ammo)
            secondImage = gooddata.secondimage;
        price = todayprice;
        count = _count;
        typegood = gooddata.typegood;
        specification = gooddata.specification;
    }
}
