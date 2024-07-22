using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float moveSpeed;
    public float biasSpeed;
    public int startHealth;  
  
}
