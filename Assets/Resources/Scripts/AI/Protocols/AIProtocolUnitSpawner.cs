using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProtocolUnitSpawner<T1, T2> : IProtocol
{
    ControllerAI ai;
    Server ser;
    int rangedCount;
    int meleeCount;

    public IProtocol SetSources(ControllerAI a, Server s)
    {
        ai = a;
        ser = s;
        return this;
    }

    public void SetTasks(Stats s)
    {
        CivContextChecker();
        CallAction(s);
    }

    void CivContextChecker()
    {
        meleeCount = 0;
        rangedCount = 0;
        for (int i = 0; i < ser.playerRealUnitsLists[ai.body.id].Count; i++)
        {
            if (ser.playerRealUnitsLists[ai.body.id][i] is T1 ) meleeCount++;
            if (ser.playerRealUnitsLists[ai.body.id][i] is T2 ) rangedCount++;
        }
    }



    void CallAction(Stats s)
    {
        Spawners w = s as Spawners;
        if (w.queue.Count < 1)
        {
            if (rangedCount <= meleeCount)
            {
                BuildUnit(w, w.spawnGO[1], 1);
            }
            else
            {
                BuildUnit(w, w.spawnGO[0], 0);
            }
        }
    }

    void BuildUnit(Spawners s, Stats b, int i)
    {
        new AITaskSpawn(ser, b, ai.body.id, i).DoTask(s);
        ai.active.Add(s);
        ai.activeReference.Add(s, new AIConditionFinishSpawning(s, ai));
    }
}
