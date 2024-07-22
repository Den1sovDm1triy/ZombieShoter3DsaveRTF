using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Good", menuName = "Custom/Good")]
public class GoodData : ScriptableObject
{
    public int price;
    public Vector2 pricePeriod;
    public Sprite spriteGoods;
    public Sprite secondimage;
    public string nameGoods;
    public string nameTitle;
    public Typegood typegood;
    public int specification;
}

public enum Typegood
{
    Ammo, Vegetables, Weapon, Interactible, Upgrade,
}
