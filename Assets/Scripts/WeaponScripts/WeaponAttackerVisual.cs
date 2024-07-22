using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackerVisual : MonoBehaviour
{
    WeaponAttacker weaponAttacker;
    [SerializeField] TrailRenderer bulletTrail;

    private void Awake()
    {
        weaponAttacker = GetComponent<WeaponAttacker>();
        /*weaponAttacker.onVisualShoot += VisualShot;*/
    }
    private void OnDestroy()
    {
       /* weaponAttacker.onVisualShoot -= VisualShot;*/
    }

   /* private void VisualShot(Vector3 start, Vector3 end)
    {
        var bullet = Instantiate(bulletTrail, start, Quaternion.identity, null);
        bullet.AddPosition(start);
        bullet.transform.position = end;        
    }*/

    

}
