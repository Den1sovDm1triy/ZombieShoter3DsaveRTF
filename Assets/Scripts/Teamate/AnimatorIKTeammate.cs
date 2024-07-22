using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AnimatorIKTeammate : MonoBehaviour
{
    public Action onShot;

    [SerializeField] InteractibleTeammate interactible;

    [SerializeField] private Animator animator;
    public Transform player;
    [SerializeField] float lookWieght;
    float curLookWieght;
    bool isNeedLook = false;
    private void Start()
    {
        player = PlayerInstance.Instance.transform;
        interactible.onEnter += NeedLook;
        interactible.onExit += StopLook;
    }

    private void OnDestroy()
    {
        interactible.onEnter -= NeedLook;
        interactible.onExit -= StopLook;
    }

    private void NeedLook()
    {
        StopAllCoroutines();
        StartCoroutine(StartLook());

    }

    private void StopLook()
    {
        StopAllCoroutines();
        StartCoroutine(HideLook());
    }

    private IEnumerator HideLook()
    {
        while (curLookWieght > 0)
        {
            curLookWieght -= 0.01f;
            yield return null;
        }
        isNeedLook = false;
    }
    private IEnumerator StartLook()
    {
        isNeedLook = true;
        while (curLookWieght < lookWieght)
        {
            curLookWieght += 0.05f;
            yield return null;
        }        
    }


    private void OnAnimatorIK()
    {
        if (isNeedLook&&player!=null)
        {
            animator.SetLookAtWeight(curLookWieght, 0.3f, 0.8f, 0.3f);
            animator.SetLookAtPosition(player.position + Vector3.up * 1.5f);
        }
    }


    public void Shot()
    {
        onShot?.Invoke();
    }
}
