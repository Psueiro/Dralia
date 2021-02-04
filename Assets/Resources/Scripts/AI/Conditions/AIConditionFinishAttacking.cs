using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConditionFinishAttacking : ICondition
{
    Stats at;
    Stats ta;
    ControllerAI ai;

    public AIConditionFinishAttacking(Stats attacker, Stats target, ControllerAI controller)
    {
        at = attacker;
        ta = target;
        ai = controller;
    }

    public void CheckCondition()
    {
        if (ta == null) RemoveMe();
    }

    public void RemoveMe()
    {
        ai.active.Remove(at);
        ai.activeReference.Remove(at);
    }
}
