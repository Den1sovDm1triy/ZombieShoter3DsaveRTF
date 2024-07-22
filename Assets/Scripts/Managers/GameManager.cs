using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YG;

public class GameManager : MonoBehaviour
{
    public static Action onGameOver;
    public GameObject joystick;
    
    

    private void Awake()
    {
        AudioListener.volume = 0;
        StartCoroutine(onCor());
    }
    void Start()
    {     
        if (!YandexGame.EnvironmentData.isMobile)
        {                
            joystick.SetActive(false);            
        }

       
        PlayerHealth.onDeath += GameOver;
    }
    [ContextMenu("Pause")]
    private void OnListener()
    {
        AudioListener.volume = 0;
        AudioListener.pause = false;
    }

    private IEnumerator onCor()
    {
        yield return new WaitForSeconds(1f);
        while (AudioListener.volume <= 1)
        {           
            AudioListener.volume += 0.01f;
            yield return null;
        }
    }


    private void OnDestroy()
    {
        PlayerHealth.onDeath -= GameOver;
    }

   private void GameOver()
   {
        onGameOver?.Invoke();
   }
}
