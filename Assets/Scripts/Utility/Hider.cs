using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{ 
    [SerializeField] private float timer;
   
      
    public void OnEnable()
    {
        Invoke(nameof(Hide), timer);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
