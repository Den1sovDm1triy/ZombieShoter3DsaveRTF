using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverrider : MonoBehaviour
{
    private Animator anim;       
    private const string nameStateIdle = "Die";  
    [SerializeField] private List<AnimationClip> deathClips;
    private Enemy enemy;
    private bool isfirst=true;  
    
    void Start()
    {       
        enemy = GetComponent<Enemy>();       
        enemy.onDeath += OvverideDeath;             
        anim = GetComponentInChildren<Animator>();        
            
    }

    private void OnDestroy()
    {
     
        enemy.onAttack -= OvverideDeath;
    }

    public void OvverideDeath()
    {
        if (isfirst)
        {
            isfirst = false;
            OverrideAnimationClip(deathClips[Random.Range(0, deathClips.Count)], nameStateIdle);
        }
    }






    public RuntimeAnimatorController GetEffectiveController(Animator animator)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        AnimatorOverrideController overrideController = controller as AnimatorOverrideController;
        while (overrideController != null)
        {
            controller = overrideController.runtimeAnimatorController;
            overrideController = controller as AnimatorOverrideController;
        }
        return controller;
    }
    public void OverrideAnimationClip(AnimationClip clip, string nameState)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = GetEffectiveController(anim);
        overrideController[nameState] = clip;
        anim.runtimeAnimatorController = overrideController;        
    }
}
