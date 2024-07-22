using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using JetBrains.Annotations;
using UnityEngine;
using YG;

public class AdsTimer : MonoBehaviour
{
    public static AdsTimer Instance;

    public bool isCanShowAds = false;
    public float timeShowAds = 90f;
    
    private void Awake(){
         if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        YandexGame.CloseFullAdEvent += CloseFullAd;
        StartCoroutine(Timer());
    }

    private void OnDestroy(){
        YandexGame.CloseFullAdEvent -= CloseFullAd;
    }

    private void CloseFullAd(){
        isCanShowAds = false;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer(){
        yield return new WaitForSeconds(timeShowAds);
        isCanShowAds=true;
    }

    


}
