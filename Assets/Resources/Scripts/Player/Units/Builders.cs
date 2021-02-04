using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builders : Stats
{
    public List<KeyCode> keys = new List<KeyCode>();
    public List<Stats> spawnGO = new List<Stats>();
    protected Server ser;
    public BuildingPlaceholder currentBuiPla;

    protected void AddKeys(KeyCode key)
    {
        if (!keys.Contains(key)) keys.Add(key);
    }

    protected void AddStat(string go)
    {
        if (!spawnGO.Contains(Resources.Load<Stats>("GameObjects/Units/" + go)))
            spawnGO.Add(Resources.Load<Stats>("GameObjects/Units/" + go));
    }

    protected void StatAssigner(string name)
    {
        List<Stats> listOfBuildings = ser.unitValues[name].spawnableUnits;
        for (int i = 0; i < listOfBuildings.Count; i++)
        {
            AddStat(listOfBuildings[i].name);
        }
    }
}