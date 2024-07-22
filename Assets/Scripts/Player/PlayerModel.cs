using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float biasSpeed;
    [HideInInspector] public int startHealth;
   
    public void Awake()
    {
        moveSpeed = playerData.moveSpeed;
        biasSpeed = playerData.biasSpeed;
        startHealth = playerData.startHealth;       
    }
}

