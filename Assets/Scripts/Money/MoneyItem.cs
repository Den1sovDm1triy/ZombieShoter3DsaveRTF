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
    private float collectionRadius;

    public void DropItem() { }

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
        StartCoroutine(CheckDistanceToPlayer());
    }

    IEnumerator CheckDistanceToPlayer()
    {
        while (true)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                collectionRadius = PlayerUpgradeManager.Instance.collectionRadius;

                Debug.Log("Distance to player: " + distance + ", Collection radius: " + collectionRadius);

                if (distance <= collectionRadius)
                {
                    MoveToPlayer();
                    while (distance <= collectionRadius)
                    {
                        distance = Vector3.Distance(transform.position, player.transform.position);
                        if (distance <= collectionRadius)
                        {
                            // Обновляем путь до текущей позиции игрока
                            transform.DOMove(player.transform.position, 0.5f).SetEase(Ease.InOutSine);
                        }
                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
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
