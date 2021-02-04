using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProtocolTownCenter<T> : IProtocol
{
    ControllerAI ai;
    Server ser;
    int workerCount;

    public IProtocol SetSources(ControllerAI a, Server s)
    {
        ai = a;
        ser = s;
        return this;
    }

    public void SetTasks(Stats s)
    {
        CivContextChecker();
        Spawners w = s as Spawners;
        if(workerCount < 5 && w.queue.Count < 1)
        {
            CallAction(w);
            ai.active.Add(s);
        }
    }

    void CivContextChecker()
    {
        workerCount = 0;

        for (int i = 0; i < ser.playerRealUnitsLists[ai.body.id].Count; i++)
        {
            if (ser.playerUnitsLists[ai.body.id][i] is T) workerCount++;
        }
    }

    void CallAction(Spawners w)
    {
        BuildUnit(w, w.spawnGO[0]);        
    }

    void BuildUnit(Spawners s, Stats b)
    {
        new AITaskSpawn(ser,b,ai.body.id,0).DoTask(s);
        ai.activeReference.Add(s,new AIConditionFinishSpawning(s,ai));
    }
}
