using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperPointer : MonoBehaviour
{
    [SerializeField] private Pointer pointer;
    [SerializeField] private Pointer nextitemPointer;       
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nextitemPointer.gameObject.SetActive(true);
            nextitemPointer.Init();            
            Compass.onSetCompasTarget(nextitemPointer.gameObject.transform);
            pointer.DestroyPointer();
            Destroy(gameObject);

        }
    }

    public void Hide()
    {
        pointer.DestroyPointer();
        Destroy(gameObject);
    }
}
