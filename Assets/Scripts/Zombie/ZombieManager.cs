using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ZombieManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip timeClip;
    [SerializeField] private AudioClip startWaveClip; 

   



    public static Action<int> onTimerBeforeStart;
    public static Action onWaveIsDestroyd;
    public static Action<int>onStartWave;
    public static Action<int> onEnemyLeft;

    public static Action onBossStart;


    [SerializeField] private List<GameObject> spawnPoint;
    private List<Enemy> enemyes = new List<Enemy>();
    [SerializeField] private List<Enemy> enemyPrefab;
    [SerializeField] private List<Enemy> bossPrefab;
    [SerializeField] private int countOnField=25;
    [SerializeField] private int fullcount;
    [SerializeField] private float Xhealth, Xdamage;
    [SerializeField] private float minTime, maxTime;
    [SerializeField] private float firstAngrytime=100;
    int currentcount;
    private int curnumberOfWave;
    private bool isHasBoss;
    private int countofBoos;
    public static int curNumberWave, curNumberDeadZombie;
    public GameObject bossMark;

    [ContextMenu("Agry")]
    
    private void Agry()
    {
        foreach(var e in enemyes)
        {
            e.ChasePlayer();
        }
    }

    private void Start()
    {
        Enemy.onDeadZombie += DeadEnemy; 
        curNumberWave=1;
        StartCoroutine(Starter(5));
        
    }

    private IEnumerator Starter(int timesecond)
    {
        audioSource.clip = timeClip;
        audioSource.Play();
        while(timesecond>=0)
        {
            onTimerBeforeStart?.Invoke(timesecond);
            yield return new WaitForSeconds(1);
            timesecond--;
        }        
        StartWave(curNumberWave);
    }

    private void OnDestroy()
    {       
        Enemy.onDeadZombie -= DeadEnemy;
    }

    

    private void StartWave(int numberOfWave)
    {     
        audioSource.Stop();
        audioSource.clip = startWaveClip;
        audioSource.Play(); 

        onStartWave?.Invoke(curNumberWave);
        curNumberWave = numberOfWave;      
        curnumberOfWave = numberOfWave;        
        currentcount = 0;        
        fullcount = numberOfWave*10;  
        onEnemyLeft?.Invoke(fullcount);  
        curNumberDeadZombie=fullcount;
        curNumberWave = numberOfWave;   
       
        if(curNumberWave<3) isHasBoss=false;   
        else isHasBoss = true;    
        if(isHasBoss)
        {
            if(curNumberWave<6)
            countofBoos = 1;
            else
            countofBoos = UnityEngine.Random.Range(1,4);
        }
        StartCoroutine(SpawnCor());
    }


    private IEnumerator SpawnCor()
    {
        while (currentcount<fullcount)
        {
            if (currentcount< fullcount&&enemyes.Count<countOnField)
            {
                currentcount++;
                Enemy curenemy=null;

                Vector3 pointOfSpawn;              
                
                pointOfSpawn = spawnPoint[UnityEngine.Random.Range(0, spawnPoint.Count)].transform.position;                

                if (curNumberWave < 4)
                {
                   curenemy = Instantiate(enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Count - 1)], pointOfSpawn, Quaternion.identity, null);
                }
                else
                {
                   curenemy = Instantiate(enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Count)], pointOfSpawn, Quaternion.identity, null);
                }
                curenemy.Setup(1 ,1 , firstAngrytime);
                enemyes.Add(curenemy);               
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTime, maxTime));
        }
        WaveReady();
    }

    private void WaveReady()
    {
        StartCoroutine(WaveCor());
    }

    private IEnumerator WaveCor()
    {
        while (enemyes.Count > 0)
        {
            yield return new WaitForSeconds(2f);
        }
        if (!isHasBoss)
        {
            onWaveIsDestroyd?.Invoke();           
            yield return new WaitForSeconds(1f);
            curNumberWave++;
            StartCoroutine(Starter(10));
        }
        else
        {
            if (countofBoos == 1)
            {                              
                yield return new WaitForSeconds(5f);
                onBossStart?.Invoke();  
                Vector3 pointOfSpawn;      
                bossMark.SetActive(true);          
                pointOfSpawn = spawnPoint[UnityEngine.Random.Range(0, spawnPoint.Count)].transform.position;               
                Enemy boss = Instantiate(bossPrefab[UnityEngine.Random.Range(0, bossPrefab.Count)], pointOfSpawn, Quaternion.identity, null);                
                boss.Setup(1, 1, 180);               
                enemyes.Add(boss);
            }
            else
            {
                for (int i = 0; i < countofBoos; i++)
                {
                    int bossIndex = i % bossPrefab.Count;
                    yield return new WaitForSeconds(5f);
                    onBossStart?.Invoke();
                    bossMark.SetActive(true); 
                    Enemy boss = Instantiate(bossPrefab[bossIndex], spawnPoint[UnityEngine.Random.Range(0, spawnPoint.Count)].transform.position, Quaternion.identity, null);
                    boss.Setup(1, 1, 180);
                    enemyes.Add(boss);
                }
            }
            StartCoroutine(WaveBossCor());
        }
    }

    IEnumerator WaveBossCor()
    {
        while (enemyes.Count > 0)
        {
            yield return new WaitForSeconds(2f);
        }
        onWaveIsDestroyd?.Invoke();
        yield return new WaitForSeconds(1f);
        curNumberWave++;
        StartCoroutine(Starter(10));
    }


    private void DeadEnemy(Enemy enemy)
    {
        curNumberDeadZombie--;
        onEnemyLeft?.Invoke(curNumberDeadZombie); 
        enemyes.Remove(enemy);
        enemy.gameObject.AddComponent<HiderDestrier>();
    }     
}
