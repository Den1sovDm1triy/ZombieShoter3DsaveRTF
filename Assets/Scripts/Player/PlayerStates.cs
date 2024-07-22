using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
   public static bool isCanMove=true;
   public static bool isNeedMove = true;
   public static bool isAlive = true;
    private void Start()
    {
        isCanMove = true;
        isNeedMove = true;
        isAlive = true;
        PlayerHealth.onDeath += Dead;
    }
    private void OnDestroy()
    {
        PlayerHealth.onDeath -= Dead;
    }
    private void Dead()
    {
        isCanMove = false;
        isNeedMove = false;
        isAlive = false;
    }
}
