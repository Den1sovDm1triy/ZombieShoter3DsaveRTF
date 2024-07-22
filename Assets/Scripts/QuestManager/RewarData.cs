using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RewardData", menuName = "RewardData", order = 3)]
public class RewardData : ScriptableObject
{
    public string rewardDescription;
    public TypeReward typeReward;
    public Sprite rewardSprite;
    public int countReward;
}

public enum TypeReward
{
    MoneyReward, ItemReward,
}
