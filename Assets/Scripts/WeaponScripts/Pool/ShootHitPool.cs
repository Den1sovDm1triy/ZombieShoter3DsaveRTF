using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHitPool : MonoBehaviour
{
    WeaponAttacker weaponAttacker;
    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int poolCount = 3;
    private bool autoExpand = true;
    [SerializeField] private List<ShootHitPoolElement> shootHitprefabList = new List<ShootHitPoolElement>();


    private PoolMono<ShootHitPoolElement> poolHit;





    private void Start()
    {
        weaponAttacker = GetComponent<WeaponAttacker>();
        weaponAttacker.onVisualHit += CreateShootHit;
        Init();
    }
    private void OnDestroy()
    {
        if (weaponAttacker == null) return;
        if (weaponAttacker.onVisualHit != null)
        {

            weaponAttacker.onVisualHit -= CreateShootHit;
        }
    }

    private void Init()
    {
        GameObject poolHitObject = new GameObject("ShootHitPool");
        this.poolHit = new PoolMono<ShootHitPoolElement>(shootHitprefabList, this.poolCount, poolHitObject.transform);
        this.poolHit.autoExpand = this.autoExpand;



    }


    private void CreateShootHit(Vector3 pointOfHit, Quaternion rotationOfHit, Transform parent)
    {
        var shootHit = this.poolHit.GetFreeElement();
        shootHit.transform.position = pointOfHit;
        shootHit.transform.rotation = rotationOfHit;
        shootHit.transform.SetParent(parent);
    }



}
