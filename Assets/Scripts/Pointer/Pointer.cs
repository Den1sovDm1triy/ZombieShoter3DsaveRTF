using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Pointer: MonoBehaviour
{
    public Action onSwitchOff;
    public virtual void DestroyPointer()
    {
        onSwitchOff?.Invoke();
      
    }

    public virtual void Init()
    {

    }
        

}
