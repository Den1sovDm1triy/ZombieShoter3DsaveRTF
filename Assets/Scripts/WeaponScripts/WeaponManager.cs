using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;
public class WeaponManager : Saver
{
    public static Action <WeaponName> onChange;
    public List<Weapon> weapons;    
    private Weapon currentWeapon;
    private Weapon lastWeapon;
    public static Action<WeaponName, WeaponModel> onChangeWeapon;
    public Action<int, int> onTakeWeapon;
    public Action<int, int> onTakeAmmo;
    public static Action onCheckAmmo;

    [SerializeField] private UnityEvent OnTakeItem;
    [SerializeField] private UnityEvent OnTakeAmmo;
    [SerializeField] private UnityEvent OnDeactivate;

    public static WeaponManager Instance;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        ObjectManager.onUseInteractibleWeapon += UseInteractWeapon;
    }

    public bool isHasWeapon(string weaponName)
    {
        bool isHas=false;
        foreach(var v in weapons){
            if (v.weaponModel.weaponName.ToString() == weaponName){
                if(v.isTaken){
                    isHas=true;
                }
            }
        }
        return isHas;
    }


    private void UseInteractWeapon(bool isActiveInteract)
    {
        if (isActiveInteract)
        {
            if (currentWeapon != null)
            {
                lastWeapon = currentWeapon;
                currentWeapon.Deactivate();
                currentWeapon = null;
            }
        }
        else
        {
            ChangeWeapon(lastWeapon.weaponModel.weaponName);
        }
    }

    public override void Save()
    {
        foreach(var w in weapons)
        {
            SaveData.SetBool(w.weaponModel.weaponName.ToString(), w.isTaken);
            SaveData.SetInt(w.weaponModel.weaponName.ToString() + "ammo", w.weaponModel.weaponAmmo.GetAmmoCount());
            SaveData.SetInt(w.weaponModel.weaponName.ToString() + "magazin", w.weaponModel.CurrentCapacity());
            SaveData.SetBool(w.weaponModel.weaponName.ToString() + "state", w.stateWeapon == StateWeapon.ActiveState ? true : false);
        }
    }


    public override void Load()
    {       
       StartCoroutine(LoadWeapon());
    }

    private IEnumerator LoadWeapon()
    {
        foreach (var w in weapons)
        {
            w.isTaken = SaveData.GetBool(w.weaponModel.weaponName.ToString());
            if (w.isTaken)
            {
                InventoryManager.onTakeWeapon?.Invoke(w.weaponModel.weaponName, SaveData.GetInt(w.weaponModel.weaponName.ToString() + "magazin"));
                int ammoS = SaveData.GetInt(w.weaponModel.weaponName.ToString() + "ammo");
                InventoryManager.onTakeAmmo?.Invoke(w.weaponModel.weaponName, ammoS);
                yield return new WaitForSeconds(0.2f);
            }
        }
       
        
    }


    public override void DeleteSave()
    {
        foreach (var w in weapons)
        {
            SaveData.Delete(w.weaponModel.weaponName.ToString());
            SaveData.Delete(w.weaponModel.weaponName.ToString() + "ammo");
            SaveData.Delete(w.weaponModel.weaponName.ToString() + "magazin");
            SaveData.Delete(w.weaponModel.weaponName.ToString() + "state");
        }
    }

    private void Start()
    {


        //Weapon.onChangeToAxe += ActivaAxe;
        UIManager.onChangeWeapon += ChangeWeapon;
        PlayerHealth.onDeath += Death;
        InventoryManager.onTakeWeapon += ActivateWeapon;        
        InventoryManager.onTakeAmmo += TakeAmmo;
        UIManager.onChooseWeapon += ChangeWeapon;
        foreach (var w in weapons)
        {
            w.Deactivate();
        }
        currentWeapon = null;
        if (!SaveData.Has("progress"))
        {
            StartCoroutine(ActivateAxeonStart());
        }
    }

   

    IEnumerator ActivateAxeonStart()
    {
        yield return new WaitForSeconds(0.2f);
        InventoryManager.onTakeWeapon?.Invoke(WeaponName.Axe, 0);
        yield return new WaitForSeconds(0.2f);
        InventoryManager.onTakeWeapon?.Invoke(WeaponName.ShotGun, 30);
        //yield return new WaitForSeconds(0.2f);
        //InventoryManager.onTakeWeapon?.Invoke(WeaponName.Revolver, 150);
        //yield return new WaitForSeconds(0.2f);
        //InventoryManager.onTakeWeapon?.Invoke(WeaponName.AKM, 150);
    }

   

    private void Death()
    {
        foreach (var w in weapons)
        {
            w.LooseWeapon();
        }
    }
    private void ChangeWeapon()
    {
        int nextWeaponIndex = GetNextAvailableWeaponIndex();
        if (nextWeaponIndex != -1)
        {
            ChangeWeapon(weapons[nextWeaponIndex].weaponModel.weaponName);           
        }
        else
        {
            return;          
        }
    }
    private void ChangeWeaponNext()
    {
        int nextWeaponIndex = GetNextAvailableWeaponIndex();
        if (nextWeaponIndex != -1)
        {
            ChangeWeapon(weapons[nextWeaponIndex].weaponModel.weaponName);           
        }
        else
        {
            return;          
        }
    }
     private void ChangeWeaponPrivious()
    {
        int nextWeaponIndex = GetPriveousAvailableWeaponIndex();
        if (nextWeaponIndex != -1)
        {
            ChangeWeapon(weapons[nextWeaponIndex].weaponModel.weaponName);           
        }
        else
        {
            return;          
        }
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
           ChangeWeaponPrivious();
        }
        if(Input.GetKeyDown(KeyCode.E)){
           ChangeWeaponNext();
        }
    }


    private int GetNextAvailableWeaponIndex()
    {
        int currentIndex = weapons.IndexOf(currentWeapon);
        Debug.Log(currentIndex);
        int nextIndex = (currentIndex + 1) % weapons.Count;

        
        bool hasAvailableWeapon = weapons.Any(weapon => weapon.isTaken);
        
        if (!hasAvailableWeapon)
        {
            return -1;
        }

        while (!weapons[nextIndex].isTaken)
        {
            nextIndex = (nextIndex + 1) % weapons.Count;
        }

        return nextIndex;
    }
    private int GetPriveousAvailableWeaponIndex()
    {
        int currentIndex = weapons.IndexOf(currentWeapon);
        Debug.Log(currentIndex);
        int nextIndex = (currentIndex - 1+weapons.Count) % weapons.Count;


        
        bool hasAvailableWeapon = weapons.Any(weapon => weapon.isTaken);
        
        if (!hasAvailableWeapon)
        {
            return -1;
        }

        while (!weapons[nextIndex].isTaken)
        {
            nextIndex = (nextIndex - 1) % weapons.Count;
        }

        return nextIndex;
    }


    
   
    private void ChangeWeapon(WeaponName weaponName)
    {
        if (currentWeapon == null)
        {
            ActivateWeaponAfterTake(weaponName);
            onChange?.Invoke(weaponName);
        }
        else if (currentWeapon.weaponModel.weaponName == weaponName)
        {
            return;/*
            currentWeapon.Deactivate();
            currentWeapon = null;
            onChangeWeapon?.Invoke(weaponName, null);
            OnDeactivate?.Invoke();
            onChange?.Invoke(WeaponName.nothing);*/ //убираем убирание оружия
        }
        else if (currentWeapon.weaponModel.weaponName != weaponName)
        {
            ActivateWeaponAfterTake(weaponName);
            onChange?.Invoke(weaponName);
        }



    }

    private void ActivateWeaponAfterTake(WeaponName weaponName)
    {
        OnTakeItem?.Invoke();
        if (currentWeapon != null) currentWeapon.Deactivate();
        foreach (var w in weapons)
        {
            if (w.weaponModel.weaponName == weaponName)
            {              
                onChangeWeapon?.Invoke(weaponName, w.weaponModel);                
                w.Activate(w.weaponModel.weaponAmmo.GetAmmoCount());                
                if (w.isEmpty)
                {                   
                    w.CheckAmmo();
                }

                currentWeapon = w;
                return;
            }
        }
    }



    public int GetAmmo(WeaponName weaponName)
    {
        int count = 0;
        foreach (var w in weapons)
        {
            if (w.weaponModel.weaponName == weaponName)
            {                               
                count = w.weaponModel.weaponAmmo.GetAmmoCount();
                Debug.Log("Ïàòðîíîâ øòóê" + count);
            }
        }
        return count;
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
        InventoryManager.onTakeWeapon -= ActivateWeapon;
        InventoryManager.onTakeAmmo -= TakeAmmo;
        PlayerHealth.onDeath -= Death;
        UIManager.onChooseWeapon -= ChangeWeapon;
        UIManager.onChangeWeapon -= ChangeWeapon;
        //Weapon.onChangeToAxe -= ActivaAxe;
        ObjectManager.onUseInteractibleWeapon -= UseInteractWeapon;


    }
    [ContextMenu("ActivateAxe")]
    public void ActivaAxe()
    {
        foreach(var v in weapons)
        {
            if (v.weaponModel.weaponAmmo.GetAmmoCount() > 0 || v.weaponModel.CurrentCapacity()>0)
            {
                if (v.isTaken)
                {
                    ChangeWeapon(v.weaponModel.weaponName);
                    return;
                }
            }
        }
        ChangeWeapon(WeaponName.Axe);        
    }


    private void ActivateWeapon(WeaponName weaponName, int ammo)
    {
        OnTakeItem?.Invoke();
        if (currentWeapon != null) currentWeapon.Deactivate();
        foreach(var w in weapons)
        {
            if (w.weaponModel.weaponName == weaponName)
            {
                w.weaponModel.TakeAmmo(ammo);
                onChangeWeapon?.Invoke(weaponName, w.weaponModel);
                w.Activate(w.weaponModel.weaponAmmo.GetAmmoCount());
                currentWeapon = w;
                lastWeapon = w;
                w.isTaken=true;
                
                onChange?.Invoke(weaponName);
                return;
            }
        }       
    }
      

    private void TakeAmmo(WeaponName weaponName, int ammo)
    {
        Debug.Log("WeaponManger Take ammo");
        OnTakeAmmo?.Invoke();
        foreach (var w in weapons)
        {
            if (w.weaponModel.weaponName == weaponName)
            {
                w.weaponModel.TakeAmmo(ammo);
                if (currentWeapon != null)
                {
                    if (currentWeapon.isEmpty && currentWeapon.weaponModel.weaponName == weaponName)
                    {
                        w.CheckAmmo();
                    }
                    return;
                }
            }
        }     
    }


   
    /*
    private void EmptyCurrentWeapon(Weapon weapon)
    {

        if (weapons.IndexOf(weapon) > 0&&(weapons[weapons.IndexOf(weapon)-1].weaponModel.weaponAmmo.GetAmmoCount()>0|| weapons[weapons.IndexOf(weapon) - 1].weaponModel.CurrentCapacity()>0))
        {
            Debug.Log("Activate  weapon" + (weapons.IndexOf(weapon) - 1));
            ActivateWeapon(weapons.IndexOf(weapon) - 1, 0);
        }
        else
        {
            Debug.Log("Deactivate  weapon" + (weapons.IndexOf(weapon)));
            currentWeapon.Deactivate();
            currentWeapon = null;
        }
    }*/
}
