using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AnimatorManager 
{
    public List<string> allAnimationNames;

    public AnimatorManager()
    {
        allAnimationNames = new List<string>();
        allAnimationNames.Add("Moving");
        allAnimationNames.Add("Attacking");
    }

   public void AnimationChange(Stats unit, string animName)
    {
        if (unit == null) return;
        Animator anim;
        if (unit.GetComponent<Animator>()) anim = unit.GetComponent<Animator>();
        else anim = unit.GetComponentInChildren<Animator>();

        if(anim)
        {
            foreach (AnimatorControllerParameter parameter in anim.parameters)
            {
                anim.SetBool(parameter.name, false);
            }
            anim.SetBool(animName, true);

        }
    }

    public void DisableAllAnims(Stats unit)
    {
        Animator anim;
        if (unit.GetComponent<Animator>()) anim = unit.GetComponent<Animator>();
        else anim = unit.GetComponentInChildren<Animator>();

        if(anim)
        {
            foreach (AnimatorControllerParameter parameter in anim.parameters)
            {
                anim.SetBool(parameter.name, false);
            }
        }
    }
}
