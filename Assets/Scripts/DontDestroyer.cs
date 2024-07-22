using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyer : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
