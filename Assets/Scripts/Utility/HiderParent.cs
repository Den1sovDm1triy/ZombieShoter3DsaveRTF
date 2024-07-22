using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiderParent : MonoBehaviour
{

    [SerializeField] private float timer;


    public void OnEnable()
    {
        Invoke(nameof(Hide), timer);
    }

    private void Hide()
    {
        transform.SetParent(null);
        this.gameObject.SetActive(false);
    }


}
