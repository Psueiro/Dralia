using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConditionReach : ICondition
{
    Vector3 goal;
    Stats stat;
    float threshold;
    ControllerAI cont;

    public AIConditionReach(Vector3 v, Stats s, float thres, ControllerAI c)
    {
        goal = v;
        stat = s;
        threshold = thres;
        cont = c;
    }

    public void CheckCondition()
    {
        if (stat == null) return;
        float dis = Vector3.Distance(stat.transform.position, goal);
        if (dis < threshold)
        {
            RemoveMe();
        }
    }

    public void RemoveMe()
    {
        cont.active.Remove(stat);
        cont.activeReference.Remove(stat);
    }
}
