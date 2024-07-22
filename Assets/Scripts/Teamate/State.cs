using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public Character character;

    public virtual void Init()
    {

    }
    public virtual void DeInit()
    {

    }
    public abstract void Run();

}
