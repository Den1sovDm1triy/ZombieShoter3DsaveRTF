using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TeamMate", menuName = "TeamMate", order = 2)]
public class TeamMateData : ScriptableObject
{
    public string employeeName;
    public int price;
    public int salaryPerDay;
    public Sprite teammateSprite;
    public TypeTeammate typeTeammate;
    public string description;
    public string descriptionWeapon;
    public int rating;
    public float moveSpeed;
    public float runSpeed;
    public int health;
    public int damage;

    public int dayUnblock;
    

}
