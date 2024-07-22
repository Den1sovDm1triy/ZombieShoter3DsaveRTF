using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
   



    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }



   
    



}
