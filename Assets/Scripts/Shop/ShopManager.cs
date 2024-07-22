using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Runtime.InteropServices;
using YG;

public class ShopManager : MonoBehaviour
{
    public static Action onCloseShop;
    public List<GameObject> hideObjects;   
    public GameObject headMark;


    public static Action onFirstVisit;
    public static Action<WeaponName> onBuyAmmo;
    public static Action onNotEnoughMoney;
    public static Action onNotEnoughSpace;
    public static Action onSellAllClick;
    public static Action onClearCards;
    public static Action onShowBuyMenu;
    public static Action onBuy;

    [SerializeField] private Button sellStoreMenuButton, buyStoreMenuButton;

    [SerializeField] private Line line;
    [SerializeField] private Transform hookLine;
    private int countOfCardInLine = 4;
    private List<Line> lines = new List<Line>();
    private List<Good> playerGoods = new List<Good>();
    [SerializeField] private List<GoodData> goodDatas;
    [SerializeField] private List<GoodData> randomOfferData;
    [SerializeField] private List<Good> offer;

    [SerializeField] private Button buyNButton, sellNButton;
    [SerializeField] private Button countUpButton;
    [SerializeField] private Button countDownButton;
    [SerializeField] private Button buyAllButton, sellAllButton;
     [SerializeField] private Button closeShopButton;

    [SerializeField] private TextMeshProUGUI priceNText;
    [SerializeField] private TextMeshProUGUI priceAllText;
    [SerializeField] private TextMeshProUGUI countToTradeText;
    [SerializeField] private TextMeshProUGUI moneyCount;

    TypeShop typeShopMenu;

    private Good currentGood;
    private int countToTrade;
    private int maxCountToTrade;
    private int priceN, priceAll;
    private Card lastcard;

    [SerializeField] private GameObject notEnoughObject;
    [SerializeField] private GameObject notEnoughSpaceObject;
    [SerializeField] private GameObject noProductsObject;
    [SerializeField] private GameObject bouthAllObject;
    [SerializeField] private TextMeshProUGUI goodsName;

    [SerializeField] private GameObject shopPanel;

    private List<Good> todayPrices = new List<Good>();

    [SerializeField] private List<Card> currentCards = new List<Card>();

    private int count = 0;

    [SerializeField] private TextMeshProUGUI adsTimer;

    private void Start()
    {
        count = 0;
        noProductsObject.SetActive(false);
        bouthAllObject.SetActive(false);
        onNotEnoughMoney += NotEnoughMoney;
        onNotEnoughSpace += NotEnoughSpace;
        MoneyManager.onMoneyChange += ChangeMoney;            
        sellStoreMenuButton.onClick.AddListener(ShowSellMenu);
        buyStoreMenuButton.onClick.AddListener(ShowBuyMenu);
        countUpButton.onClick.AddListener(UpCount);
        countDownButton.onClick.AddListener(DownCount);
        closeShopButton.onClick.AddListener(CloseShop);
        Card.onInitCard += InitCard;
        Card.onClickCard += ClickOnCard;       
        goodsName.text = "";
        ZombieManager.onWaveIsDestroyd+=ShowStore;
    }

    [ContextMenu("Close")]
    private void CloseShop()
    {
        count++;
        onCloseShop?.Invoke();
        Time.timeScale = 1;
        shopPanel.SetActive(false);
        HideObjects(false);
        if (count > 1)
        {
            adsTimer.text = "Рекламная пауза через 3";
            adsTimer.gameObject.SetActive(true);
            StartCoroutine(AdsCor());
        }
        YandexGame.SaveProgress();

    }

    private IEnumerator AdsCor()
    {
        yield return new WaitForSeconds(1);
        adsTimer.text = "Рекламная пауза через 2";
        yield return new WaitForSeconds(1);
        adsTimer.text = "Рекламная пауза через 1";
        yield return new WaitForSeconds(1);
        adsTimer.text = "Рекламная пауза через 0";
        adsTimer.gameObject.SetActive(false);
        YandexGame.FullscreenShow();
    }
    
