using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioClip damageClip, deathClip, idleClip, angryClip, attackClip, hitClip;
    [SerializeField] private AudioSource audioSource;
    public void PlayDamage()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(damageClip);
    }
    public void PlayDeath()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(damageClip);
        Invoke(nameof(Mute), 1f);
    }

    private void Mute()
    {
        audioSource.enabled = false;
    }
    public void PlayIdle()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(damageClip);
    }
    public void PlayAttack()
    {
        audioSource.loop = false;
        //audioSource.PlayOneShot(attackClip);
    }
    public void PlayHit()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(hitClip);
    }
    public void PlayAngry()
    {
        //audioSource.loop = false;
        //audioSource.PlayOneShot(angryClip);
    }
}
