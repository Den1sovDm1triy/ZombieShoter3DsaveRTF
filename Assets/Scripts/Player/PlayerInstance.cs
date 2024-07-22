
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInstance : Singleton<PlayerInstance>
{
    [SerializeField] public Transform pointOfSearch;
}
