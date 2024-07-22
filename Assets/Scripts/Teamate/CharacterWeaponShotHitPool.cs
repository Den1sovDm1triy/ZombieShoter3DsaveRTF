using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponShotHitPool : MonoBehaviour
{
    WeaponCharacter weaponCharacter;
    Character character;
    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int poolCount = 3;
    private bool autoExpand = true;
    [SerializeField] private List<ShootHitPoolElement> shootHitprefabList = new List<ShootHitPoolElement>();


    private PoolMono<ShootHitPoolElement> poolHit;





    private void Start()
    {
        weaponCharacter= GetComponent<WeaponCharacter>();
        character = GetComponentInParent<Character>();
        weaponCharacter.onShot += CreateShootHit;
        Init();
    }
    private void OnDestroy()
    {
        if(weaponCharacter.onShot!=null)
        weaponCharacter.onShot -= CreateShootHit;
    }

    private void Init()
    {
        GameObject poolHitObject = new GameObject("ShootHitPool");
        this.poolHit = new PoolMono<ShootHitPoolElement>(shootHitprefabList, this.poolCount, poolHitObject.transform);
        this.poolHit.autoExpand = this.autoExpand;

    }


    private void CreateShootHit()
    {
        Ray ray = new Ray(transform.position+Vector3.up*1.7f, (character.target.transform.position+ Vector3.up * 1f) - (transform.position + Vector3.up * 1.7f));

        RaycastHit[] hits = Physics.RaycastAll(ray, 100);
        foreach (var hit in hits)
        {          
                if (hit.collider.CompareTag("Enemy"))
                {
                    CreateBlood(hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                }           
        }       
    }

    private void CreateBlood(Vector3 pointOfHit, Quaternion rotationOfHit, Transform parent)
    {
        var shootHit = this.poolHit.GetFreeElement();
        shootHit.transform.position = pointOfHit;
        shootHit.transform.rotation = rotationOfHit;
        shootHit.transform.SetParent(parent);
    }
}
