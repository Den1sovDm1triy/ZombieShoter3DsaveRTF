using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCrawl : MonoBehaviour
{
    [SerializeField]Enemy enemy;
    [SerializeField] Collider capsuleV, capsuleH;

    private void Start()
    {
        enemy.onAttack += ColliderVertical;
        enemy.onMoveToPlayer += ColliderHorizontal;
        enemy.onDeath -= DeleteVertical;
    }

    private void OnDestroy()
    {
        enemy.onAttack -= ColliderVertical;
        enemy.onMoveToPlayer -= ColliderHorizontal;
        enemy.onDeath -= DeleteVertical;
    }

    private void DeleteVertical()
    {
        capsuleV.enabled = false;

    }

    private void ColliderVertical()
    {
        capsuleV.enabled = true;
        capsuleH.enabled = false;
    }
    private void ColliderHorizontal()
    {
        capsuleV.enabled = false;
        capsuleH.enabled = true;
    }




}
