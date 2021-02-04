using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProtocolAttacker : IProtocol
{
    ControllerAI ai;
    Server ser;
    ITask wander;
    int offensiveUnitsCount;
    int currentlyAttackingUnitsCount;

    public IProtocol SetSources(ControllerAI a, Server s)
    {
        ai = a;
        ser = s;
        return this;
    }

    public void SetTasks(Stats s)
    {
        ConditionChecker(s);
        TakeAction(s);
    }

    void ConditionChecker(Stats s)
    {
        offensiveUnitsCount = 0;
        currentlyAttackingUnitsCount = 0;
        for (int i = 0; i < ser.playerRealUnitsLists[ai.body.id].Count; i++)
        {
            if(ser.playerRealUnitsLists[ai.body.id][i] is IAttack)
            {
                offensiveUnitsCount++;
                if(ser.playerRealUnitsLists[ai.body.id][i].target != null)
                {
                    currentlyAttackingUnitsCount++;
                }
            }           
        }
    }

    void TakeAction(Stats s)
    {
        if (offensiveUnitsCount > 15 && currentlyAttackingUnitsCount < offensiveUnitsCount / 3 && ser.timer > 300)
            Attack(s);
        else
            Wander(s);

    }

    void Wander(Stats s)
    {
        float radius = s.lineofSightRadius / 2;
        Vector3 vec = new Vector3(s.transform.position.x + Random.Range(-radius, radius), 25, s.transform.position.z + Random.Range(-radius, radius));
        Ray ray = new Ray(vec, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == 0)
            {
                Vector3 vec2 = new Vector3(vec.x,s.transform.position.y,vec.z);
                new AITaskMove(vec2, ai.move).DoTask(s);
                ai.active.Add(s);
                ai.activeReference.Add(s, new AIConditionReach(vec2, s, 3, ai));
            }           
        }
    }

    void Attack(Stats s)
    {
        if (ser.playerUnitsLists[0].Count <= 0) return;
        List<float> allDistances = new List<float>();
        Dictionary<float, Stats> targetReturner = new Dictionary<float, Stats>();
        for (int i = 0; i < ser.playerUnitsLists[0].Count; i++)
        {
            float dis = Vector3.Distance(s.transform.position, ser.playerUnitsLists[0][i].transform.position);
            if (!allDistances.Contains(dis)) allDistances.Add(dis);
            targetReturner.Add(dis, ser.playerUnitsLists[0][i]);
        }
        if (allDistances.Count != 1)
        {
            for (int i = 0; i < allDistances.Count; i++)
            {
                if (i > 0)
                {
                    if (allDistances[i] < allDistances[i - 1])
                        allDistances.Remove(allDistances[i - 1]);
                    else allDistances.Remove(allDistances[i]);
                }
            }
        }
        ai.active.Add(s);
        Stats target = targetReturner[allDistances[0]];
        IAttack a = s as IAttack;
        a.TargetSetter(target);
    }
}
