using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegEffectManager : MonoBehaviour
{
    public List<GameObject> takeVegEffect;
    void Start()
    {
        VegetablesItem.onReadyToTake += ShowEffect;
    }

    private void OnDestroy()
    {
        VegetablesItem.onReadyToTake -= ShowEffect;
    }

    private void ShowEffect(Vector3 pos)
    {
        foreach (var t in takeVegEffect)
        {
            if (!t.activeInHierarchy)
            {
                t.transform.position = pos;
                t.SetActive(true);
                return;
            }
        }
    }
}
