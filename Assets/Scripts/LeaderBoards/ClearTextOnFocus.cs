using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class ClearTextOnFocus : MonoBehaviour
{
    [SerializeField]private TMP_InputField inputField;
    
    

    public void ClearInput()
    {        
        inputField.text = string.Empty;
    }
}







