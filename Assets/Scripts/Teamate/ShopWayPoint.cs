using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWayPoint : MonoBehaviour
{    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
