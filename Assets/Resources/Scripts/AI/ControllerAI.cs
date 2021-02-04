using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAI : IController
{
    SelectionManager select;
    UIManager uiMan;

    public Entity body;
    public BuildingManager build;
    public MovementManager move;
    Server ser;

    public List<Stats> active = new List<Stats>();

    List<IProtocol> protocols = new List<IProtocol>();

    Dictionary<string, IProtocol> protocolAssigner = new Dictionary<string, IProtocol>();

    public Dictionary<Stats, ICondition> activeReference = new Dictionary<Stats, ICondition>();

    public void SetKeys()
    {
        if (!body) return;
        TaskAssigner();
        ConditionChecker(); 
    } 

    void TaskAssigner()
    {
        for (int i = 0; i < body.server.playerRealUnitsLists[body.id].Count; i++)
        {
            Stats currentUnit = ser.playerRealUnitsLists[body.id][i];          
            if (!active.Contains(currentUnit))
            {
                protocolAssigner[currentUnit.name].SetTasks(currentUnit);
            }

            if (currentUnit is IAttack)
                TriggerAggro(currentUnit);
        }
    }
    void TriggerAggro(Stats unit)
    {
        IAttack a = unit as IAttack;

        for (int j = 0; j < ser.playerUnitsLists[0].Count; j++)
        {
            float dist = Vector3.Distance(unit.transform.position, ser.playerUnitsLists[0][j].transform.position);
            if (dist < unit.lineofSightRadius / 2)
            {
                a.TargetSetter(ser.playerUnitsLists[0][j]);
            }
        }
    }

    void ConditionChecker()
    {
        for (int i = 0; i < active.Count; i++)
        {
            if (activeReference.ContainsKey(active[i]))
            activeReference[active[i]].CheckCondition();
        }
    }

    public void SetManagers(Entity e, SelectionManager h, BuildingManager b, MovementManager m, UIManager u)
    {
        body = e;
        select = h;
        build = b;
        move = m;
        ser = body.server;
        uiMan = u.SetBuildingManager(b).SetSelectionManager(h);
        uiMan.enabled = false;
        ProtocolAssigner();
    }

    void ProtocolAssigner() //Checks which protocol applies to who
    {
        protocols.Add(new AIProtocolWorker(ser.mapinfo));

        if(body.faction == 0)
        {
            protocols.Add(new AIProtocolTownCenter<Worker>());
        }
        else protocols.Add(new AIProtocolTownCenter<Summoner>());


        protocols.Add(new AIProtocolAttacker());

        if (body.faction == 0)
        {
            protocols.Add(new AIProtocolUnitSpawner<Scorpion,MechSoldier>());
        }
        else protocols.Add(new AIProtocolUnitSpawner<Golem,Devil>());


        for (int i = 0; i < protocols.Count; i++)
        {
            protocols[i].SetSources(this, ser);
        }

        if(!protocolAssigner.ContainsKey("Worker"))
        protocolAssigner.Add("Worker", protocols[0]);
        if(!protocolAssigner.ContainsKey("Summoner"))
        protocolAssigner.Add("Summoner", protocols[0]);

        if(!protocolAssigner.ContainsKey("Workshop"))
        protocolAssigner.Add("Workshop", protocols[1]);
        if(!protocolAssigner.ContainsKey("Fountain"))
        protocolAssigner.Add("Fountain", protocols[1]);

        if(!protocolAssigner.ContainsKey("Barracks"))
        protocolAssigner.Add("Barracks", protocols[3]);
        if(!protocolAssigner.ContainsKey("Academy"))
        protocolAssigner.Add("Academy", protocols[3]);

        if(!protocolAssigner.ContainsKey("Mech Soldier"))
        protocolAssigner.Add("Mech Soldier", protocols[2]);
        if(!protocolAssigner.ContainsKey("Scorpion"))
        protocolAssigner.Add("Scorpion", protocols[2]);
        if(!protocolAssigner.ContainsKey("Tank"))
        protocolAssigner.Add("Tank", protocols[2]);
        if(!protocolAssigner.ContainsKey("Devil"))
        protocolAssigner.Add("Devil", protocols[2]);
        if(!protocolAssigner.ContainsKey("Golem"))
        protocolAssigner.Add("Golem", protocols[2]);
    }

    public IController Clone()
    {
        ControllerAI con = new ControllerAI();
        con.SetManagers(body, select, build, move, uiMan);
        return con;
    }
}
