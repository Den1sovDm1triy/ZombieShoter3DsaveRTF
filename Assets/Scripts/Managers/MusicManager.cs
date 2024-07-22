using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Button musicButton;
    [SerializeField] private Image musicIcon;
    [SerializeField] private Sprite onMusic, offmusic;
    [SerializeField] private bool isMusicPlay;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    private bool nowwave = false;
    private int waveNumber;
    private int currentTrackIndex=-1;


    private void OnEnable() {
         YandexGame.GetDataEvent+= SDKEnabled;
    }
      private void OnDisable()
    {
         YandexGame.GetDataEvent-= SDKEnabled;
    }


    private void SDKEnabled()
    {
        isMusicPlay=YandexGame.savesData.isMusicPlay;
        if(isMusicPlay==false){
            musicIcon.sprite=offmusic;
        }
        else{
            musicIcon.sprite=onMusic;
        }
    }

    private void Awake(){
        ZombieManager.onStartWave+=StartWave; 
        ZombieManager.onWaveIsDestroyd+=StopTrack;
        currentTrackIndex =0;  
        isMusicPlay=YandexGame.savesData.isMusicPlay;
        if(isMusicPlay==false){
            musicIcon.sprite=offmusic;
        }
        else{
            musicIcon.sprite=onMusic;
        }
        musicButton.onClick.AddListener(ChangeMusicSetings);   
         
    }

    private void ChangeMusicSetings(){
        if(isMusicPlay){
            isMusicPlay=false;
            musicIcon.sprite=offmusic;
            audioSource.Pause();            
        }
        else{             
            isMusicPlay=true;
            musicIcon.sprite=onMusic;
            audioSource.Play();
            if (nowwave)
            {
                audioSource.clip = audioClips[currentTrackIndex];
                audioSource.Play();
                audioSource.loop = true;
                audioSource.volume = 0;
                StopAllCoroutines();
                StartCoroutine(IncreaseVolumeOverTime(1, 5f));
            }
        }  
        YandexGame.savesData.isMusicPlay=isMusicPlay;      
    }

    private void OnDestroy(){
        ZombieManager.onStartWave-=StartWave;
        ZombieManager.onWaveIsDestroyd-=StopTrack;
    }

    private void StopTrack(){
        nowwave=false;
        StopAllCoroutines();
        currentTrackIndex = (currentTrackIndex + 1) % audioClips.Length;
        StartCoroutine(IncreaseVolumeOverTime(0, 1f));
    }

    private void StartWave(int waveNumber){
        nowwave=true;
        this.waveNumber=waveNumber;
        if(!isMusicPlay) return;
        if(waveNumber>=0){
            audioSource.clip = audioClips[currentTrackIndex];
            audioSource.Play();
            audioSource.loop=true;
            audioSource.volume=0;
            StopAllCoroutines();
            StartCoroutine(IncreaseVolumeOverTime(1, 5f));
        }
    }
   
    private IEnumerator IncreaseVolumeOverTime(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;
        
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }
        
        audioSource.volume = targetVolume; // Ensure the volume reaches the target exactly
    }


}
    



