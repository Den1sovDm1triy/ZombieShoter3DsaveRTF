using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDeath, OnTakeDamage, OnBiting, OnMoveToPlayer, OnRun, OnWalk, OnStop, OnAttack;


    public Action onDeath;
    public static Action<Enemy> onDeadZombie;
    public Action onTakeDamage;
    public Action onBiting;
    public Action onAttack;
    public Action onMoveToPlayer;  
    public Action onRun;
    public Action onWalk;
    public Action onStop;
    public Action onLoosePlayer;
    public Action onCanAttack;
    public Action onCantAttack;



    public EnemyBehaviour Pursiut, AttackBeh;
    public EnemyBehaviour currentBehaviour;
    public EnemyBehaviour previousBehaviour;
    [HideInInspector] public EnemyModel enemyModel;
    [HideInInspector] public SphereCollider sphereCollider;  

    [HideInInspector] public GameObject enemyObject;    
    [HideInInspector] public bool isDead;    
   

    private void Awake()
    {
        isDead = false;
        enemyModel = GetComponent<EnemyModel>();
        onDeath += Dead;
        onTakeDamage += UtakeDamage;
        onRun += URun;
        onAttack += UAttack;
        onStop += UStop;
        onMoveToPlayer += ChasePlayer;
        onTakeDamage += CheckState;        
        PlayerController.onAttackSound += CheckAttack;        
    }

    public void Setup(float Xhealth,float Xdamage,float angryTime)
    {
        Init();
        enemyModel.Setup(Xhealth, Xdamage);
        currentBehaviour = null;
        previousBehaviour = null;
        InitBehaivour(Pursiut);        
    }




    
    private void OnDestroy()
    {
        onDeath -= Dead;
        onTakeDamage -= UtakeDamage;
        onRun -= URun;
        onAttack -= UAttack;
        onMoveToPlayer -= ChasePlayer;
        onTakeDamage -= CheckState;        
        PlayerController.onAttackSound -= CheckAttack;
    }

    private void CheckAttack(float distance)
    {
        Vector3 playerPosition = PlayerInstance.Instance.transform.position;
        Vector3 myPosition = transform.position;

        float sqrDistanceToPlayer = (playerPosition - myPosition).sqrMagnitude;

        float sqrDistance = distance * distance; // ����������� �������� ����������

        if (sqrDistanceToPlayer < sqrDistance)
        {
            StartCoroutine(ShotAgre());
        }
        else
        {
            return;
        }
    }

    private IEnumerator ShotAgre()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, 2.5f));
        InitBehaivour(Pursiut);
    }

    

    private void CheckState()
    {
        if (currentBehaviour != Pursiut)
        {
            InitBehaivour(Pursiut);
        }
    }

    public void ChasePlayer()
    {
        InitBehaivour(Pursiut);
    }

    private void UAttack()
    {
        InitBehaivour(AttackBeh);
        OnAttack?.Invoke();
    }
    private void UtakeDamage()
    {
        OnTakeDamage?.Invoke();
    }
    private void URun()
    {
        OnRun?.Invoke();
    }

    private void UStop()
    {
        OnStop?.Invoke();
    }
   




    
        
    private void InitBehaivour(EnemyBehaviour enemyBehaviour)
    {
        if (isDead) return;
        Debug.Log("����� ���������");
        
        if (currentBehaviour != enemyBehaviour)
        {
            previousBehaviour = currentBehaviour;
            if (previousBehaviour != null)
                previousBehaviour.DeInit();
            if (enemyBehaviour != null)
                enemyBehaviour.Init();
            currentBehaviour = enemyBehaviour;
            Debug.Log(currentBehaviour);
        }

        else if (enemyBehaviour == Pursiut)
        {
            previousBehaviour = currentBehaviour;
            if (previousBehaviour != null)
                previousBehaviour.DeInit();
            if (enemyBehaviour != null)
                enemyBehaviour.Init();
            currentBehaviour = enemyBehaviour;
            Debug.Log(currentBehaviour);
        }
    }


  

    


    private void Init()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = true;
        sphereCollider.radius = enemyModel.findTargetDistance;             
    }

    private void Dead()
    {
        if (!isDead)
        {
            onDeadZombie?.Invoke(this);
            isDead = true;
        }
        if(sphereCollider!=null)
        sphereCollider.enabled = false;
        OnDeath?.Invoke();
        currentBehaviour.DeInit();
        Vector3 deathPosition = transform.position;       

    }

  


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
            InitBehaivour(Pursiut);
        }
    }

    

    private void OnTriggerStay(Collider other)
    {       
        if(other.CompareTag("Player") && !PlayerStates.isAlive&&!isDead)
        {            
            onBiting?.Invoke();
        }
    }
      

  
    public bool IsDead()
    {
        return isDead;
    }


}
