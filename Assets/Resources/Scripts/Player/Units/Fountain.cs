using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : Spawners
{

    protected override void Start()
    {
        ser = GetComponentInParent<Entity>().server;
        AddKeys(KeyCode.U);
        StatAssigner("Fountain");
        SetLastPosition(2);
        base.Start();
    }

    protected override void Update()
    {
        ListChecker();      
        base.Update();
    }
}
