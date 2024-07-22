using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class InventoryManager : MonoBehaviour
{   
    public static Action<IItem> onReadyToTake;
    public static Action<WeaponName, int> onTakeWeapon;
    [SerializeField] private UnityEvent OnTakeWeapon;
    public static Action<WeaponName, int> onTakeAmmo;
    [SerializeField] private UnityEvent OnTakeAmmo;
    public static Action<VegetablesItem, bool> onTakeItem;
    [SerializeField] private UnityEvent OnTakeItem;
    public static Action notReadyToTake;
    [SerializeField] private List<IItem> items=new List<IItem>();
    [SerializeField] private List<WeaponName> weapons = new List<WeaponName>();
    public IItem currentIndicateItem;

    private bool isNeedCheck;

    private bool coolDown;


    public static InventoryManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }


    private void Start()
    {
        coolDown = false;
        WeaponItem.onTakeWeapon += TakeWeapon;
        AmmoItem.onTakeAmmo += TakeAmmo;
        VegetablesItem.onTakeVegetable += TakeVegetable;
        
    }
    private void OnDestroy()
    {
        WeaponItem.onTakeWeapon -= TakeWeapon;
        AmmoItem.onTakeAmmo -= TakeAmmo;
        VegetablesItem.onTakeVegetable -= TakeVegetable;
       
    }

    private void SetAmmoAfterReward(WeaponName weaponName, int count)
    {
        onTakeAmmo?.Invoke(weaponName, count);
    }


    private void TakeVegetable(VegetablesItem vegetablesItem, GameObject vegetableObject, bool isSound)
    {
        onTakeItem.Invoke(vegetablesItem, isSound);        
        StartCoroutine(Destroyer(vegetableObject));
    }

    


    private void TakeAmmo(WeaponName weaponName, int ammo, GameObject itemObject)
    {
        onTakeAmmo?.Invoke(weaponName, ammo);
        Weapon.onEmptyRequest?.Invoke(weaponName,ammo);
        StartCoroutine(Destroyer(itemObject));
    }

    public void SellAmmo(WeaponName weaponName, int ammo)
    {
        onTakeAmmo?.Invoke(weaponName, ammo);
    }


    private void TakeWeapon(WeaponName weaponName, int ammo, GameObject itemObject)
    {
        if (!weapons.Contains(weaponName))
        {
            weapons.Add(weaponName);
            onTakeWeapon?.Invoke(weaponName, ammo);
            StartCoroutine(Destroyer(itemObject));
        }
    }

    private IEnumerator Destroyer(GameObject itemObject)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(itemObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<IItem>().TakeItem(true);
            isNeedCheck = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<IItem>().TakeItem(true);
            isNeedCheck = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<IItem>().StopIndicate();
            isNeedCheck = false;
            notReadyToTake?.Invoke();
        }
    }

    private void Update()
    {
        if (!isNeedCheck) { return; }       
       
    }

    private IEnumerator RayCoolDawn()
    {
        coolDown = true;
        yield return null;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        int layerMask = LayerMask.GetMask("Item");
        RaycastHit[] hits = Physics.RaycastAll(ray, 2.5f, layerMask);
        RaycastHit closestHit = new RaycastHit(); ;
        float closestDistance = Mathf.Infinity;
        bool foundClosestHit = false;

        foreach (var hit in hits)
        {
            if (hit.distance < closestDistance)
            {
                closestHit = hit;
                closestDistance = hit.distance;
                foundClosestHit = true;
            }
        }

        if (foundClosestHit)
        {
            if (closestHit.collider.CompareTag("Item"))
            {
                IItem curItem = closestHit.collider.gameObject.GetComponent<IItem>();
                onReadyToTake?.Invoke(curItem);
            }
            else
            {
                notReadyToTake?.Invoke();
            }
        }
        else
        {
            notReadyToTake?.Invoke();
        }
        yield return null;
        yield return null;
        yield return null;
        coolDown = false;
    } 
    

}
