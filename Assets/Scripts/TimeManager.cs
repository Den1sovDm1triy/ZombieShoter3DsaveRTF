using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public Light light1;
    public Light light2;
    private float originalIntensity1;
    private float originalIntensity2;
    private float rotationSpeed = 20f;

    private void Start()
    {
        // —охран€ем исходные значени€ €ркости дл€ обоих источников света
        originalIntensity1 = light1.intensity;
        originalIntensity2 = light2.intensity;
    }

    private void Update()
    {
        // ¬ызываем функцию дл€ управлени€ €ркостью световых источников
        ManageLightIntensity();
        ManageLightDirection();
    }

    private void ManageLightIntensity()
    {
        // ќпускаем €ркость обоих источников света до нул€ за 1 минуту
        float duration = 60f; // 1 минута в секундах
        float t = Mathf.PingPong(Time.time, duration) / duration; // Ќормализуем врем€ от 0 до 1

        // ”станавливаем новые значени€ €ркости источников света
        light1.intensity = Mathf.Lerp(0f, originalIntensity1, t);
        light2.intensity = Mathf.Lerp(0f, originalIntensity2, t);
    }
    private void ManageLightDirection()
    {
        // »змен€ем угол направлени€ light1 дл€ создани€ эффекта теней от солнца
        float rotationAngle = Mathf.Sin(Time.time * rotationSpeed/10000) * 45f; // ѕлавные волнообразные изменени€
        light1.transform.eulerAngles = new Vector3(rotationAngle, 45f, 0f); // ”станавливаем угол направлени€ света
    }
}


