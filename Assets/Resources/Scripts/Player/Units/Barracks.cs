using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Spawners
{
    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AddKeys(KeyCode.M);
        AddKeys(KeyCode.C);
        StatAssigner("Barracks");
        SetLastPosition(2);
        base.Start();
    }

    protected override void Update()
    {
        ListChecker();
        base.Update();
    }
}
