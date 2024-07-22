using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InternetConnectionChecker : MonoBehaviour
{
    public static Action<bool> onConnection;

    private void Start()
    {
        StartCoroutine(CheckNetworkStatus());
    }



    private IEnumerator CheckNetworkStatus()
    {
        while (true)
        {
            yield return new  WaitForSeconds(0.5f);
            NetworkReachability status = Application.internetReachability;

            if (status == NetworkReachability.NotReachable)
            {
                onConnection?.Invoke(true);
            }
            else
            {
                onConnection?.Invoke(false);
            }
        }
    }
}
