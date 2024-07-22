using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WeaponAttacker : MonoBehaviour
{
    public Action<Vector3, Vector3> onVisualShoot;
    public static Action <int> onGiveDamage;
    public Action<Vector3, Quaternion, Transform> onVisualHit;
    public static Action onHeadShot;

    public static Action onHitBodyMelee, onHitHeadMelee, onHitBodyRange, onHitHeadRange;


   
    public Weapon weapon;

    [Tooltip("Количество линий полетв пули  в выстреле - 1 для ружей и пистолетов, 5 и более для бробовиков")]
    [SerializeField] int shootLine; 

    private Transform pointofShoot;    


    private void Start()
    {       
        pointofShoot = GetComponentInChildren<PointOfShoot>().transform;        
        weapon.onWeaponShoot += Shoot;
    }
    private void OnDestroy()
    {
        if(weapon.onWeaponShoot!=null)
        weapon.onWeaponShoot -= Shoot;
    }

   


    

    private void Shoot(int capasity)
    {
        if (weapon.weaponModel.weaponType == WeaponType.range)
        {
            for (int i = 0; i < shootLine; i++)
            {

                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Ray ray = Camera.main.ScreenPointToRay(screenCenter);

                float randomOffsetX = UnityEngine.Random.Range(-i * 0.01f, i * 0.01f);
                float randomOffsetY = UnityEngine.Random.Range(-i * 0.01f, i * 0.01f);

                Vector3 randomOffset = Camera.main.transform.right * randomOffsetX + Camera.main.transform.up * randomOffsetY;
                Vector3 rayDirection = (ray.direction + randomOffset).normalized;



                RaycastHit[] hits = Physics.RaycastAll(ray, weapon.Distance());

                foreach (var hit in hits)
                {
                    if (hit.collider.CompareTag("EnemyHead"))
                    {
                        hit.transform.GetComponentInParent<EnemyHealth>().TakeDamage(100);
                        onGiveDamage?.Invoke(weapon.Damage()*5);
                        onVisualHit?.Invoke(hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                        onHeadShot?.Invoke();
                        onHitHeadRange?.Invoke();
                    }
                    else if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponentInParent<EnemyHealth>().TakeDamage(weapon.Damage());
                        onGiveDamage?.Invoke(weapon.Damage());
                        onVisualHit?.Invoke(hit.point, Quaternion.LookRotation(hit.normal), hit.transform);      
                        onHitBodyRange?.Invoke();                  
                    }
                    else
                    {
                        onVisualShoot?.Invoke(pointofShoot.position, hit.point);
                    }
                }
            }
        }
        else
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 3f))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("урон");
                    EnemyHealth curEnemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
                    curEnemyHealth.TakeDamage(weapon.Damage());
                    onGiveDamage?.Invoke(weapon.Damage());
                    /*if (hit.collider.CompareTag("EnemyHead")) // Проверяем, попал ли луч в голову
                    {
                        curEnemyHealth.TakeDamage(weapon.Damage()*3);
                        onHeadShot?.Invoke();// Применяем увеличенный урон за хедшот
                    }
                    else
                    {
                        curEnemyHealth.TakeDamage(weapon.Damage()); // Обычный урон
                    }*/
                    onVisualShoot?.Invoke(pointofShoot.position, hit.point);
                    onVisualHit?.Invoke(hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                    onHitBodyMelee?.Invoke();
                }
                else if (hit.collider.CompareTag("EnemyHead"))
                {
                    Debug.Log("урон");
                    EnemyHealth curEnemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
                    curEnemyHealth.TakeDamage(100);
                    onGiveDamage?.Invoke(weapon.Damage()*5);
                    /*if (hit.collider.CompareTag("EnemyHead")) // Проверяем, попал ли луч в голову
                    {
                        curEnemyHealth.TakeDamage(weapon.Damage() * 3); // Применяем увеличенный урон за хедшот
                    }
                    else
                    {
                        curEnemyHealth.TakeDamage(weapon.Damage()); // Обычный урон
                    }*/
                    onVisualShoot?.Invoke(pointofShoot.position, hit.point);
                    onVisualHit?.Invoke(hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                    onHitHeadMelee?.Invoke();
                }
            }



            /*Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 3f))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("урон");
                    EnemyHealth curEnemyHealth = hit.transform.GetComponentInParent<EnemyHealth>();
                    curEnemyHealth.TakeDamage(weapon.Damage());
                    onVisualShoot?.Invoke(pointofShoot.position, hit.point);
                    onVisualHit?.Invoke(hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                }
            }*/
        }

    }



}