    private void HideObjects(bool isHide){
        foreach(var v in hideObjects){
            v.SetActive(!isHide);
        }
    }


    private void InitCard(Card card)
    {
        currentCards.Add(card);
    } 

    private void ClearCards()
    {
        currentCards.Clear();
        onClearCards?.Invoke();
    }


    private void OnDestroy()
    {
        MoneyManager.onMoneyChange -= ChangeMoney;         
        onNotEnoughMoney -= NotEnoughMoney;
        onNotEnoughSpace -= NotEnoughSpace;
        Card.onInitCard -= InitCard;
        if(Card.onClickCard!=null)
        Card.onClickCard -= ClickOnCard;
        currentCards.Clear();
        sellStoreMenuButton.onClick.RemoveAllListeners();
        buyStoreMenuButton.onClick.RemoveAllListeners();
        countUpButton.onClick.RemoveAllListeners();
        countDownButton.onClick.RemoveAllListeners();
        sellAllButton.onClick.RemoveAllListeners();
        buyAllButton.onClick.RemoveAllListeners();
        sellNButton.onClick.RemoveAllListeners();
        buyNButton.onClick.RemoveAllListeners();
        ZombieManager.onWaveIsDestroyd-=ShowStore;
    }

      

    private void NotEnoughMoney()
    {
        notEnoughObject.SetActive(true);
    }
    private void NotEnoughSpace()
    {
        notEnoughSpaceObject.SetActive(true);
    }

    private void ChangeMoney(int amount, int count)
    {
        moneyCount.text = amount.ToString();
    }

    private void InitShopButtons(TypeShop typeShop)
    {        
        if (typeShop == TypeShop.SellType)
        {
            buyAllButton.gameObject.SetActive(false);
            buyNButton.gameObject.SetActive(false);
            sellAllButton.gameObject.SetActive(true);
            sellNButton.gameObject.SetActive(true);
            sellAllButton.onClick.RemoveAllListeners();
            sellNButton.onClick.RemoveAllListeners();
            sellAllButton.onClick.AddListener(SellAll);
            sellNButton.onClick.AddListener(Sell);
        }
        else if (typeShop == TypeShop.BuyType)
        {
            buyAllButton.gameObject.SetActive(true);
            buyNButton.gameObject.SetActive(true);
            sellAllButton.gameObject.SetActive(false);
            sellNButton.gameObject.SetActive(false);
            buyNButton.onClick.RemoveAllListeners();
            buyAllButton.onClick.RemoveAllListeners();
            buyAllButton.onClick.AddListener(BuyAll);
            buyNButton.onClick.AddListener(Buy);
        }
    }
       


    private void SellAll()
    {
        if (currentGood.typegood == Typegood.Vegetables)
        {
            foreach (var v in ItemManager.Instance.vegetables)
            {
                if (currentGood.name == v.type.ToString())
                {
                    onSellAllClick?.Invoke();
                    ItemManager.Instance.SellItem(v.type, -maxCountToTrade);
                    MoneyManager.Instance.AddMoney(maxCountToTrade * currentGood.price);
                    lastcard.Trade(maxCountToTrade);
                    //ReinitMenu();
                    StartCoroutine(ReinitCor());
                }
            }
        }

        /*if (currentGood.typegood == Typegood.Ammo)
        {
            foreach (var w in WeaponManager.Instance.weapons)
            {
                if (w.weaponModel.weaponName.ToString() == currentGood.name)
                {
                    InventoryManager.Instance.SellAmmo(w.weaponModel.weaponName, -maxCountToTrade * currentGood.specification);
                    MoneyManager.Instance.AddMoney(maxCountToTrade * currentGood.price);
                }
            }
        }*/
        CheckCountToTrade();

    }

