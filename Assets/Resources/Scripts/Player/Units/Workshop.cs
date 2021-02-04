using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : Spawners
{
    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AddKeys(KeyCode.W);
        StatAssigner("Workshop");
        SetLastPosition(2);
        base.Start();
    }

    protected override void Update()
    {
        ListChecker();
        base.Update();
    }
}
