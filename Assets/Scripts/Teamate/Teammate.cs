using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Teammate : MonoBehaviour
{
    public int id;
    public static Action<Teammate, bool> onReadyInteract;

    public Action onNightNotHiredCome;
    public Action onDayNotHiredCome;
    public Action onNightHiredCome;
    public Action onDayHiredCome;
    public Action onActivateJob;
    public Action onDeactivateJob;
    private int teamMateId;
    public bool isHired;
    public TeamMateData mateData;
    public InteractibleTeammate interactibleTeammate;
    public int i;

    private void Awake()
    {
        interactibleTeammate.onEnter += ReadyToInteract;
        interactibleTeammate.onExit += StopInteract; 
        interactibleTeammate.FiredRadius();

    }

    private void OnDestroy()
    {
        interactibleTeammate.onEnter -= ReadyToInteract;
        interactibleTeammate.onExit -= StopInteract;
    }

    

    private void StopInteract()
    {
        onReadyInteract?.Invoke(this, false);
    }


    private void ReadyToInteract()
    {
        onReadyInteract?.Invoke(this, true);
    }


    public void Recuit()
    {
        isHired = true;
        onActivateJob?.Invoke();
        interactibleTeammate.RecuitRadius();

    }

    public void Fired()
    {
        isHired = false;
        onDeactivateJob?.Invoke();
        interactibleTeammate.FiredRadius();
    }


    public void NightWhenNotHired()
    {
        interactibleTeammate.FiredRadius();
        onNightNotHiredCome?.Invoke();       
    }
    public void DayWhenNotHired()
    {
        interactibleTeammate.FiredRadius();
        onDayNotHiredCome?.Invoke();
    }

    public void NightWhenHired()
    {
        interactibleTeammate.RecuitRadius();
        onNightHiredCome?.Invoke();
    }
    public void DayWhenHired()
    {
        interactibleTeammate.RecuitRadius();
        onDayHiredCome?.Invoke();
    }
}

public enum TypeTeammate
{
    bodyGuard, farmer,
}

