using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionrayCaster : MonoBehaviour
{
    public static Action onNoInteractible;
    [SerializeField] private Transform pointOfTake;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private float raycastInterval = 0.3f;

    private bool isRaycasting = false;
    InteractibleObjects interactible;
    InteractibleObjects lastInteractible;
    private Ray ray;

    bool isActiveCaster=true;

    private void Start()
    {
        InteractibleObjects.onTake += Taken;
        ObjectManager.onReadyObjectToDrop += StopInteractRay;
    }

    private void OnDestroy()
    {
        InteractibleObjects.onTake -= Taken;
        ObjectManager.onReadyObjectToDrop -= StopInteractRay;
    }

    private void Taken(bool isTaken)
    {
        isActiveCaster = !isTaken;
    }

    private void StopInteractRay(bool isStop)
    {
        isActiveCaster = !isStop;
    }

    private void Update()
    {
        if (isActiveCaster)
        {
            ray.origin = Camera.main.transform.position;
            ray.direction = Camera.main.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance, interactLayer))
            {
                Debug.Log(hit.collider.name);
                interactible = hit.collider.GetComponent<InteractibleObjects>();
                if (lastInteractible != null)
                {
                    if (interactible != lastInteractible) lastInteractible.OnRaycastHit(false);
                }
                lastInteractible = interactible;
                if (interactible != null)
                {
                    interactible.OnRaycastHit(true);
                }
            }
            else
            {
                ResetRaycastHit();
            }
        }
    }




    private void ResetRaycastHit()
    {
        if (lastInteractible != null)
        {            
            lastInteractible.OnRaycastHit(false);
            lastInteractible = null;
        }
        if (interactible != null)
        {

            interactible.OnRaycastHit(false);
            interactible = null;
        }
        onNoInteractible?.Invoke();

    }

}
