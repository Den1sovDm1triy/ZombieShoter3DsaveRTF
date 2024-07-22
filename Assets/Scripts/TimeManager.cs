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
        // ��������� �������� �������� ������� ��� ����� ���������� �����
        originalIntensity1 = light1.intensity;
        originalIntensity2 = light2.intensity;
    }

    private void Update()
    {
        // �������� ������� ��� ���������� �������� �������� ����������
        ManageLightIntensity();
        ManageLightDirection();
    }

    private void ManageLightIntensity()
    {
        // �������� ������� ����� ���������� ����� �� ���� �� 1 ������
        float duration = 60f; // 1 ������ � ��������
        float t = Mathf.PingPong(Time.time, duration) / duration; // ����������� ����� �� 0 �� 1

        // ������������� ����� �������� ������� ���������� �����
        light1.intensity = Mathf.Lerp(0f, originalIntensity1, t);
        light2.intensity = Mathf.Lerp(0f, originalIntensity2, t);
    }
    private void ManageLightDirection()
    {
        // �������� ���� ����������� light1 ��� �������� ������� ����� �� ������
        float rotationAngle = Mathf.Sin(Time.time * rotationSpeed/10000) * 45f; // ������� ������������� ���������
        light1.transform.eulerAngles = new Vector3(rotationAngle, 45f, 0f); // ������������� ���� ����������� �����
    }
}


