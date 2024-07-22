using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfShoot : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "shootPoint", allowScaling:false);
    }
}
