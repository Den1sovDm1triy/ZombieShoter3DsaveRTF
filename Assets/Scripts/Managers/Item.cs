using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem 
{
    public void TakeItem(bool isSound);
    public void DropItem();
    public void Indicate();
    public void StopIndicate();
}
