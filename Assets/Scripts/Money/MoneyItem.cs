using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class MoneyItem : MonoBehaviour, IItem
{
    public static Action<Vector3, int> onTakeMoney;
    [SerializeField] private int amount;
    [SerializeField] private Outline outline;
    [SerializeField] private float flightDuration = 3.0f; // Продолжительность полета
    [SerializeField] private float flightHeight = 1.0f; // Высота траектории полета
    bool first;
    public void DropItem()
    {
        
    }

    public void Setup(int count)
    {
        amount = count;  
    }

    public void Indicate()
    {
        outline.enabled = true;
    }

    public void StopIndicate()
    {
        outline.enabled = false;
    }

    public void TakeItem(bool isSound)
    {
        if (!first)
        {
            first = true;
            onTakeMoney?.Invoke(transform.position, amount);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
          transform.DORotate(new Vector3(0, 360, 0), 1.0f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
          MoveToPlayer();
    }

    void MoveToPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 startPosition = transform.position;
            Vector3 midPoint = startPosition + (playerPosition - startPosition) / 2 + Vector3.up * flightHeight;

            Vector3[] path = new Vector3[] { midPoint, playerPosition };

            transform.DOPath(path, flightDuration, PathType.CatmullRom)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    onTakeMoney?.Invoke(transform.position, amount);
                    Destroy(this.gameObject);
                });
        }
        else
        {
            Debug.LogWarning("Player object not found.");
        }
    }
}
