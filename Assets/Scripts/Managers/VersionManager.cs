using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    void Start() {     
        string currentVersion = Application.version;
        versionText.text = currentVersion;     

      
    }
}
