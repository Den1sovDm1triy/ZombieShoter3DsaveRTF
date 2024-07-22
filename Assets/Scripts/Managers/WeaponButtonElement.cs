using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonElement : MonoBehaviour
{   
    public Button weaponButton;
    public TextMeshProUGUI count;
    public TextMeshProUGUI textMesh;
    public string nameButton;
    [SerializeField] private Image weaponImage;


    public void Init(string weaponText, Sprite weaponSprite, bool isneedText, bool isneedCount)
    {
        nameButton = weaponText;
        textMesh.text = nameButton;
        weaponImage.sprite = weaponSprite;
        if (!isneedText)
        {
            textMesh.gameObject.SetActive(false);
        }
        if (!isneedCount)
        {
            count.gameObject.SetActive(false);
        }
    }  
   
 }
