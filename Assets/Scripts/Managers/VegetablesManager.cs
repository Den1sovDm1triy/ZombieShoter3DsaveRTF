using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetablesManager : MonoBehaviour
{
    [SerializeField] private GameObject vegprefab;
    GameObject curVegetables;
    GameObject lasVegetables;

    private void Start()
    {
        SaveManager.onSave += Regenerate;

        if (SaveData.Has("progress"))
        {
            Regenerate();
        }
    }
    private void OnDestroy()
    {
        SaveManager.onSave -= Regenerate;
    }

    private void Regenerate()
    {
        if (lasVegetables != null) Destroy(lasVegetables);
        curVegetables = Instantiate(vegprefab);
        lasVegetables = curVegetables;
    }

}
