using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DogBeh : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Transform player;
    public Animator anim;
    public AudioSource audio;
    public AudioClip bark, whining;
    private void Start()
    {      
        StartCoroutine(Mover());
    }



    IEnumerator Mover()
    {
        while (true)
        {
            bool isPause = false;
            if (Vector3.Distance(transform.position, player.position) > 5f)
            {
                audio.PlayOneShot(bark);
                meshAgent.SetDestination(player.transform.position);
                anim.SetBool("isWalking", true);                
                anim.SetBool("isSitting", false);
                meshAgent.speed = 2.5f;
                meshAgent.stoppingDistance =Random.Range(2, 3);
            }
            else
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isSitting", true);
                meshAgent.speed = 0;
                if (!audio.isPlaying&&isPause==false)
                {
                    audio.PlayOneShot(whining);
                    isPause = true;
                }
            }
            yield return new WaitForSeconds(Random.Range(3f, 15f));
            isPause = false;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= meshAgent.stoppingDistance)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isSitting",  true);
            meshAgent.speed = 0;
        }
    }
}
