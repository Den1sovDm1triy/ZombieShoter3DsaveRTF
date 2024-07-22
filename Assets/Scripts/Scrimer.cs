using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Scrimer : MonoBehaviour
{
    public UnityEvent onActive;
    [SerializeField] private float fulltimer, timer;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip attack;
    bool iscanStart = true;
    bool iscanattack = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            iscanattack = true;
            Debug.Log("PlayuerEnter");
            if(iscanStart)
            StartCoroutine(Timer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            iscanattack = false;
            StopAllCoroutines();
            timer = 0;
            iscanStart = true;
            anim.SetBool("isAttacking_2", false);
        }
    }

    private IEnumerator Timer()
    {
        iscanStart = false;       
        yield return new WaitForSeconds(fulltimer);        
        anim.SetBool("isAttacking_2", true);
        onActive?.Invoke();
        yield return new WaitForSeconds(1.2f);
        audio.PlayOneShot(attack);
        if (iscanattack) PlayerHealth.onSetDamage(40);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isAttacking_2", false);
    }
}
