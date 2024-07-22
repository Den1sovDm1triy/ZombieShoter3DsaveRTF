using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class PlayerHealth : MonoBehaviour
{
    public static Action<int> onSetDamage;
    [SerializeField] UnityEvent OnTakeDamage;
    [SerializeField] UnityEvent OnDead;
    public static Action <float> onHeal;
    public static Action onDeath;
    public static Action onTakeDamage;
    public static Action<float> onTakeIntDamage;
    public static Action<float> onBarUpdate;
    public static Action onBarShake;
    public static Action<float> onBarCalibrate;

    private float heath;
    [HideInInspector] public float startheath;
    private PlayerModel playerModel;
    [SerializeField]
    private  float speedRepair;

    public static PlayerHealth Instance;

    void Start()
    {
       if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        playerModel = GetComponent<PlayerModel>();        
        startheath = playerModel.startHealth;
        onBarCalibrate?.Invoke(startheath);
        heath = startheath;       
        EnemyAttack.onPlayerTakeDamage += TakeDamage;
        onSetDamage += TakeDamage;
        onHeal += Heal;
        PlayerUpgradeManager.onHealthUpUpgrade+=HealthUpgrade;
        WeaponAttacker.onGiveDamage+=Vampire;
    }

   

    private void OnDestroy()
    {       
        EnemyAttack.onPlayerTakeDamage -= TakeDamage;
        onSetDamage -= TakeDamage;
        onHeal -= Heal;
         PlayerUpgradeManager.onHealthUpUpgrade-=HealthUpgrade;
         WeaponAttacker.onGiveDamage-=Vampire;
    }
    

    private void Vampire(int damage)
    {
        if (heath > 0)
        {
            heath += PlayerUpgradeManager.Instance.vampireX/100* damage;
            heath = Mathf.Clamp(heath, 0, startheath);
            onBarUpdate?.Invoke(heath);
        }
    }

    private void HealthUpgrade(int healthPlus)
    {
       startheath = playerModel.startHealth+healthPlus;
       onBarCalibrate?.Invoke(startheath);
       heath = startheath;      
    }

    private void Heal(float hp)
    {
        if (heath <= 0) return;
        if (heath < startheath)
        {
            heath += hp;
            if (heath > startheath)
            {
                heath = startheath;
            }
        }
        onBarUpdate?.Invoke(heath);
    }

    public  bool IsDead(){
        if(heath>0) return false;
        else return true;        
    }


    private IEnumerator HealthRepair()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            if (heath < startheath)
            {
                heath += PlayerUpgradeManager.Instance.healPoints;
                onBarShake?.Invoke();
            }
            else heath = startheath;
            onBarUpdate?.Invoke(heath);
            yield return new WaitForSeconds(3f);        
        }

    }


    private void TakeDamage(int damage)
    {
        Debug.Log(damage);
        heath -= damage;
        heath = Mathf.Clamp(heath, 0, playerModel.startHealth);
        onBarUpdate?.Invoke(heath);
        if (heath <= 0)
        {
            StopAllCoroutines();
            Debug.Log("����� ����");
            onTakeIntDamage?.Invoke(heath);
            onDeath?.Invoke();
            OnTakeDamage?.Invoke();
            OnDead?.Invoke();
            heath= 0;
        }
        else
        {
            StopAllCoroutines();            
            StartCoroutine(HealthRepair());
            Debug.Log("TakeeDamage");
            onTakeDamage?.Invoke();
            onTakeIntDamage?.Invoke(heath);
            OnTakeDamage?.Invoke();
        }
    }

   
}
