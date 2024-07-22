using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RandomInfo : MonoBehaviour
{
    [SerializeField] private List<Sprite> infoSprites;
    [SerializeField] private Image infoImage;
    private int i;
    private void OnEnable()
    {
        i++;
        if (i == 1)
        {
            infoImage.sprite = infoSprites[1];
            return;
        }

        infoImage.sprite = infoSprites[Random.Range(0, infoSprites.Count)];
    }
}
