using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    public static Action onSave;
    public static Action onLoad;
    public static Action onDeleteSave;
    public static Action onIsLoading;
    public static Action onIsFinishLoading;


   
    private void Start()
    {
        Debug.Log(SaveData.Has("progress")+"прогресс");
        if (SaveData.Has("progress"))
        {
            onIsLoading?.Invoke();
            StartCoroutine(Loading());
        }
    }
    private IEnumerator Loading()
    {
        yield return null;
        Load();
        yield return new WaitForSeconds(1f);
        onIsFinishLoading?.Invoke();
    }



    [ContextMenu("Save")]
    public void SaveContext()
    {
        SaveData.SetBool("progress", true);
        onSave?.Invoke();
        Time.timeScale = 0;
    }
   
    public static void Save()
    {
        SaveData.SetBool("progress", true);
        onSave?.Invoke();
        Time.timeScale = 0;
    }

    [ContextMenu("Load")]
    public void Load()
    {
        Debug.Log("Сколько раз");
        onLoad?.Invoke();
    }
    [ContextMenu("Delete")]    
    public void DeleteSave()
    {
        SaveData.Delete("progress");
        onDeleteSave?.Invoke();
    }


    [ContextMenu("ClearAll")]
    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
    }
}




