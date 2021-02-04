using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConditionFinishSpawning : ICondition
{
    Spawners spawner;
    ControllerAI cont;

    public AIConditionFinishSpawning(Spawners s, ControllerAI c)
    {
        spawner = s;
        cont = c;
    }

    public void CheckCondition()
    {
        if (!spawner.spawning)
        {
            RemoveMe();
        }
    }

    public void RemoveMe()
    {
        cont.active.Remove(spawner);
        cont.activeReference.Remove(spawner);
    }
}
