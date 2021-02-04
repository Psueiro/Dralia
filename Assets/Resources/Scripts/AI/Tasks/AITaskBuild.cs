using UnityEngine;

public class AITaskBuild : ITask
{
    Vector3 goal;
    Stats building;
    BuildingManager buildingManager;
    Entity civ;
    Builders builder;
    ControllerAI ai;

    public AITaskBuild(BuildingManager m, Vector3 g, Stats s, Entity i, Builders b, ControllerAI a)
    {
        buildingManager = m;
        goal = g;
        building = s;
        civ = i;
        builder = b;
        ai = a;
    }

    public void DoTask(Stats s)
    {
        buildingManager.SetPlacementChecker(building);
        buildingManager.plCh.id = civ.id;
        buildingManager.plCh.Positioning(buildingManager.plCh.boundX, buildingManager.plCh.boundZ);
        buildingManager.plCh.transform.position = goal;

        //if(buildingManager.plCh.able)
        if (civ.server.ResourceCompare(civ.id, buildingManager.plCh.myStat.name))
        {   
            //test with attacks
            //for (int i = 0; i < civ.server.buiPlaList[civ.id].Count; i++)
            //{
            //    if (civ.server.buiPlaList[i][civ.id].builders.Contains(builder as Builders))
            //        civ.server.buiPlaList[i][civ.id].builders.Remove(builder as Builders);
            //}
            civ.server.RequestBuilding(goal - new Vector3(0, goal.y, 0), buildingManager.plCh.transform.localScale, buildingManager.plCh.transform.rotation,
            buildingManager.plCh.GetComponent<MeshFilter>(), buildingManager.plCh.GetComponent<MeshRenderer>().materials, civ.transform,
            buildingManager.plCh.myStat.name, builder as Builders);
            buildingManager.plCh.gameObject.SetActive(false);
            ai.activeReference.Add(s, new AIConditionFinishBuilding(builder, builder.currentBuiPla, ai));
        }
    }
}
