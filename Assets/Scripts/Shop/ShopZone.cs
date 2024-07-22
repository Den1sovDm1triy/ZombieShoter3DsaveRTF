using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class ShopZone : MonoBehaviour
{
    public UnityEvent OnFirstEnter;

    public static Action<bool> onShopReady;
    public static Action<bool> onShopOpen;
    public Collider shopZone;
    public bool isClientInside;
    public GameObject openMessage;
    public GameObject closeMessage;    
    private Transform player;
    [SerializeField] private Transform message;
    public static Action<int> onFirstEnter;
    int countenter = 0;



    private void Update()
    {
        // Поворот вывески в сторону игрока (горизонтально)
        message.LookAt(message.transform.position + player.forward, player.up);

        // Зафиксировать вертикальное вращение (поворот по оси x)
        Vector3 eulerAngles = message.transform.rotation.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.z = 0f;
        message.rotation = Quaternion.Euler(eulerAngles);
    }




    private void Start()
    {
       player = Camera.main.transform;
        isClientInside = false;         
      
    }


   

    private void ShopUnavalable()
    {     
        shopZone.enabled = false;
        isClientInside = false;
        onShopOpen?.Invoke(false);
        onShopReady?.Invoke(false);
        closeMessage.SetActive(true);
        openMessage.SetActive(false);       
    }

    private void ShopAvailable(int count)
    {       
            shopZone.enabled = true;
            onShopOpen?.Invoke(true);
            openMessage.SetActive(true);
            closeMessage.SetActive(false);          
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onShopReady?.Invoke(true);
            isClientInside = true;           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isClientInside)
            {
                onShopReady?.Invoke(true);
                isClientInside = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onShopReady?.Invoke(false);
            isClientInside = false;
        }
    }
}
