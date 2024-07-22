using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;
public abstract class InteractibleObjects : MonoBehaviour
{
    public UnityEvent OnTake, OnDrop, OnActivate, OnDeactivate, onInHand;
    public static  Action<bool> onTake;
    public bool isTakable;
    public bool isUsable;
    public static Action<InteractibleObjects> onInteract;
    public static Action<InteractibleObjects, bool, bool, bool> onReadyToInteract;

    
    public bool isActive;
    public bool isTaken;
    public bool isInHand;
    [SerializeField] Outline outline;
    [SerializeField] Rigidbody rigid;
    [SerializeField] Collider col;

    public Vector3 takeRotation;
    public bool isWeaponPoint;
    public bool isUseInHand;
    public TypeInteractible typeInteractible;
    public Sprite objectsSprite;

    


    private void Start()
    {
        col = GetComponent<Collider>();       
    }


    public void Drop()
    {
        if(rigid!=null)
        rigid.isKinematic = true;
        col.isTrigger = false;
        OnDrop?.Invoke();
        isTaken = false;
        onTake?.Invoke(false);

        if (isWeaponPoint) rigid.isKinematic = false;
    }
    public void Taken()
    {
        if (rigid != null)
            rigid.isKinematic = true;
        OnTake?.Invoke();
        onTake?.Invoke(true);
        isTaken = true;
        if(isWeaponPoint)
        transform.DOMove(PointOfTake.Instance.weaponPoint.position, 0.3f, false);
        else
        {
            transform.DOMove(PointOfTake.Instance.point1.position, 0.3f, false);
        }
        transform.DOLocalRotate(takeRotation, 0.3f);
    }

    public virtual void InHand()
    {
        onInHand?.Invoke();
    }
    

    public void OnRaycastHit(bool isHit)
    {
        if (isHit)
        {
            if (outline != null)
                outline.enabled = true;
            onReadyToInteract?.Invoke(this, true, isUsable, isTakable);
        }
        else
        {
            if(outline!=null)
                outline.enabled = false;
            onReadyToInteract?.Invoke(this, false, isUsable, isTakable);
        }
    }

    public void Interact()
    {
        if (!isActive)
        {            
            Activate();            
        }
        else
        {            
            Deactivate();            
        }
    }
    
    public virtual void Activate()
    {
        isActive = true;
        OnActivate?.Invoke();
    }
    public virtual void Deactivate()
    {
        isActive = false;
        OnDeactivate?.Invoke();
    }
}

public enum TypeInteractible
{
    woodBarrel, metalBarrel, woodbox, hay, radio,  chainsaw, woodfence, flamethrower, chair, tyre, fence, chairRocking, none,
}
