using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="WeaponData", menuName = "WeaponData", order =1)]
public class WeaponData : ScriptableObject
{
    public int damage;
    public float shootDelay;
    public float reloadDelay;
    public int capacityMagazine;
    public float shootDistance;
    public AudioClip reloadClip, shootClip, emptyClip, activateClip;
    public WeaponType weaponType;
    public WeaponName weaponName;
    public int ammoAtStart;
}

public enum WeaponType
{
    melee, range,
}
