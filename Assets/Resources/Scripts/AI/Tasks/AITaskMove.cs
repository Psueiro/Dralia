using UnityEngine;

public class AITaskMove : ITask
{
    Vector3 goal;
    MovementManager move;

    public AITaskMove(Vector3 g, MovementManager m)
    {
        goal = g;
        move = m;
    }

    public void DoTask(Stats s)
    {
        move.Move(goal, s);
    }
}
