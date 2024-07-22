using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;
    [SerializeField] private Transform slider;

    private void Update()
    {
        // Поворот вывески в сторону игрока (горизонтально)
        slider.LookAt(slider.transform.position + player.forward, player.up);

        // Зафиксировать вертикальное вращение (поворот по оси x)
        Vector3 eulerAngles = slider.transform.rotation.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.z = 0f;
        slider.rotation = Quaternion.Euler(eulerAngles);
    }




    private void Start()
    {
        player = Camera.main.transform;
    }
}
