using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProtocolWorker : IProtocol
{
    Server ser;
    ControllerAI ai;
    Stack<ITask> tasks = new Stack<ITask>();
    List<Vector3> availableMineralAreas = new List<Vector3>();
    List<Vector3> availableCrystalAreas = new List<Vector3>();

    int drillCount;
    int buildingDrillCount;
    int buildingSpawnerCount;
    int buildingHouseCount;

    bool buildHouses;
    int furnaceCount;
    int buildingFurnaceCount;
    int spawnerCount;

    int order;

    MapInfoSaver map;

    public IProtocol SetSources(ControllerAI a, Server s)
    {
        ai = a;
        ser = s;
        return this;
    }

    public AIProtocolWorker(MapInfoSaver m)
    {
        map = m;
        for (int i = 0; i < map.minerals.childCount; i++)
        {
            availableMineralAreas.Add(map.minerals.GetChild(i).transform.position);
        }
        for (int i = 0; i < map.crystals.childCount; i++)
        {
            availableCrystalAreas.Add(map.crystals.GetChild(i).transform.position);
        }
    }

    public void SetTasks(Stats s)
    {
        CivContextChecker();
        CallAction(s);

        if (tasks.Count > 0)
        {
            tasks.Pop().DoTask(s);
            ai.active.Add(s);
        }
    }

    void CivContextChecker()
    {
        drillCount = 0;
        buildingDrillCount = 0;
        furnaceCount = 0;
        buildingFurnaceCount = 0;

        if (ser.playerCurrentUnitCounters[ai.body.id] > ser.playerUnitLimits[ai.body.id] - 3) buildHouses = true; else buildHouses = false;

        for (int i = 0; i < ser.playerUnitsLists[ai.body.id].Count; i++)
        {
            if (ser.playerUnitsLists[ai.body.id][i] is Drill) drillCount++;
            if (ser.playerUnitsLists[ai.body.id][i] is Furnace) furnaceCount++;
        }

        for (int i = 0; i < ser.buiPlaList[ai.body.id].Count; i++)
        {
            if (ser.buiPlaList[ai.body.id][i].myPlaceHolder is Drill) buildingDrillCount++;
            if (ser.buiPlaList[ai.body.id][i].myPlaceHolder is Furnace) buildingFurnaceCount++;
            if (ser.buiPlaList[ai.body.id][i].myPlaceHolder is Spawners) buildingSpawnerCount++;
            if (ser.buiPlaList[ai.body.id][i].myPlaceHolder is House) buildingHouseCount++;
        }
    }

    void CallAction(Stats s)
    {
        if (s == null) return;
        Builders w = s as Builders;
        if (drillCount == 0 && buildingDrillCount == 0)
        {
            BuildResourceGatherer(w, availableMineralAreas, 3);
            order++;
        }
        else if (furnaceCount == 0 && buildingFurnaceCount == 0)
        {
            BuildResourceGatherer(w, availableCrystalAreas, 2);
            order++;
        }
        else if (buildHouses && buildingHouseCount == 0)
        {
            BuildSomethingNearby(w, 0);
        }
        else
        {
            switch (order)
            {
                case 0:
                case 4:
                    BuildResourceGatherer(w, availableMineralAreas, 3);
                    break;
                case 1:
                case 5:
                case 7:
                case 11:
                    BuildResourceGatherer(w, availableCrystalAreas, 2);
                    break;
                case 2:
                case 6:
                case 8:
                case 10:
                case 12:
                    BuildSomethingNearby(w, 0);
                    break;
                case 3:
                case 9:
                case 13:
                    BuildSomethingNearby(w, 1);
                    break;
                case 14:
                    BuildResourceGatherer(w, availableMineralAreas, 3);
                    order = 0;
                    break;
            }
            order++;
        }
    }


    void BuildResourceGatherer(Builders w, List<Vector3> locations, int item)
    {
        List<float> allDistances = new List<float>();
        Dictionary<float, Vector3> vectorReturner = new Dictionary<float, Vector3>();

        for (int i = 0; i < locations.Count; i++)
        {
            float dis = Vector3.Distance(w.transform.position, locations[i]);
            if (!allDistances.Contains(dis)) allDistances.Add(dis);
            vectorReturner.Add(dis, locations[i]);
        }

        if (allDistances.Count != 1)
        {
            for (int i = allDistances.Count - 1; i >= 0; i--)
            {
                if (i > 0)
                {
                    if (allDistances[i] < allDistances[i - 1])
                        allDistances.Remove(allDistances[i - 1]);
                    else allDistances.Remove(allDistances[i]);
                }
            }
        }

        Vector3 goal = vectorReturner[allDistances[0]];
        tasks.Push(new AITaskBuild(ai.build, goal, w.spawnGO[item], ai.body, w, ai));
        locations.Remove(goal);
    }

    void BuildSomethingNearby(Builders w, int item)
    {
        float radius = w.lineofSightRadius / 2;
        Vector3 vec = new Vector3(w.transform.position.x + Random.Range(-radius, radius), 25, w.transform.position.z + Random.Range(-radius, radius));
        Ray ray = new Ray(vec, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == w.spawnGO[item].layerSpawn)
            {
                Vector3 vec2 = new Vector3(vec.x, w.transform.position.y, vec.z);
                tasks.Push(new AITaskBuild(ai.build, vec2, w.spawnGO[item], ai.body, w, ai));
            }
        }
    }


    //ai.build.SetPlacementChecker(w.spawnGO[3]);
    //ai.build.plCh.id = ai.body.id;
    //ai.build.plCh.Positioning(ai.build.plCh.boundX, ai.build.plCh.boundZ);
    //ai.build.plCh.transform.position = goal;
    //Has nothing to check the terrain below to see if it's able or not, I tried setting it but it just would check if it was able in one frame, it'd take till the update    
}
