using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnHider : MonoBehaviour
{
    [SerializeField] GameObject unHideObject;
    [SerializeField] float unHideTime;
    [SerializeField] private List<Sprite> infoSprites;
    [SerializeField] private Image infoImage;
    [SerializeField] private Button ContinueButton;
    [SerializeField] LoadingManager loadingManager;

    private void Start()
    {
        ContinueButton.onClick.AddListener(ShowInfo);
        Time.timeScale = 1;

    }

    private void ShowInfo()
    {
        ContinueButton.gameObject.SetActive(false);
        infoImage.sprite = infoSprites[Random.Range(0, infoSprites.Count)];
        infoImage.gameObject.SetActive(true);
        loadingManager.LoadScene("FarmScene");
        StartCoroutine(StartScene());
    }

    private IEnumerator StartScene()
    {
        yield return new WaitForSeconds(3f);
        loadingManager.LoadScene("FarmScene");
    }

    private void OnEnable()
    {
        StartCoroutine(UnHideCor());
    }

    private IEnumerator UnHideCor()
    {
        yield return new WaitForSeconds(unHideTime);
        ContinueButton.gameObject.SetActive(true);
    }
}