    private void Sell()
    {

        if (currentGood.typegood == Typegood.Vegetables)
        {
            foreach (var v in ItemManager.Instance.vegetables)
            {
                if (currentGood.name == v.type.ToString())
                {
                    ItemManager.Instance.SellItem(v.type, -countToTrade);
                    MoneyManager.Instance.AddMoney(countToTrade * currentGood.price);
                    lastcard.Trade(countToTrade);

                    if (lastcard.good.count == 0) /*ReinitMenu();*/ StartCoroutine(ReinitCor());
                }
            }
        }
        CheckCountToTrade();
    }

    private void BuyAll()
    {
        if (MoneyManager.Instance.IsEnough(priceAll)&&priceAll>0)
        {
            if (currentGood.count > 0)
            {
                if (currentGood.typegood == Typegood.Ammo)
                {
                    string result = string.Empty;
                    int indexOfDelimiter = currentGood.name.IndexOf("_");
                    if (indexOfDelimiter != -1)
                    {
                        result = currentGood.name.Substring(0, indexOfDelimiter);
                    }
                    foreach (var v in WeaponManager.Instance.weapons)
                    {
                        if (result == v.weaponModel.weaponName.ToString())
                        {
                            onBuyAmmo?.Invoke(v.weaponModel.weaponName);
                            InventoryManager.onTakeAmmo(v.weaponModel.weaponName, currentGood.specification * maxCountToTrade);
                            MoneyManager.Instance.SpendMoney(maxCountToTrade * currentGood.price);
                            lastcard.Trade(maxCountToTrade);

                            if (lastcard.good.count == 0)
                            {
                                offer.Remove(currentGood);
                                StartCoroutine(ReinitCor());
                                //ReinitMenu();
                            }
                        }
                    }
                }
                else if (currentGood.typegood == Typegood.Weapon)
                {
                    string result = string.Empty;
                    int indexOfDelimiter = currentGood.name.IndexOf("_");
                    if (indexOfDelimiter != -1)
                    {
                        result = currentGood.name.Substring(0, indexOfDelimiter);
                    }

                    foreach (var w in WeaponManager.Instance.weapons)
                    {
                        if (result == w.weaponModel.weaponName.ToString())
                        {                           
                            InventoryManager.onTakeWeapon(w.weaponModel.weaponName, currentGood.specification);
                            MoneyManager.Instance.SpendMoney(currentGood.price);
                            lastcard.Trade(1);

                            if (lastcard.good.count == 0)
                            {
                                offer.Remove(currentGood);
                                StartCoroutine(ReinitCor());
                                //ReinitMenu();
                            }
                        }
                    }
                }
                else if (currentGood.typegood == Typegood.Interactible)
                {
                    string result = currentGood.name;

                    foreach (var w in ObjectManager.Instance.interactibleObjects)
                    {
                        if (result.ToLower() == w.typeInteractible.ToString().ToLower())
                        {
                            if (ObjectManager.Instance.IsHasSlotWhenTryBut(w))
                            {                                
                                ObjectManager.Instance.SpawnGood(w, currentGood.specification * maxCountToTrade);
                                MoneyManager.Instance.SpendMoney(currentGood.price * maxCountToTrade);
                                lastcard.Trade(maxCountToTrade);

                                if (lastcard.good.count == 0)
                                {
                                    offer.Remove(currentGood);
                                    StartCoroutine(ReinitCor());
                                    //ReinitMenu();
                                }
                            }
                            else onNotEnoughSpace?.Invoke();
                        }
                    }
                }

            }
            else
            {
                return;
            }
        }
        else
        {
            onNotEnoughMoney?.Invoke();
        }
        CheckCountToTrade();
    }

