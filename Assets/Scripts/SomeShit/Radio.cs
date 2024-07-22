using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : InteractibleObjects
{
    private AudioSource audioSource;
    public AudioClip[] tracks;
    private int currentTrackIndex = 0;
    private bool isLooping = false;
    private float lastPlaybackTime = 0f;

    bool firstOn=true;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Реализация методов Activate() и Deactivate() обязательна
    public override void Activate()
    {
        base.Activate();       
        StartPlaying();      
    }

    public override void Deactivate()
    {
        base.Deactivate();
        StopPlaying();
    }

    private void StartPlaying()
    {
       
        if (tracks.Length > 0)
        {           
            audioSource.enabled = true;
            if (firstOn)
            {
                currentTrackIndex = Random.Range(0, tracks.Length);
                firstOn = false;
            }            
            audioSource.clip = tracks[currentTrackIndex];           
            audioSource.time = lastPlaybackTime;            
            audioSource.Play();            
            audioSource.loop = false;
            audioSource.loop = isLooping;
            audioSource.loop = isLooping; 
        }
        else
        {
            Debug.LogWarning("No tracks assigned to the radio.");
        }
    }

    private void StopPlaying()
    {
        // Сохраняем текущую позицию воспроизведения перед остановкой
        lastPlaybackTime = audioSource.time;

        // Останавливаем воспроизведение
        audioSource.Stop();

        // Выключаем AudioSource
        audioSource.enabled = false;
    }

    private void Update()
    {
        // Проверяем, завершилось ли воспроизведение текущего трека
        if (!audioSource.isPlaying&&isActive)
        {
            // Проигрываем следующий трек
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
          
        currentTrackIndex = (currentTrackIndex + 1) % tracks.Length;       

        // Устанавливаем новый трек
        audioSource.clip = tracks[currentTrackIndex];

        // Проигрываем новый трек
        audioSource.Play();
    }

    public void ToggleTrackLooping()
    {       
        isLooping = !isLooping;
        audioSource.loop = isLooping;
    }

   
    public void SetPlaybackTime(float time)
    {        
        audioSource.time = Mathf.Clamp(time, 0f, audioSource.clip.length);
    }
}