using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;


public class LevelManager : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private string currentSceneName;
    [SerializeField] private bool iscanLoadScene;

    [SerializeField] private Button startGameButton;
    [SerializeField] private TextMeshProUGUI startButtonText;

    [SerializeField] private LevelButton currentButton;
    [SerializeField] private List<LevelButton> levelButtons;

    [SerializeField] private List<GameObject> hideObjectsOnLoad;
    

    private AsyncOperation asyncLoad;   
    [SerializeField] private Slider loadingSlider;
   
    private void Start()
    {
        LevelButton.onClick += ClickButton;
        startGameButton.onClick.AddListener(StartGame);
        currentButton = levelButtons[0];
        currentButton.Click();
        YandexReward.checkUnblock+=CheckUnblock;
    }
    private void OnDestroy()
    {
        LevelButton.onClick -= ClickButton;
        YandexReward.checkUnblock-=CheckUnblock;
    }

    private void CheckUnblock()
    {       
        currentButton.Click();
    }
    
    private void ClickButton(LevelButton levelButton)
    {
        currentButton=levelButton;
        Debug.Log(currentButton.IsBlocked()+"blocked");
        if(currentButton.IsBlocked())
        {
            iscanLoadScene = false;
            int diff = currentButton.unblockCristall - YandexGame.savesData.cristals;
            description.text=$"Уровень  {currentButton.info} заблокирован, не хватает {diff} кристаллов для открытия";   
        }          
        else
        {
            description.text="Начать уровень " +  currentButton.info;
            iscanLoadScene = true;            
        }
           

    }
    private void StartGame(){
        if(iscanLoadScene){
            LoadSceneAsync(currentButton.namescene);            
        }
        else {
            int diff = currentButton.unblockCristall - YandexGame.savesData.cristals;
            description.text = $"Не хватает кристаллов {diff} для открытия уровня";
        }
    }

    private void HiderObjects(){
        foreach (var item in hideObjectsOnLoad){
            item.SetActive(false);
        }
    }


    public void LoadSceneAsync(string sceneName)
    {       
        HiderObjects();
        loadingSlider.gameObject.SetActive(true);
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }
      

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            description.text =  "Загрузка "+(Math.Round(progress * 100)).ToString() + "%";
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
