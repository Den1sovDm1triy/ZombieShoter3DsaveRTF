using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEffect : MonoBehaviour
{
    [SerializeField] BossAnimationEvents bossAnimation;
    [SerializeField] private ParticleSystem die1Step, die2step;
    [SerializeField] private AudioSource stepAudio;
    [SerializeField] private AudioClip die1, die2;
    void Start()
    {
        bossAnimation.onActionDie1 += Die1;
        bossAnimation.onActionDie2 += Die2;
    }

    private void OnDestroy()
    {
        bossAnimation.onActionDie1 -= Die1;
        bossAnimation.onActionDie2 -= Die2;
    }

    private void Die1()
    {
        stepAudio.PlayOneShot(die1);
        die1Step.Play();
    }
    private void Die2()
    {
        stepAudio.PlayOneShot(die2);
        die2step.Play();
    }
}
