using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemPanel : MonoBehaviour
{
    [SerializeField] private GameObject problemPanel;
    private void Start()
    {
        InternetConnectionChecker.onConnection += Problem;
    }
    private void Problem(bool isproblem)
    {       
            problemPanel.SetActive(isproblem);       
    }
    private void OnDestroy()
    {
        InternetConnectionChecker.onConnection -= Problem;
    }
}
