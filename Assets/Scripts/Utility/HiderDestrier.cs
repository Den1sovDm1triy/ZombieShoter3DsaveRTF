using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiderDestrier : MonoBehaviour
{
    public void Start()
    {
        Invoke(nameof(DEstroyObject), 15f);
    }

    private void DEstroyObject()
    {
        Destroy(this.gameObject);
    }
}

