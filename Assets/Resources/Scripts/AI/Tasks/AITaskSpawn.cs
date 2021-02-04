public class AITaskSpawn : ITask
{
    Server ser;
    Stats u;
    int id;
    int index;

    public AITaskSpawn(Server server, Stats unit, int i, int ind)
    {
        ser = server;
        u = unit;
        id = i;
        index = ind;
    }

    public void DoTask(Stats s)
    {
        Spawners w = s as Spawners;
        ser.SpawnQueueAdder(w,u,id,index);
    }
}
