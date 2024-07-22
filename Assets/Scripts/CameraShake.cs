using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    public float duration;
    public float stretch;
    public int vibrato;
    public float randomlessl;
    private bool isShaking = false;
    private Vector3 initialPosition;
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        PlayerHealth.onTakeDamage += Shake;        
    }

    private void OnDestroy()
    {
        PlayerHealth.onTakeDamage -= Shake;
    }
   
    [ContextMenu("Shake")]
    private void Shake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        Vector3 initialPosition = cameraTransform.localPosition;
        cameraTransform.DOShakePosition(duration, stretch, vibrato, randomlessl, false, true)
            .OnComplete(() =>
            {
                // После завершения тряски, возвращаем камеру в исходное положение
                cameraTransform.DOLocalMove(initialPosition, 0.3f).OnComplete(() =>
                {
                    isShaking = false;
                });
            });

        // Ждем завершения тряски
        yield return new WaitForSeconds(duration);
    }
}

