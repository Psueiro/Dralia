public class AIConditionFinishBuilding : ICondition
{
    Builders builder;
    BuildingPlaceholder building;
    ControllerAI cont;

    public AIConditionFinishBuilding(Builders worker, BuildingPlaceholder structure, ControllerAI a)
    {
        builder = worker;
        building = structure;
        cont = a;
    }

    public void CheckCondition()
    {
        if (building == null || !building.builders.Contains(builder))
        {
            RemoveMe();
        }
    }

    public void RemoveMe()
    {
        cont.active.Remove(builder);
        cont.activeReference.Remove(builder);
    }
}
