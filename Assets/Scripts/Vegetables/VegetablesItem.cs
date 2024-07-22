using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class VegetablesItem : MonoBehaviour, IItem, IHealer
{
    public static Action<Vector3> onReadyToTake;
    public Action onTake;
    public static Action<VegetablesItem, GameObject, bool> onTakeVegetable; 
    [SerializeField] private Outline outline;
    public VegetablesType type;
    [SerializeField] private float energyPoints;
    public  Sprite imageSprite;
    private bool istaken;

    private bool isSound;

    [SerializeField] private Collider coll;
    public void DropItem()
    {
       
    }

    public void Heal(int hp)
    {
        PlayerHealth.onHeal?.Invoke(hp);
        Destroy(this.gameObject);
    }

    public void Indicate()
    {
        outline.enabled = true;
    }

    private void OnAnimationComplete()
    {
        onTake?.Invoke();
        Debug.Log("vegissound2" + isSound);
        onTakeVegetable?.Invoke(this, this.gameObject, isSound);
        this.gameObject.SetActive(false);
    }

    public void StopIndicate()
    {
        outline.enabled = false;
    }

    public void TakeItem(bool isSound)
    {
        Debug.Log("vegissound1" + isSound);
        this.isSound = isSound;
        coll.enabled = false;
        if (!istaken)
        {            
            onReadyToTake?.Invoke(transform.position);
            
                istaken = true;
            if (isSound)
            {
                transform.DOMove(PlayerInstance.Instance.transform.position, 0.1f)
                .OnComplete(OnAnimationComplete);
            }
            else OnAnimationComplete();
        }
        /* onTake?.Invoke();
         onTakeVegetable?.Invoke(this, this.gameObject);
         this.gameObject.SetActive(false);    */
    }
}

public enum VegetablesType
{
    Tomato, Pumpkin, Potato, Onion, Squash, Stawberry, Carrot, Non
}
