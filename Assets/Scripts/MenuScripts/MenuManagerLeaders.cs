using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManagerLeaders : MonoBehaviour
{
    [SerializeField] Button backButton;
    [SerializeField] Button restart, restartWithProgressButton;
    [SerializeField] GameObject backObject;

    private void Start()
    {
        backButton.onClick.AddListener(BackToMain);       
       
    }

  


   




    private void BackToMain()
    {
        //InterstitialAds.onShowInterstitial?.Invoke();
        SceneManager.LoadScene("FarmScene");
    }
}
