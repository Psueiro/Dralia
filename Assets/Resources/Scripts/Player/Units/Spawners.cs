using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : Builders
{
    public Vector3 spawnPosition;
    public bool spawning;
    public List<Stats> queue = new List<Stats>();
    public SpawnRequirements t;

    public void SetLastPosition(float dist)
    {
        spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - dist);
    }

    public void ListChecker()
    {
        if(queue.Count > 0 && !spawning)
        {
            t = new SpawnRequirements { spawner = this, timer = 0, cooldown = ser.unitValues[queue[0].name].percentageUnit};
            ser.SpawnRequirementAdder(t);
            spawning = true;
        }
    }
}
