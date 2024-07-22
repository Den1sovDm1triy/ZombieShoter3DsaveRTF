using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    [SerializeField] private List<string> infoMessages = new List<string>();
    [SerializeField] private TextMeshProUGUI infoText;
    private int ind;


    private void Start(){
        StartCoroutine(Messager());
    }

    private IEnumerator Messager(){
        while(true)
        {
        ind = Random.Range(0, infoMessages.Count);
        infoText.text = infoMessages[ind];
        yield return new WaitForSeconds(5);
        }

    }
}
