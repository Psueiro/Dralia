using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Academy : Spawners
{
    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AddKeys(KeyCode.G);
        AddKeys(KeyCode.D);
        StatAssigner("Academy");
        SetLastPosition(2);
        base.Start();
    }

    protected override void Update()
    {
        ListChecker();
        base.Update();
    }
}
