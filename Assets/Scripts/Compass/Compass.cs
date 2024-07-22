using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Compass : MonoBehaviour
{
    public static Action<Transform> onSetCompasTarget;
    public static Action onOffCompass;
    public Transform target;
    public float rotationSpeed = 5f;
    [SerializeField] private GameObject distanceObject;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField]bool isActivate;
    [SerializeField] private GameObject arrow;

    private void Awake()
    {
        Deinit();
        onSetCompasTarget += Init;
        onOffCompass += Deinit;
    }


    private void OnDestroy()
    {
        onSetCompasTarget -= Init;
        onOffCompass -= Deinit;
    }


    private void Init(Transform target)
    {
        this.target = target;
        isActivate = true;       
        arrow.SetActive(true);
        distanceObject.SetActive(true);
    }

    public void Deinit()
    {
        this.target = null;
        isActivate = false;
        arrow.SetActive(false);
        distanceObject.SetActive(false);
    }

    void Update()
    {
        if (target == null)
        {           
            return;
        }


        Vector3 targetDirection = target.position - transform.position;
        targetDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        var distance = Vector3.Distance(transform.position, target.position);
        distanceText.text = Mathf.Round(distance).ToString() + "m";
    }
}
