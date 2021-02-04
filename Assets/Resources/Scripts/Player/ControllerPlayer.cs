using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ControllerPlayer : IController
{
    Entity civ;
    Vector3 lastRightClick;
    Vector3 lastLeftClick;
    Vector3 lastLeftRelease;
    SelectionManager select;
    BuildingManager build;
    UIManager uiMan;
    MovementManager move;

    public IController Clone()
    {
        ControllerPlayer con = new ControllerPlayer();
        con.SetManagers(civ, select, build, move, uiMan);
        return con;
    }

    public void SetKeys()
    {
        if (!civ) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider)
                {
                    lastLeftClick = hit.point;
                    if (!build.plCh.isActiveAndEnabled)
                    {
                        select.ActivateSquare(lastLeftClick);
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(!select.selSquare)return;
            if (Time.timeScale > 0 && !build.plCh.isActiveAndEnabled)
            {
                select.selSquare.Spread();
                select.selSquare.Select();
                select.selSquare.Highlight();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider)
                {
                    lastLeftRelease = hit.point;
                }
            }
            if (build.plCh.isActiveAndEnabled && build.plCh.able)
            {
                if (civ.server.ResourceCompare(civ.id, build.plCh.myStat.name)
                    //&& civ.server.playerUnitLimits[civ.id]
                    )
                {
                    for (int i = 0; i < civ.server.buiPlaList[civ.id].Count; i++)
                    {
                        if (civ.server.buiPlaList[i][civ.id].builders.Contains(select.selectedUnits[0] as Builders))
                            civ.server.buiPlaList[i][civ.id].builders.Remove(select.selectedUnits[0] as Builders);
                    }
                    civ.server.RequestBuilding(lastLeftRelease, build.plCh.transform.localScale, build.plCh.transform.rotation,
                    build.plCh.GetComponent<MeshFilter>(), build.plCh.GetComponent<MeshRenderer>().materials, civ.transform,
                    build.plCh.myStat.name, select.selectedUnits[0] as Builders);
                }
                else
                {
                    if (civ.server.allCrystals[civ.id] < build.plCh.myStat.cryCost)
                        uiMan.announcements.GetChild(0).gameObject.SetActive(true);
                    if (civ.server.allMinerals[civ.id] < build.plCh.myStat.minCost)
                        uiMan.announcements.GetChild(1).gameObject.SetActive(true);
                }

                build.plCh.able = false;
                build.plCh.gameObject.SetActive(false);
            }
            else
            {
                if (select.selSquare)
                {
                    select.SelectionFade();
                    select.selSquare.Release();
                    select.DeactivateSquare();
                }
            }
            uiMan.miniMap.MoveCamera();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider && Time.timeScale > 0)
            {
                lastRightClick = hit.point;
                BuildingPlaceholder buiPla = hit.collider.gameObject.GetComponent<BuildingPlaceholder>();

                for (int i = 0; i < select.selectedUnits.Count; i++)
                {
                    //movement
                    if (select.selectedUnits[i].GetComponent<NavMeshAgent>() && select.selectedUnits[i].transform.parent == uiMan.gameObject.transform)
                    {
                        if (hit.collider.gameObject.layer == 8)
                        {
                            var col = hit.collider.gameObject.GetComponent<Collider>();
                            Vector3 clocol = col.ClosestPointOnBounds(select.selectedUnits[i].transform.position);
                            move.Move(clocol, select.selectedUnits[i]);
                        }
                        else
                        {
                            if (uiMan.miniMap.rectangle.rect.Contains(uiMan.miniMap.localMousePosition))
                            {
                                move.Move(uiMan.miniMap.MoveUnit(), select.selectedUnits[i]);
                            }
                            else move.Move(hit.point, select.selectedUnits[i]);

                            if (select.selectedUnits[i] is IAttack)
                            {
                                IAttack myAttack = select.selectedUnits[i].GetComponent<IAttack>();
                                myAttack.TargetSetter(null);
                            }
                        }


                    }

                    //attacking
                    if (select.selectedUnits[i] is IAttack && hit.collider.gameObject.layer == 8 && hit.collider.gameObject.GetComponentInParent<Entity>().team != select.selectedUnits[i].GetComponentInParent<Entity>().team)
                    {
                        IAttack myAttack = select.selectedUnits[i].GetComponent<IAttack>();
                        Stats target = hit.collider.gameObject.GetComponent<Stats>();
                        myAttack.TargetSetter(target);
                    }

                    //continuing to build
                    if (select.selectedUnits[i] is Builders && buiPla
                        && select.selectedUnits[i].GetComponent<Builders>().spawnGO.Contains(buiPla.myPlaceHolder) //so  only workers that can build said building can build it
                        && !buiPla.builders.Contains(select.selectedUnits[i] as Builders) && buiPla.civ.id == civ.id) //so it doesn't add more builders the more I click
                    {
                        buiPla.builders.Add(select.selectedUnits[i] as Builders);
                    }
                    else build.BuilderCancel(select);
                }
            }
            build.plCh.gameObject.SetActive(false);
        }

        if (select.selectedUnits.Count == 1 && select.selectedUnits[0] is Builders)
        {
            if (select.selectedUnits[0].civ.id == civ.id)
            {
                Builders builder = select.selectedUnits[0].GetComponent<Builders>();
                for (int i = 0; i < builder.keys.Count; i++)
                {
                    if (Input.GetKeyDown(builder.keys[i]) && Time.timeScale != 0)
                    {
                        if (select.selectedUnits[0] is Spawners)
                        {
                            Spawners spawner = select.selectedUnits[0].GetComponent<Spawners>();
                            civ.server.SpawnQueueAdder(spawner, spawner.spawnGO[i], civ.id, i);
                        }
                        else
                        {
                            build.SetPlacementChecker(builder.spawnGO[i]);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (build.plCh.isActiveAndEnabled) build.plCh.gameObject.SetActive(false);
            else
            {
                uiMan.OpenThis(1);
                uiMan.Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            uiMan.OpenThis(2);
            uiMan.Pause();
        }

        if (Input.GetKeyDown(KeyCode.Pause))
        {
            uiMan.OpenThis(5);
            //uiMan.OpenThis(0);
            uiMan.Pause();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if(Time.timeScale < 10)
            {
                Time.timeScale++;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (Time.timeScale > 1)
            {
                Time.timeScale--;
            }
        }

        if (Input.GetKey(KeyCode.S) && Time.timeScale > 0)
        {
            for (int i = 0; i < select.selectedUnits.Count; i++)
            {
                if (select.selectedUnits[i].civ.id == civ.id)
                {
                    if (select.selectedUnits[i] is BuildingPlaceholder)
                    {
                        var costRefund = select.selectedUnits[i].GetComponent<BuildingPlaceholder>().myPlaceHolder.GetComponent<Stats>();
                        civ.server.allMinerals[civ.id] += costRefund.cryCost / 2;
                        civ.server.allMinerals[civ.id] += costRefund.minCost / 2;
                        select.selectedUnits[i].health = 0;
                    }
                    if (select.selectedUnits[i] is Spawners)
                    {
                        var costRefund = select.selectedUnits[i].GetComponent<Spawners>();
                        if (costRefund.spawning)
                        {
                            civ.server.allMinerals[civ.id] += costRefund.queue[0].cryCost;
                            civ.server.allMinerals[civ.id] += costRefund.queue[0].minCost;
                            civ.server.playerCurrentUnitCounters[civ.id] -= costRefund.queue[0].unitLimitPusher;
                            civ.server.spawnRequirementList.Remove(costRefund.t);
                            costRefund.queue.Remove(costRefund.queue[0]);
                            costRefund.spawning = false;
                        }
                    }
                    if (select.selectedUnits[i].GetComponent<NavMeshAgent>())
                    {
                        move.Move(select.selectedUnits[i].transform.position, select.selectedUnits[i]);
                    }
                }
            }
        }

        for (int i = 0; i < civ.server.playerRealUnitsLists[civ.id].Count; i++)
        {
            for (int j = 0; j < civ.server.playerRealUnitsLists.Count; j++)
            {
                for (int k = 0; k < civ.server.playerRealUnitsLists[j].Count; k++)
                {
                    if (civ.server.RequestDistance(civ.server.playerRealUnitsLists[civ.id][i], civ.server.playerRealUnitsLists[j][k]) < civ.server.playerRealUnitsLists[civ.id][i].attackRange && civ.server.playerRealUnitsLists[j][k].civ.team != civ.team)
                        civ.server.playerRealUnitsLists[civ.id][i].target = civ.server.playerRealUnitsLists[j][k];
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            select.BuildingPlaceHolderPreview(0.5f);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            select.BuildingPlaceHolderPreview(0);
        }

        for (int i = 0; i < select.selectionHotkeys.Count; i++)
        {
            //Change to LeftControl
            if (Input.GetKeyDown(select.selectionHotkeys[i]) && Input.GetKey(KeyCode.LeftControl))
                select.HotKeying(i);
            if (Input.GetKeyDown(select.selectionHotkeys[i]))
            {
                select.HotCalling(i);
            }
        }
    }

    public void SetManagers(Entity c, SelectionManager h, BuildingManager b, MovementManager m, UIManager u)
    {
        civ = c;
        select = h;
        build = b;
        move = m;
        uiMan = u.SetBuildingManager(b).SetSelectionManager(h);
    }
}

///Game Design
//Add story prompts
//Add Underground 

///Bugs
//AI can build things anywhere
//Spawner bar is linked to a random spawner rather than the one you're clicking on
//Make the building animation end when somethings done building
//ziggurat/Academy's placeholder is always fully opaque
//Make enemy units invisible in the fog of war
//clicking in the dark may select an enemy
//clicking on the map while selecting a unit deselects the unit //Caused by Canvas Group you either quickly move the map around or deselect your character

///Adjust if you have time
//Add Skills*
//Fix pause menus
//Fix Stat assigning* //Keeping current health, speed, attack speed, stuff they can build stored in server so people cannot hack it (full)
//Swap the Not Real Unit dynamic for a real unit dynamic instead