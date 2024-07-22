using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAnimationEvents : MonoBehaviour
{
    public Action onStepLeft, onStepRight;
    public Action onActionDie1, onActionDie2;



    public void StepLeft()
    {
        Debug.Log("Left");
        onStepLeft?.Invoke();
    }
    public void StepRight()
    {
        onStepRight?.Invoke();
    }

    public void Die1Stet()
    {
        onActionDie1?.Invoke();
    }
    public void Die2Stet()
    {
        onActionDie2?.Invoke();
    }
}
