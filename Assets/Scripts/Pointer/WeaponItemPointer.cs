using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemPointer :  Pointer
{
    public WeaponItem item;
    public override void Init()
    {
        PointerManager.Instance.AddToWeaponItemList(this);
        item.onTake += DestroyPointer;
    }

    private void OnDestroy()
    {
        item.onTake -= DestroyPointer;
    }

    public override void DestroyPointer()
    {
        base.DestroyPointer();
        PointerManager.Instance.RemoveFromWeaponItemList(this);
    }
}
