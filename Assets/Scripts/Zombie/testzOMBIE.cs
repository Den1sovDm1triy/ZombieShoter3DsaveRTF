using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testzOMBIE : MonoBehaviour
{
    [SerializeField]NavMeshAgent meshAgent;
    [SerializeField] Animator anim;
    [SerializeField] Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        Go();
    }


    private void Go()
    {
        
        StartCoroutine(Pursuit());
    }

    private IEnumerator Pursuit()
    {
        anim.SetBool("Walk", false);
        yield return new WaitForSeconds(Random.Range(0, 5));
        anim.SetBool("Walk", true);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, 5));
            meshAgent.SetDestination(player.transform.position);
        }
    }
 }
