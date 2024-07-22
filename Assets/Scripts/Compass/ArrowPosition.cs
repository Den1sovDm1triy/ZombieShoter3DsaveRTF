using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPosition : MonoBehaviour
{
    public float arrowTargetSmooth;
    public Transform[] waypoints;
    public float changedistance;
    public GameObject arrow;
    int i;
    private void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Vector3 vectorToTarget = transform.InverseTransformPoint(waypoints[i].position);
        float distanceToTarget = vectorToTarget.magnitude;
        if (distanceToTarget<changedistance)
            Changeway();
    }

    public void Look()
    {
       transform.LookAt(waypoints[i]);
       //transform.localRotation = Quaternion.Lerp(transform.localRotation, waypoints[i].localRotation, arrowTargetSmooth * Time.deltaTime);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
            Changeway();
    }*/
    [ContextMenu("Changeway")]
    public void Changeway()
    {
        if(i<waypoints.Length-1)
        i++;
        if(i==5)
        {
            arrow.SetActive(false);
        }
        if (i == 7)
        {
            arrow.SetActive(true);
        }
        if (i == 10)
        {
            arrow.SetActive(false);
        }

    }

    
}
