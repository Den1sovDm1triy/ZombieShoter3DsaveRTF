using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using YG;
using System.Runtime.InteropServices;

public class UIManager : MonoBehaviour
{
    public static Action<InteractibleObjects> onTakeInteractible;
    public static Action onDrop;
    public static Action <bool> onSpeedTime;
    public bool isSpeedTime;

  
    public static Action<Teammate> onShowTeammateMenu;
    public static Action onChangeWeapon;
    public static Action<VegetablesType> onChooseItem;
    public static Action<WeaponName> onChooseWeapon;
    public static Action onShowShop;
    public static Action onOpenDoor;


    [SerializeField] private GameObject blockDoor;
    [SerializeField] private TextMeshProUGUI blockDoorKeyInfo;
    [SerializeField] private GameObject endGameObject;
    [SerializeField] private Button restartButton, leadersButton, restartFromLastSave;
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] private Button takeButton;
    [SerializeField] private ButtonElement elementButton;
    [SerializeField] private Transform weaponButtonHook;
    [SerializeField] private Transform itemButtonHook;
    [SerializeField] ItemManager itemManager;
    private List<ButtonElement> buttons = new List<ButtonElement>();
    IItem curItem;

    [SerializeField] private Sprite axeSprite, revolverSprite, shotguSprite, uziSprite, akmSprite, pistolSprite, chainSawSprite, nothingSprite;

    [SerializeField] private Button changeButon;
    [SerializeField] private Image changeButtonImage;
    [SerializeField] private GameObject saveMark;
    [SerializeField] private TextMeshProUGUI savemarkNumberDay, savemarkCountdead;
    [SerializeField] private Image dayMarkImage;
    [SerializeField] private GameObject redDayMark, normalDayMark;
    [SerializeField] private GameObject headShotMark;
    [SerializeField] private GameObject shopButton;
   
    [SerializeField] private Button doorButton;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI cristalText;
    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private GameObject goToShopObject;    
    [SerializeField] private Button manageGardenButton;
    [SerializeField] private Teammate currentteammate;
    [SerializeField] private Button interactTeamateButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Image speedButtonImage;

    [SerializeField] private GameObject moneyAmount;


    [SerializeField] InteractibleObjects currentInteractObject;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button takeInteractButton;
    [SerializeField] private TextMeshProUGUI takeInteractText;    


    [SerializeField] private GameObject waveinfoObject;
    [SerializeField] private TextMeshProUGUI waveinfoText;
    [SerializeField] private GameObject countEnemyObject;
    [SerializeField] private TextMeshProUGUI countEnemyText;

    [SerializeField] private GameObject timerObject;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject pauseMenu;
    private Coroutine MarkerCor;
    bool firstTime=true;
    private int numberScene; 
    private bool ismobile=false;
    private bool isLock=false;
    private bool isShopOpen=false, isUpgradeOpen=false;
    [SerializeField] private Button closePauselButton;
    [SerializeField] private Button continuePauseButtton;
    [SerializeField] private Button openPauseButton;

    
    [SerializeField] private Button showRewardButton;
    [SerializeField] private Button mainmenuButton;

    [SerializeField] private List<GameObject> hideOnPause = new List<GameObject>();
    [SerializeField] private GameObject info;

    private void Start()
    {
        curItem = null;
        restartButton.onClick.AddListener(ShowAdsRestart);
        leadersButton.onClick.AddListener(Leaders);
        takeButton.onClick.AddListener(Take);
        InventoryManager.onTakeWeapon += TakeWeapon;
        InventoryManager.onReadyToTake += ReadyToTake;
        InventoryManager.notReadyToTake += NotReadyToTake;
        InventoryManager.onTakeItem += TakeItem;
        takeButton.gameObject.SetActive(false);
        GameManager.onGameOver += ShowGameOverWindow;
        Vegetable.onChangeCount += ChangeVegatableCount;
        ItemManager.onEndEmptyItem += EmptyItem;
        WeaponManager.onChange += ChangeSprite;       
        changeButon.onClick.AddListener(ChangeWeapon);    
        WeaponAttacker.onHeadShot += HeadShot;
        ShopZone.onShopReady += ShopReady;
        shopButton.GetComponent<Button>().onClick.AddListener(OpenShop);
        MoneyManager.onMoneyChange += MoneyChange;   
        MoneyManager.onCristalChange+=CristalChange;   
            
        InteractibleObjects.onReadyToInteract += ReadyToInteract;
        InteractionrayCaster.onNoInteractible += HideInteractButtons;     
        ZombieManager.onStartWave+=StartWave;
        ZombieManager.onEnemyLeft+=EnemyLeft;
        ZombieManager.onWaveIsDestroyd+=WaveIsDestroyd;
        ZombieManager.onTimerBeforeStart+=TimerBeforeStart;
        ZombieManager.onBossStart +=BossStart;
        YandexGame.CloseFullAdEvent+=CloseFullAd;
        ShopManager.onShowBuyMenu+=ShowBuyMenu;
        ShopManager.onCloseShop+=CloseShop;
        PlayerUpgradeShop.onShowUpgradeShop+=ShowUpgradeShop;
        PlayerUpgradeShop.onCloseUpgradeShop+=CloseUpgradeShop;
        openPauseButton.onClick.AddListener(OpenPause);
        closePauselButton.onClick.AddListener(ClosePause);
        continuePauseButtton.onClick.AddListener(ClosePause);
        ismobile = YandexGame.EnvironmentData.isMobile;
        showRewardButton.onClick.AddListener(ShowReward);
        mainmenuButton.onClick.AddListener(Leaders);
        YandexGame.RewardVideoEvent += Rewarded;
        if (!ismobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            info.SetActive(true);
        }
        else info.SetActive(false);
    }
    private void OnDestroy()
    {
        InventoryManager.onTakeWeapon -= TakeWeapon;
        InventoryManager.onReadyToTake -= ReadyToTake;
        InventoryManager.notReadyToTake -= NotReadyToTake;
        InventoryManager.onTakeItem -= TakeItem;
        GameManager.onGameOver -= ShowGameOverWindow;
        Vegetable.onChangeCount -= ChangeVegatableCount;
        ItemManager.onEndEmptyItem -= EmptyItem;
        WeaponManager.onChange -= ChangeSprite;

        WeaponAttacker.onHeadShot -= HeadShot;
        ShopZone.onShopReady -= ShopReady;
        MoneyManager.onMoneyChange -= MoneyChange;
        MoneyManager.onCristalChange-=CristalChange;   




        manageGardenButton.onClick.RemoveAllListeners();
        InteractibleObjects.onReadyToInteract -= ReadyToInteract;
        InteractionrayCaster.onNoInteractible -= HideInteractButtons;
        ZombieManager.onStartWave -= StartWave;
        ZombieManager.onEnemyLeft -= EnemyLeft;
        ZombieManager.onWaveIsDestroyd -= WaveIsDestroyd;
        ZombieManager.onTimerBeforeStart -= TimerBeforeStart;
        ZombieManager.onBossStart -= BossStart;
        YandexGame.CloseFullAdEvent -= CloseFullAd;
        ShopManager.onShowBuyMenu -= ShowBuyMenu;
        ShopManager.onCloseShop -= CloseShop;
        PlayerUpgradeShop.onShowUpgradeShop -= ShowUpgradeShop;
        PlayerUpgradeShop.onCloseUpgradeShop -= CloseUpgradeShop;
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    private void CristalChange(int count, int changeAmmount)
    {
        cristalText.text = count.ToString();
    }

    private void ShowReward(){
        YandexGame.RewVideoShow(2);
    }

    private void Rewarded(int id){
        if (id == 2){            
            MoneyManager.Instance.AddMoney(50);            
            YandexGame.savesData.money = MoneyManager.Instance.GetMoneyCount();         
            pauseMenu.SetActive(false); 
            openPauseButton.gameObject.SetActive(true);
            if(!ismobile){
                BlockCursor();
            }  
            HiderObjects(true);
         }        
    }

   


    private void ShowBuyMenu(){
        openPauseButton.gameObject.SetActive(false);
        if(ismobile) return;
        isShopOpen=true;
        UnlockCursor();
    }

    private void CloseShop()
    {
       openPauseButton.gameObject.SetActive(true);
       if(ismobile) return;
       isShopOpen=false;
       BlockCursor();
    }
    private void ShowUpgradeShop(){
        openPauseButton.gameObject.SetActive(false);
        if(ismobile) return;
        isUpgradeOpen=true;
        UnlockCursor();
    }
    private void CloseUpgradeShop(){
        openPauseButton.gameObject.SetActive(true);
        if(ismobile) return;
        isUpgradeOpen=false;
        BlockCursor();
    }




     private void BlockCursor()
        {
            if (!ismobile)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;                
            }
        }

        private void UnlockCursor(){            
            if(!ismobile){
               Cursor.lockState = CursorLockMode.None;
               Cursor.visible = true;
            }
        }






    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!YandexGame.EnvironmentData.isMobile&&!isShopOpen&&!isUpgradeOpen&&!PlayerHealth.Instance.IsDead())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OpenPause();
        }
    }

    private void HiderObjects(bool isHide){
        foreach(var v in hideOnPause){
            v.SetActive(isHide);
        }
    }
    private void OpenPause(){
        pauseMenu.SetActive(true);
        openPauseButton.gameObject.SetActive(false);
        Time.timeScale=0;
        HiderObjects(false);
    }
    private void ClosePause()
    {        
        HiderObjects(true);
        pauseMenu.SetActive(false);
        YandexGame.SaveProgress();
         openPauseButton.gameObject.SetActive(true);
        Time.timeScale=1;
        if(ismobile) return;
        if(isShopOpen||isUpgradeOpen){
           return;
        }
        BlockCursor();
    }


    

    private void CloseFullAd()
    {     
         BlockCursor();
    }

    private void TimerBeforeStart(int time)
    {
        timerObject.SetActive(true);
        timerText.text = "До старта новой волны: " + time.ToString();
    }

    private void WaveIsDestroyd()
    {
        waveinfoObject.SetActive(false);
        countEnemyObject.SetActive(false);
    }

    private void StartWave(int waveNumber)
    {
        timerObject.SetActive(false);
        waveinfoObject.SetActive(true);
        countEnemyObject.gameObject.SetActive(true);
        waveinfoText.text = "Волна " + waveNumber.ToString();
    }

    private void EnemyLeft(int count)
    {
        //
        countEnemyText.text ="Осталось зомби " + count.ToString();
    } 

    private void BossStart()
    {       
        countEnemyObject.gameObject.SetActive(false);
    }
   

    private void HideInteractButtons()
    {
        currentInteractObject = null;
        takeInteractButton.gameObject.SetActive(false);
        interactButton.gameObject.SetActive(false);
    }
    private void ReadyToInteract(InteractibleObjects interactibleObjects, bool isActive,bool isUsable,bool isTakable)
    {
        if (isActive)
        {
            if (interactibleObjects != null)
            {
                currentInteractObject = interactibleObjects;
                if (isUsable)
                    interactButton.gameObject.SetActive(true);
                if (isTakable)
                {
                    takeInteractButton.gameObject.SetActive(true);
                    if (!currentInteractObject.isTaken)
                    {
                        takeInteractText.text = "TAKE";
                    }
                }
            }

        }
        else
        {
            if (interactibleObjects != null)
            {
                currentInteractObject = interactibleObjects;
                if (!currentInteractObject.isTaken)
                {
                    currentInteractObject = null;
                    interactButton.gameObject.SetActive(false);
                    takeInteractButton.gameObject.SetActive(false);
                }
                else
                {
                    return;
                }
            }
            else
            {
                interactButton.gameObject.SetActive(false);
                takeInteractButton.gameObject.SetActive(false);
            }
        }
       
    }

    private void TakeInteract()
    {
        if (currentInteractObject != null)
        {
            if (currentInteractObject.isTakable)
            {                
                onTakeInteractible?.Invoke(currentInteractObject);                
                takeInteractButton.gameObject.SetActive(false);
                currentInteractObject = null;              
            }
           
        }
    }

    private void Interact()
    {
        if (currentInteractObject != null)
        {
            currentInteractObject.Interact();
        }
    }
    
    
    



   


    

    private void OpenShop()
    {
        Time.timeScale = 0;
        shopPanel.SetActive(true);
        onShowShop?.Invoke();
    }
    



    private void ChangeWeapon()
    {
        onChangeWeapon?.Invoke();
    }

   

    public void TakeKey(Vector3 pos, ItemKey itemKey)
    {
        blockDoor.SetActive(true);
        blockDoorKeyInfo.text = itemKey.description;
    }


    public void GoToShop()
    {
        goToShopObject.SetActive(true);
    }
    public void StopGoToShop()
    {
        goToShopObject.SetActive(false);
    }

   

   
    private void MoneyChange(int moneyCount, int amount)
    {
        AnimTweenManager.DoScaleShake(moneyText.gameObject.transform, 1.3f, 0.2f);
        moneyText.text = moneyCount.ToString();
        if (amount > 0)
        {
            moneyAmountText.text = "+" + amount.ToString();
            moneyAmountText.color = Color.green;
            moneyAmount.SetActive(true);
        }
        else if(amount < 0)
        {
            moneyAmountText.text = amount.ToString();
            moneyAmountText.color = Color.red;
            moneyAmount.SetActive(true);
        }      
    }

    private void ShopReady(bool showShop)
    {
        if (showShop)
        {
            shopButton.SetActive(true);
        }
        else
        {
            shopButton.SetActive(false);
        }
    }

    private void HeadShot()
    {
        headShotMark.SetActive(true);
        /*if(MarkerCor!=null) 
        {
            StopCoroutine(MarkerCor);
            MarkerCor=null;
        }
        MarkerCor = StartCoroutine(HeadMarker());  */
    }

    IEnumerator HeadMarker()
    {
        headShotMark.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        headShotMark.SetActive(false);
    }
   


    private void ShowAdsRestart()
    {
        int currentsceneIndex = SceneManager.GetActiveScene().buildIndex;                  
        YandexGame.FullscreenShow();       
        SceneManager.LoadScene(currentsceneIndex);
    }

    private void ShowAdsMain()
    {
        if (AdsTimer.Instance.isCanShowAds)
        {           
            YandexGame.FullscreenShow();
        }
        else SceneManager.LoadScene("PreStartScene");

    }

    private void Leaders()
    {        
        SceneManager.LoadScene("PreStartScene");
    }

    private void ChangeSprite(WeaponName weaponName)
    {
        if (!changeButon.gameObject.activeInHierarchy) changeButon.gameObject.SetActive(true);
        Sprite sprite;
        if (weaponName == WeaponName.Axe)
        {
            changeButtonImage.enabled = true;
            sprite = axeSprite;
            changeButtonImage.sprite = sprite;
        }

        else if (weaponName == WeaponName.ShotGun)
        {
            sprite = shotguSprite;
            changeButtonImage.sprite = sprite;

        }
        else if (weaponName == WeaponName.Revolver)
        {
            sprite = revolverSprite;
            changeButtonImage.sprite = sprite;
        }
        else if (weaponName == WeaponName.Uzi)
        {
            sprite = uziSprite;
            changeButtonImage.sprite = sprite;
        }
        else if (weaponName == WeaponName.AKM)
        {
            sprite = akmSprite;
            changeButtonImage.sprite = sprite;
        }
        else if (weaponName == WeaponName.Pistol)
        {
            sprite = pistolSprite;
            changeButtonImage.sprite = sprite;
        }
         else if (weaponName == WeaponName.ChainSaw)
        {
            sprite = chainSawSprite;
            changeButtonImage.sprite = sprite;
        }

        else
        {
            changeButtonImage.enabled = false;
        }

    }



    private void EmptyItem(VegetablesType type)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].textMesh.text == type.ToString())
            {
                Destroy(buttons[i].gameObject);
                buttons.RemoveAt(i);

            }
        }

    }



    public void ChangeVegatableCount(VegetablesType type, int count)
    {
        foreach (var b in buttons)
        {
            if (b.textMesh.text == type.ToString())
            {
                b.count.text = count.ToString();
                AnimTweenManager.DoScaleShake(b.transform, 1.3f, 0.1f);
            }
        }
    }

    private void TakeItem(VegetablesItem vegetablesItem, bool isSound)
    {
        if (!itemManager.HasItem(vegetablesItem.type))
        {
            int s = 0;
            foreach (var b in buttons)
            {
                if (b.nameButton == vegetablesItem.type.ToString())
                {
                    s++;
                }
            }
            if (s == 0)
            {
                ButtonElement itemB = Instantiate(elementButton, itemButtonHook);
                itemB.Init(vegetablesItem.type.ToString(), vegetablesItem.imageSprite, false, true);
                AddItemOnButton(vegetablesItem.type.ToString(), itemB.weaponButton);
                buttons.Add(itemB);
            }
        }
    }


    private void TakeWeapon(WeaponName weaponName, int ammmo)
    {
        Sprite sprite;
        if (weaponName == WeaponName.Axe)
        {
            sprite = axeSprite;
        }
        else if (weaponName == WeaponName.ShotGun)
        {
            sprite = shotguSprite;
        }
        else if (weaponName == WeaponName.Uzi)
        {
            sprite = uziSprite;
        }
        else if (weaponName == WeaponName.AKM)
        {
            sprite = akmSprite;
        }
        else if (weaponName == WeaponName.Pistol)
        {
            sprite = pistolSprite;
        }
         else if (weaponName == WeaponName.ChainSaw)
        {
            sprite = chainSawSprite;
        }
        else
            sprite = revolverSprite;
        ButtonElement weaponB = Instantiate(elementButton, weaponButtonHook);
        weaponB.Init(weaponName.ToString(), sprite, false, false);
        AddWeaponOnButton(weaponName, weaponB.weaponButton);

    }

    private void AddItemOnButton(String name, Button button)
    {
        switch (name)
        {
            case "Potato":
                button.onClick.AddListener(ChoosePotato);
                break;
            case "Pumpkin":
                button.onClick.AddListener(ChoosePumpkin);
                break;
            case "Carrot":
                button.onClick.AddListener(ChooseCarrot);
                break;
            case "Tomato":
                button.onClick.AddListener(ChooseTomato);
                break;
            case "Onion":
                button.onClick.AddListener(ChooseOnion);
                break;
            case "Squash":
                button.onClick.AddListener(ChooseSquash);
                break;
            case "Strawberry":
                button.onClick.AddListener(ChooseStrawberry);
                break;
        }
    }


    private void ChoosePotato()
    {
        onChooseItem?.Invoke(VegetablesType.Potato);
    }
    private void ChoosePumpkin()
    {
        onChooseItem?.Invoke(VegetablesType.Pumpkin);
    }
    private void ChooseCarrot()
    {
        onChooseItem?.Invoke(VegetablesType.Carrot);
    }
    private void ChooseTomato()
    {
        onChooseItem?.Invoke(VegetablesType.Tomato);
    }
    private void ChooseSquash()
    {
        onChooseItem?.Invoke(VegetablesType.Squash);
    }
    private void ChooseStrawberry()
    {
        onChooseItem?.Invoke(VegetablesType.Stawberry);
    }
    private void ChooseOnion()
    {
        onChooseItem?.Invoke(VegetablesType.Onion);
    }


    private void AddWeaponOnButton(WeaponName weaponName, Button button)
    {
        switch (weaponName)
        {
            case WeaponName.Axe:
                button.onClick.AddListener(ChoseAxe);
                break;
            case WeaponName.ShotGun:
                button.onClick.AddListener(ChoseShotGun);
                break;
            case WeaponName.Revolver:
                button.onClick.AddListener(ChoseRevolver);
                break;
            case WeaponName.Uzi:
                button.onClick.AddListener(ChoseUzi);
                break;
            case WeaponName.AKM:
                button.onClick.AddListener(ChoseAKM);
                break;
            case WeaponName.Pistol:
                button.onClick.AddListener(ChosePistol);
                break;
            case WeaponName.ChainSaw:
                button.onClick.AddListener(ChoseChainSaw);
                break;
            default: break;
        }
    }


    private void ChooseWeapon(WeaponName weaponName)
    {
        onChooseWeapon?.Invoke(weaponName);
        Debug.Log(weaponName);
    }

    public void ChoseAxe()
    {
        ChooseWeapon(WeaponName.Axe);
    }
    public void ChoseShotGun()
    {
        ChooseWeapon(WeaponName.ShotGun);
    }
    public void ChoseRevolver()
    {
        ChooseWeapon(WeaponName.Revolver);
    }
    public void ChoseUzi()
    {
        ChooseWeapon(WeaponName.Uzi);
    }
    public void ChoseAKM()
    {
        ChooseWeapon(WeaponName.AKM);
    }

    public void ChosePistol()
    {
        ChooseWeapon(WeaponName.Pistol);
    }
    public void ChoseChainSaw()
    {
        ChooseWeapon(WeaponName.ChainSaw);
    }



    private void ReadyToTake(IItem item)
    {
        curItem = item;
        takeButton.gameObject.SetActive(true);
    }

    private void NotReadyToTake()
    {
        curItem = null;
        takeButton.gameObject.SetActive(false);
    }
    [ContextMenu("Take")]
    private void Take()
    {
        if (curItem != null)
        {
            curItem.TakeItem(true);
            curItem = null;
        }
    }


   


    private void HideButtons()
    {
        weaponButtonHook.gameObject.SetActive(false);
        itemButtonHook.gameObject.SetActive(false);
    }


    private void ShowGameOverWindow()
    {
        endGameObject.SetActive(true);      
        HideButtons();
        openPauseButton.gameObject.SetActive(false);
        if(!ismobile)
        UnlockCursor();
    }
    private void Restart()
    {
        SaveManager.onDeleteSave?.Invoke();       
        //InterstitialAds.onShowInterstitial?.Invoke();
        loadingManager.LoadScene("FarmScene");
       
    }
    private void RestartWithProgress()
    {
               
    }

    private void ProgressLoad(string reward)
    {
        if (reward == "loadprogress")
        {
            SaveData.SetBool("loadprogress", true);
            loadingManager.LoadScene("FarmScene");
        }
    }

}
