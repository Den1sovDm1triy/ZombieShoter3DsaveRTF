using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Character : MonoBehaviour
{
    public Action onAfterWait;
    public NavMeshAgent navMeshAgent;
    public  Health health;
    public Animator anim;
    public  List<Transform> shopwayPoint=new List<Transform>();
    public Transform exitPoint, exitPoint2, enterPoint;
    
    

    public  AudioSource audio;
    public AudioClip okClip, firedClip, idlewalkclip, idlestayclip;
    

    public State currentState;
    public State lastState;



    public State IdleShopState;
    public State LeftZoneAtNightState;
    public State BackToZoneAtDay;
    public State ReadyToFight;
    public State AttackState;
    public State ReadyToCollect;
    public State CollectVegetables;

    public Teammate teammate;

    public  Enemy target;
    public VegetablesItem item;
    CharacterAttackRadius characterAttackRadius;
    CharacterCollectRadar characterCollectRadar;
    private bool damageToTargetSubscribed = false;


    private void Awake()
    {
        audio = GetComponent<AudioSource>();      
        health.Init(teammate.mateData.health);
        teammate.onActivateJob += Activate;
        teammate.onDeactivateJob += Deactivate;
        teammate.onNightNotHiredCome += ExitFromZone;
        teammate.onDayNotHiredCome += EnterToZone;
        teammate.onDayHiredCome += Activate;
    }

    public void ActivateWeapon()
    {      
        WeaponCharacter weaponCharacter = GetComponentInChildren<WeaponCharacter>();
        weaponCharacter.Activate();
        if (!damageToTargetSubscribed)
        {
            damageToTargetSubscribed = true;
            weaponCharacter.onShot += DamageToTarget;
        }
        
    }

    private void DamageToTarget()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<EnemyHealth>().TakeDamage(teammate.mateData.damage);              
        }
    }

    public void DeactivateWeapon()
    {
        WeaponCharacter weaponCharacter = GetComponentInChildren<WeaponCharacter>();
        weaponCharacter.Deactivate();
        if (damageToTargetSubscribed)
        {
            weaponCharacter.onShot -= DamageToTarget;
            damageToTargetSubscribed = false;
        }
    }
    private void OnDestroy()
    {
        teammate.onActivateJob -= Activate;
        teammate.onDeactivateJob -= Deactivate;
        teammate.onDayNotHiredCome -= EnterToZone;
        teammate.onDayHiredCome -= Activate;
        
    }

   

    private void ExitFromZone()
    {
        SetState(LeftZoneAtNightState);
    }

    private void EnterToZone()
    {
        SetState(BackToZoneAtDay);
    }

    private void Activate()
    {
        PlayAudio(okClip, false, 0.5f);
        if (teammate.mateData.typeTeammate == TypeTeammate.bodyGuard)
        {
            SetState(ReadyToFight);
            characterAttackRadius = GetComponentInChildren<CharacterAttackRadius>();
            characterAttackRadius.Activate();
            characterAttackRadius.onAttack += Attack;
            characterAttackRadius.onRelax += Relax;
        }
        else if(teammate.mateData.typeTeammate == TypeTeammate.farmer)
        {
            SetState(ReadyToCollect);            
            characterCollectRadar = GetComponentInChildren<CharacterCollectRadar>();
            CharacterInventory characterInventory = GetComponentInChildren<CharacterInventory>();
            characterInventory.isWorking = true;
            characterCollectRadar.Activate();
            characterCollectRadar.onFindVegetables += GoToVegetable;
            characterCollectRadar.onRelax += Relax;
            
        }
    }
    private void Relax()
    {
        if (teammate.mateData.typeTeammate == TypeTeammate.bodyGuard)
        {       
            if (currentState as AttackState)
                SetState(ReadyToFight); 
        }
        else if(teammate.mateData.typeTeammate == TypeTeammate.farmer)
        {
            SetState(ReadyToCollect);
        }
       
    }

    private void GoToVegetable(VegetablesItem vegetablesItem) 
    {
        item = vegetablesItem;
        SetState(CollectVegetables);
    }


    private void Attack(Enemy enemy)
    {
            target = enemy;
            SetState(AttackState);        
    }

    private void Deactivate()
    {
        Debug.Log("fired");
        PlayAudio(firedClip, false, 0.5f);
        SetState(LeftZoneAtNightState);
        if (teammate.mateData.typeTeammate == TypeTeammate.farmer)
        {
            CharacterInventory characterInventory = GetComponentInChildren<CharacterInventory>();
            if(characterCollectRadar==null) characterCollectRadar = GetComponentInChildren<CharacterCollectRadar>();
            characterInventory.isWorking = false;

            characterCollectRadar.Deactivate();
            if (characterCollectRadar.onFindVegetables != null)
                characterCollectRadar.onFindVegetables -= GoToVegetable;
            if (characterCollectRadar.onRelax != null)
                characterCollectRadar.onRelax -= Relax;
        }
        else if (teammate.mateData.typeTeammate == TypeTeammate.bodyGuard)
        {
            DeactivateWeapon();
            characterAttackRadius = GetComponentInChildren<CharacterAttackRadius>();
            characterAttackRadius.Deactivate();
            if(characterAttackRadius.onAttack!=null)
                characterAttackRadius.onAttack -= Attack;
            if (characterAttackRadius.onRelax != null)
                characterAttackRadius.onRelax -= Relax;
        }
    }

    public void FindAllVeg()
    {       
        characterCollectRadar.Stop();       
        SetState(ReadyToCollect);
    }




    public void PlayAudio(AudioClip clip, bool islooping, float volume)
    {
        audio.Stop();
        audio.loop = islooping;
        audio.volume = volume;
        audio.clip = clip;
        audio.Play();
    }

    public void PlayIdleWalkAudio()
    {
        PlayAudio(idlewalkclip, true, 0.3f);
    }
    public void PlayIdleWStayAudio()
    {
        PlayAudio(idlestayclip, true, 0.3f);
    }

    public void InitBeforeHire()
    {
        SetState(IdleShopState);
    }

    public IEnumerator Waiter(int x, int y)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(x, y));
        onAfterWait?.Invoke();
        Debug.Log("111111111111111111111111111");
    }

    public void SetState(State state)
    {
        if ((lastState as LeftZoneAtNightState) && state == LeftZoneAtNightState) return;

        lastState = currentState;
        if (lastState != null)
            lastState.DeInit();
        currentState = state;
        currentState = Instantiate(state);
        currentState.character = this;
        if(state == IdleShopState)
        {
            if(teammate.mateData.typeTeammate==TypeTeammate.bodyGuard)
            DeactivateWeapon();
            ShopWayPoint [] points = FindObjectsOfType<ShopWayPoint>();
            foreach(var v in points)
            {
                shopwayPoint.Add(v.transform);
            }
        }

       

        
       
        currentState.Init();      

    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Run();
        }
    }
}