    private void Buy()
    {
        if (MoneyManager.Instance.IsEnough(countToTrade * currentGood.price))
        {
            onBuy?.Invoke();
            if (currentGood.count > 0)
            {
                if (currentGood.typegood == Typegood.Ammo)
                {
                    string result = string.Empty;
                    int indexOfDelimiter = currentGood.name.IndexOf("_");
                    if (indexOfDelimiter != -1)
                    {
                        result = currentGood.name.Substring(0, indexOfDelimiter);
                    }
                    foreach (var v in WeaponManager.Instance.weapons)
                    {
                        if (result == v.weaponModel.weaponName.ToString())
                        {
                            onBuyAmmo?.Invoke(v.weaponModel.weaponName);
                            InventoryManager.onTakeAmmo(v.weaponModel.weaponName, currentGood.specification * countToTrade);
                            MoneyManager.Instance.SpendMoney(countToTrade * currentGood.price);
                            lastcard.Trade(countToTrade);

                            if (lastcard.good.count == 0)
                            {
                                offer.Remove(currentGood);
                                StartCoroutine(ReinitCor());
                                //ReinitMenu();
                            }
                        }
                    }
                }
                else if (currentGood.typegood == Typegood.Weapon)
                {
                    string result = string.Empty;
                    int indexOfDelimiter = currentGood.name.IndexOf("_");
                    if (indexOfDelimiter != -1)
                    {
                        result = currentGood.name.Substring(0, indexOfDelimiter);
                    }
                    foreach (var v in WeaponManager.Instance.weapons)
                    {
                        if (result == v.weaponModel.weaponName.ToString())
                        {                           
                            InventoryManager.onTakeWeapon(v.weaponModel.weaponName, currentGood.specification);
                            MoneyManager.Instance.SpendMoney(currentGood.price);
                            lastcard.Trade(1);

                            if (lastcard.good.count == 0)
                            {
                                offer.Remove(currentGood);
                                StartCoroutine(ReinitCor());
                                //ReinitMenu();
                            }
                        }
                    }
                }
                else if (currentGood.typegood == Typegood.Interactible)
                {
                    string result = currentGood.name;                    

                    foreach (var w in ObjectManager.Instance.interactibleObjects)
                    {
                        if (result.ToLower() == w.typeInteractible.ToString().ToLower())
                        {
                            if (ObjectManager.Instance.IsHasSlotWhenTryBut(w))
                            {                              
                                ObjectManager.Instance.SpawnGood(w, currentGood.specification);
                                MoneyManager.Instance.SpendMoney(currentGood.price);
                                lastcard.Trade(1);

                                if (lastcard.good.count == 0)
                                {
                                    offer.Remove(currentGood);
                                    StartCoroutine(ReinitCor());
                                    //ReinitMenu();
                                }
                            }
                            else
                            {
                                onNotEnoughSpace?.Invoke();
                            }
                        }
                    }
                }
                
            }
            else
            {
                return;
            }
        }
        else
        {
            onNotEnoughMoney?.Invoke();
        }
        CheckCountToTrade();
    }


    private void UpCount()
    {
        if (currentGood == null) return;
        if (typeShopMenu == TypeShop.SellType)
        {
            if (countToTrade < maxCountToTrade)
            {
                countToTrade++;
                countToTradeText.text = countToTrade.ToString();
            }
        }
        else if (typeShopMenu == TypeShop.BuyType)
        {
            if (countToTrade < maxCountToTrade)
            {
                countToTrade++;
                countToTradeText.text = countToTrade.ToString();
            }
        }
        priceN = currentGood.price * countToTrade;
        priceNText.text = priceN.ToString();
    }
    private void DownCount()
    {
        if (currentGood == null) return;
        if (countToTrade > 1)
        {
            countToTrade--;
            countToTradeText.text = countToTrade.ToString();
        }
        priceN = currentGood.price * countToTrade;
        priceNText.text = priceN.ToString();
    }




