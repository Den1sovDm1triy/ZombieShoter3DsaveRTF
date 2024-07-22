using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer :  Pointer
{
    [SerializeField] Enemy enemy;

    private void Awake()
    {
        PointerManager.Instance.AddToEnemyList(this);
        enemy.onDeath += DestroyPointer;
    }

    private void OnDestroy()
    {
        enemy.onDeath -= DestroyPointer;
    }

    public override void DestroyPointer()
    {
        base.DestroyPointer();
        PointerManager.Instance.RemoveFromEnemyList(this);
    }


}


