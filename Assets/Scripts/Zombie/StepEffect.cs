using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepEffect : MonoBehaviour
{
    [SerializeField] BossAnimationEvents bossAnimation;
    [SerializeField] private ParticleSystem leftStep, rightstep;
    [SerializeField] private AudioSource stepAudio;
    [SerializeField] private List<AudioClip> step;
    private int currentStepIndex = 0;
    private void Start()
    {
        bossAnimation.onStepLeft += LeftStep;
        bossAnimation.onStepRight += RightStep;
    }

    private void OnDestroy()
    {
        bossAnimation.onStepLeft -= LeftStep;
        bossAnimation.onStepRight -= RightStep;
    }
    private void LeftStep()
    {
        if(leftStep!=null)
        leftStep.Play();
        PlayFootstepSound();

    }
    private void RightStep()
    {
        if(rightstep!=null)
        rightstep.Play();
        PlayFootstepSound();
    }
    private void PlayFootstepSound()
    {
        
        stepAudio.PlayOneShot(step[currentStepIndex]);

        
        currentStepIndex++;

      
        if (currentStepIndex >= step.Count)
        {
            currentStepIndex = 0;
        }
    }
}