using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelButton : MonoBehaviour
{
    public static Action <LevelButton> onClick;
    [SerializeField] private Button button;
    public string description;
    
    public string info;
    [SerializeField] private TextMeshProUGUI infoText;
    public int unblockCristall;
    public string namescene;
    public bool isblock =>IsBlocked();

    [SerializeField] private Image blockImage;


    public void Click(){
        onClick?.Invoke(this);        
    }

    private void Start()
    {
        button.onClick.AddListener(Click);
        YandexReward.checkUnblock+=Check;
        StartCoroutine(Starter());
    }

    private IEnumerator Starter()
    {
        yield return null;
        if (IsBlocked())
        {
            blockImage.gameObject.SetActive(true);
        }
        else
        {
            blockImage.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        YandexReward.checkUnblock -= Check;
    }

    private void Check(){
        if(IsBlocked()){
            blockImage.gameObject.SetActive(true);
        }
        else{
            blockImage.gameObject.SetActive(false);
        }
    }


    public bool IsBlocked(){
        if(YandexGame.savesData.cristals>=unblockCristall){
            return false;
        }
        else return true;
    } 




}
