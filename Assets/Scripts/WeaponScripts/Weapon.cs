using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class Weapon : MonoBehaviour
{
    public static Action onChangeToAxe;
    public static Action <WeaponName, int> onEmptyRequest;
    public static Action onShoot;
    public Action <int> onWeaponShoot; 
    public Action onWeaponReload;
    public Action onWeaponChange;
    public Action onEmptyWeapon;
    public UnityEvent OnReadyWeapon;
    public UnityEvent OnDeactivate;
    public StateWeapon stateWeapon;
   
    [HideInInspector] public WeaponModel weaponModel;    
    [SerializeField] GameObject weaponGameObject;

    Rigidbody rigid;
    BoxCollider weaponCollider;

    //private int currentCapasityOfMagazine;


    private bool firstempty;
    private Coroutine ShootCoroutine;
    private Coroutine ReloadCoroutine;
    [SerializeField]private bool isCanShoot;
    public bool isEmpty=false;
    public bool isTaken;    
    private void Start()
    {
        Init();
    }

    public void Init()
    {        
        weaponModel = GetComponent<WeaponModel>();
        rigid = GetComponentInChildren<Rigidbody>();
        weaponCollider = GetComponentInChildren<BoxCollider>();
        rigid.isKinematic = true;
        rigid.useGravity = false;
        weaponCollider.enabled = false;
        isCanShoot = false;
        PlayerController.onAttack += Shoot;        
        firstempty = true;
        isTaken = false;
    }

    public void Activate(int ammo)
    {
        //TakeAmmo(ammo);
        weaponModel.Init();      
        Invoke("Active", 0.2f);
        OnReadyWeapon?.Invoke();
    }


    private void Active()
    {
        stateWeapon = StateWeapon.ActiveState;
        weaponGameObject.transform.position = weaponGameObject.transform.position + weaponGameObject.transform.up*2;
        onEmptyRequest?.Invoke(weaponModel.weaponName, weaponModel.weaponAmmo.GetAmmoCount());
        if (weaponModel.CurrentCapacity() > 0||weaponModel.weaponType==WeaponType.melee) isCanShoot = true;
    }

    public void Deactivate()
    {
        OnDeactivate?.Invoke();
        onWeaponChange?.Invoke();        
        //weaponGameObject.SetActive(false);
        weaponGameObject.transform.position = weaponGameObject.transform.position - weaponGameObject.transform.up*2;
        StopAllCoroutines();
        ShootCoroutine = null;
        ReloadCoroutine = null;
        stateWeapon = StateWeapon.DeActiveState;
        firstempty = true;
    }


    private void OnDestroy()
    {
        PlayerController.onAttack -= Shoot;        
    }

    public void LooseWeapon()
    {
        switch (stateWeapon)
        {
            case StateWeapon.ActiveState:
                PlayerController.onAttack -= Shoot;
                weaponGameObject.transform.SetParent(null);
                rigid.isKinematic = false;
                rigid.useGravity = true;
                weaponCollider.enabled = true;
                rigid.AddForce(Vector3.up, ForceMode.Impulse);
                rigid.AddTorque(new Vector3(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-20, 20)), ForceMode.Impulse);
                break;
            case StateWeapon.DeActiveState:
                break;
        }        
    }

    private IEnumerator IsEmptyRepeat()
    {
        firstempty = false;
        yield return new WaitForSeconds(1f);        
        firstempty = true;
    }



    public void CheckAmmo()
    {
        if (weaponModel.weaponAmmo.GetAmmoCount() > 0)
            ReloadDelay();
        onEmptyRequest?.Invoke(weaponModel.weaponName, weaponModel.weaponAmmo.GetAmmoCount());
    }

    private void Shoot()
    {
        switch (stateWeapon)
        {
            case StateWeapon.ActiveState:
                if (weaponModel.weaponType == WeaponType.range)
                {
                    if (weaponModel.CurrentCapacity() > 0)
                    {
                        if (isCanShoot)
                        {
                            onShoot?.Invoke();
                            weaponModel.Shoot();
                            onWeaponShoot?.Invoke(weaponModel.CurrentCapacity());
                            ShootDelay(weaponModel.CurrentCapacity());
                            onEmptyRequest?.Invoke(weaponModel.weaponName, weaponModel.weaponAmmo.GetAmmoCount());
                        }
                    }
                    else
                    {
                        if (weaponModel.weaponAmmo.GetAmmoCount() > 0)
                            ReloadDelay();
                        else
                        {
                            isEmpty = true;
                            if (firstempty&&weaponModel.weaponName!=WeaponName.Axe)
                            {    
                                onEmptyWeapon?.Invoke();
                                StartCoroutine(IsEmptyRepeat());
                            }
                            else
                            {                                
                                onChangeToAxe?.Invoke();
                            }
                            //stateWeapon = StateWeapon.DeActiveState;
                        }
                    }
                }
                else if(weaponModel.weaponType == WeaponType.melee||weaponModel.weaponName==WeaponName.Axe)
                {
                    if (isCanShoot)
                    {
                        onWeaponShoot?.Invoke(weaponModel.CurrentCapacity());
                        ShootDelay(weaponModel.CurrentCapacity());
                    }
                }
                else if(weaponModel.weaponType == WeaponType.melee||weaponModel.weaponName==WeaponName.ChainSaw){
                    return;
                }
                break;
            case StateWeapon.DeActiveState:
                break;
        } 
    }

    private void ShootDelay(int currentCapasity)
    {
        isCanShoot = false;
        if (currentCapasity > 0)       
            //if(ShootCoroutine==null)
            StopAllCoroutines();
            ShootCoroutine = StartCoroutine(ShootDelayCor());
           
    }

    private void ReloadDelay()
    {
        Debug.Log("ПЕРЕЗАРЯДКА");
        if (ReloadCoroutine == null)
        {
            onWeaponReload?.Invoke();
            ReloadCoroutine = StartCoroutine(ReloadDelayCor());
        }
    }



    IEnumerator ShootDelayCor()
    {
        yield return new WaitForSeconds(weaponModel.DelayOfShoot());
        isCanShoot = true;
        StopCoroutine(ShootCoroutine);

        if (weaponModel.CurrentCapacity() <= 0)
        {
            if (weaponModel.weaponAmmo.GetAmmoCount() > 0)
                ReloadDelay();
            else
            {
                isEmpty = true;
                if (firstempty)
                {
                    onEmptyWeapon?.Invoke();
                    firstempty = false;
                }
                //stateWeapon = StateWeapon.DeActiveState;
            }
        }
        
    }

    IEnumerator ReloadDelayCor()
    {       
        yield return new WaitForSeconds(weaponModel.DelayOfReload());
        isCanShoot = true;
        weaponModel.ReloadCapasity();
        ReloadCoroutine = null;
        isEmpty = false;
        
    }

    public int Damage()
    {
        return weaponModel.GetDamage();
    }

    public float Distance()
    {
        return weaponModel.GetShootDistance();
    }

    public void TakeAmmo(int _ammo)
    {
        weaponModel.TakeAmmo(_ammo);        
    }
}
