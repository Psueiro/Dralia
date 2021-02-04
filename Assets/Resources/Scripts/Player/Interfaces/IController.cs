using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController 
{
    void SetKeys();
    void SetManagers(Entity e, SelectionManager h, BuildingManager b, MovementManager m, UIManager u);
    IController Clone();
}
