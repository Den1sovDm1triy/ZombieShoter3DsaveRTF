using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GAnalitics : MonoBehaviour
{
    private static GAnalitics instance;

    public static GAnalitics Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameAnalytics.Initialize();
    }

    public  void SendLevelStartedEvent(string levelName)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelName);
    }
}
