using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieModel : MonoBehaviour
{
    [SerializeField] private List<GameObject> zombieModels;
    public Outline outline;
    void Awake()
    {
        int m = Random.Range(0, zombieModels.Count);
        GameObject enemymodelobject = zombieModels[m];
        enemymodelobject.SetActive(true);
        outline = enemymodelobject.GetComponent<Outline>();
    }
   
}