    private void ClearLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i].gameObject);
        }
        lines.Clear();
    }

   

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(0.8f);        
        if (!PlayerHealth.Instance.IsDead())
        {
            if (!PlayerUpgradeShop.isUpgradeShopOpen)
            {
                headMark.SetActive(false);
                Time.timeScale = 0;
                shopPanel.SetActive(true);
                typeShopMenu = TypeShop.None;
                CheckOfferGoods();
                HideObjects(true);
                ShowBuyMenu();
            }
        }
        else{           
            StartCoroutine(Show());
        }
    }
    [ContextMenu("Show")]
    private void ShowStore()
    {       
       StartCoroutine(Show());
    }

    public void GoTime()
    {
        onFirstVisit?.Invoke();
    }

    private void CheckPlayerGoods()
    {
        playerGoods.Clear();
        foreach (var v in ItemManager.Instance.vegetables)
        {
            if (v.count > 0)
            {
                foreach (var g in goodDatas)
                {
                    if (v.type.ToString() == g.nameGoods)
                    {
                        Good curGood = new Good(g, g.price, v.count);
                        playerGoods.Add(curGood);
                    }
                }

            }
        }

        /*foreach (var w in WeaponManager.Instance.weapons)
        {
            if (WeaponManager.Instance.GetAmmo(w.weaponModel.weaponName) > 0)
            {
                foreach (var g in goodDatas)
                {
                    if (w.weaponModel.weaponName.ToString() == g.nameGoods)
                    {
                        var countammopack = WeaponManager.Instance.GetAmmo(w.weaponModel.weaponName) / g.specification;
                        if (countammopack > 0)
                        {
                            Good curgood = new Good(g, countammopack);
                            playerGoods.Add(curgood);
                        }
                    }
                }
            }
        }*/
    }

   

    private void CheckOfferGoods()
    {
        offer.Clear();
        foreach (var r in randomOfferData)
        {

            if (r.typegood == Typegood.Ammo)
            {
                Good curgood = new Good(r,r.price, 1000);
                offer.Add(curgood);
            }
            else if (r.typegood == Typegood.Weapon)
            {
                int indexOfDelimiter = r.nameGoods.IndexOf("_");

                if (indexOfDelimiter != -1)
                {
                    string result = r.nameGoods.Substring(0, indexOfDelimiter);
                    Debug.Log(result);
                    if (!WeaponManager.Instance.isHasWeapon(result))
                    {
                        Good curgood = new Good(r, r.price, 1);
                        offer.Add(curgood);
                    }
                }
            }
            /*else if (r.typegood == Typegood.Interactible)
            {
                if (r.nameGoods == "Radio"||r.nameGoods== "ChainSaw" || r.nameGoods== "FlameThrower")
                {
                    Good curgood = new Good(r, r.price, 1);
                    offer.Add(curgood);
                }
                else
                {
                    Good curgood = new Good(r, r.price, UnityEngine.Random.Range(1, 20));
                    offer.Add(curgood);
                }

            }*/
        }
    }
    private List<Good> GetRandonOffer()
    {
        return offer;
    }



    private void ShowSellMenu()
    {
        goodsName.text = "";
        ClearCards();
        bouthAllObject.SetActive(false);
        if (typeShopMenu == TypeShop.SellType) return;
        NotInteractible();
        currentGood = null;
        ClearLines();
        typeShopMenu = TypeShop.SellType;
        InitShopButtons(typeShopMenu);
        CheckPlayerGoods();
        InitGoodPanel(playerGoods);
        if (playerGoods.Count == 0)
        {
            noProductsObject.SetActive(true);
        }
    }
    private IEnumerator ReinitCor()
    {
        yield return null;
        ReinitMenu();
    }

    private void ReinitMenu()
    {
        NotInteractible();
        goodsName.text = "";
        if (typeShopMenu == TypeShop.SellType)
        {            
            ClearLines();
            ClearCards();
            InitShopButtons(typeShopMenu);
            CheckPlayerGoods();
            InitGoodPanel(playerGoods);            
            if (playerGoods.Count == 0)
            {
                noProductsObject.SetActive(true);
                return;
            }
            currentCards[currentCards.Count - 1].ClickOnCard();
            return;
        }
        else if (typeShopMenu == TypeShop.BuyType)
        {
            ClearLines();
            ClearCards();
            InitShopButtons(typeShopMenu);
            InitGoodPanel(GetRandonOffer());
            if (offer.Count == 0)
            {
                bouthAllObject.SetActive(true);
            }
        }

       


    }


    private void NotInteractible()
    {
        lastcard = null;
        maxCountToTrade = 0;
        countToTradeText.text = "";
        priceAll = 0;
        priceAllText.text = "";
        priceN = 0;
        priceNText.text = "";
        sellNButton.interactable = false;
        sellAllButton.interactable = false;
        buyAllButton.interactable = false;
        buyNButton.interactable = false;
        countUpButton.interactable = false;
        countDownButton.interactable = false;
    }
    private void NowInteractible()
    {
        countToTrade = 0;

        sellNButton.interactable = true;
        sellAllButton.interactable = true;
        buyAllButton.interactable = true;
        buyNButton.interactable = true;
        countUpButton.interactable = true;
        countDownButton.interactable = true;
    }

    private void ShowBuyMenu()
    {
        goodsName.text = "";
        ClearCards();
        onShowBuyMenu?.Invoke();
        noProductsObject.SetActive(false);
        if (typeShopMenu == TypeShop.BuyType) return;
        NotInteractible();
        currentGood = null;
        ClearLines();
        typeShopMenu = TypeShop.BuyType;
        InitShopButtons(typeShopMenu);
        InitGoodPanel(GetRandonOffer());
        if (offer.Count == 0)
        {
            bouthAllObject.SetActive(true);
        }
    }



    private void InitGoodPanel(List<Good> goods)
    {
        int n = 0;
        for (int i = 0; i < CheckCountofLine(); i++)
        {
            var curline = Instantiate(line, hookLine);
            curline.SetMaxCapacity(countOfCardInLine);
            lines.Add(curline);

            for (int j = 0; j < lines[i].maxcapacity; j++)
            {
                if (lines[i].isEmpty() && n < goods.Count)
                {
                    lines[i].Setup(goods[n]);
                    n++;
                }
                else
                {
                    lines[i].Setup(null);
                }
            }
        }
       
    }


    private void ClickOnCard(Card card)
    {
        if (card == lastcard) return;
        if (lastcard != null)
        {
            lastcard.UnClick();
        }
        lastcard = card;
        NowInteractible();
        countToTrade = 1;
        countToTradeText.text = "1";
        currentGood = card.good;
        priceN = card.good.price * countToTrade;
        priceNText.text = priceN.ToString();
        goodsName.text = currentGood.nameForTitle;

        CheckCountToTrade();


    }

    private void CheckCountToTrade()
    {
        if (lastcard == null) return;
        if (typeShopMenu == TypeShop.SellType)
        {
            maxCountToTrade = lastcard.good.count;
            priceAll = lastcard.good.count * lastcard.good.price;
            priceAllText.text = priceAll.ToString();
            if (countToTrade > lastcard.good.count)
            {
                countToTrade = lastcard.good.count;
                countToTradeText.text = countToTrade.ToString();
                priceN = lastcard.good.price * countToTrade;
                priceNText.text = priceN.ToString();
            }

        }
        else if (typeShopMenu == TypeShop.BuyType)
        {
            maxCountToTrade = MoneyManager.Instance.HowManyCanBuy(lastcard.good.price);
            if (maxCountToTrade > currentGood.count) maxCountToTrade = currentGood.count;
            priceAll = maxCountToTrade * lastcard.good.price;
            priceAllText.text = priceAll.ToString();
            if (countToTrade > lastcard.good.count)
            {
                countToTrade = lastcard.good.count;
                countToTradeText.text = countToTrade.ToString();
                priceN = lastcard.good.price * countToTrade;
                priceNText.text = priceN.ToString();
            }
        }
    }





    private int CheckCountOfGoods()
    {
        if (typeShopMenu == TypeShop.SellType)
        {
            return playerGoods.Count;
        }
        else if (typeShopMenu == TypeShop.BuyType)
        {
            return offer.Count;
        }
        else return 0;
    }
    private int CheckCountofLine()
    {
        return DivideAndRoundUp(CheckCountOfGoods(), countOfCardInLine);
    }
    public static int DivideAndRoundUp(int dividend, int divisor)
    {
        return (int)Math.Ceiling((double)dividend / divisor);
    }

}

public enum TypeShop
{
    SellType, BuyType, RealType, None,
}
