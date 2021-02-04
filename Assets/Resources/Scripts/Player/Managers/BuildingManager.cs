using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    public PlacementChecker plCh;

    public BuildingManager(PlacementChecker p)
    {
        plCh = p;
    }

    public void SetPlacementChecker(Stats spawnObject)
    {
            plCh.boundX = spawnObject.hitboxX;
            plCh.boundZ = spawnObject.hitboxZ;
        if (spawnObject.GetComponent<MeshRenderer>())
        {
            plCh.transform.localScale = spawnObject.transform.localScale;
            plCh.transform.rotation = spawnObject.transform.rotation;
        }
        else
        {
            plCh.transform.rotation = spawnObject.transform.GetChild(0).transform.rotation;
            plCh.transform.localScale = spawnObject.transform.GetChild(0).transform.localScale;
        }

        plCh.Positioning(plCh.boundX, plCh.boundZ);
        plCh.cryCost = spawnObject.cryCost;
        plCh.minCost = spawnObject.minCost;
        plCh.myStat = spawnObject;
        plCh.layerAble = spawnObject.layerSpawn;
        if (spawnObject.GetComponent<MeshFilter>())
            plCh.GetComponent<MeshFilter>().sharedMesh = spawnObject.GetComponent<MeshFilter>().sharedMesh;
        else
            plCh.GetComponent<MeshFilter>().sharedMesh = spawnObject.GetComponentInChildren<MeshFilter>().sharedMesh;
        Material[] mat;
        if (spawnObject.GetComponent<MeshRenderer>())
            mat = spawnObject.GetComponent<MeshRenderer>().sharedMaterials;
        else
            mat = spawnObject.GetComponentInChildren<MeshRenderer>().sharedMaterials;
        plCh.GetComponent<MeshRenderer>().sharedMaterials = mat;
        plCh.gameObject.SetActive(true);
    }

    public void BuilderCancel(SelectionManager select)
    {
        for (int j = 0; j < select.selectedUnits.Count; j++)
        {
            if (select.selectedUnits[j] is Builders)
            {
                for (int i = 0; i < select.units.Count; i++)
                {
                    BuildingPlaceholder buildPlaceholder = select.units[i].GetComponent<BuildingPlaceholder>();
                    if (buildPlaceholder && buildPlaceholder.builders.Count > 0)
                    {
                        buildPlaceholder.builders.Clear();
                    }
                }
            }
        }
    }
}
