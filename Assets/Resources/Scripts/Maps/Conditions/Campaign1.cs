using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaign1 : IConditions
{
    Server _s;
    List<Vector3> furnaceList = new List<Vector3>();
    UnlockablesManager unlockablesManager;

    public Campaign1()
    {
        unlockablesManager = new UnlockablesManager();
    }

    public IConditions CryLocSetter(List<Vector3> v)
    {
        for (int i = 0; i < v.Count; i++)
        {
            furnaceList.Add(v[i]);
        }
        return this;
    }


    public string CanvasPrompt()
    {
        return "-Explore the map with your units." + "\n" + "-Destroy all enemy Furnaces." + "\n" + "-Build your own Furnaces in their place with your Workers." + "\n" + "Do it within 15 minutes.";
    }

    public void GameStarter()
    {
        Vector3 playLoc = new Vector3(80, 0, -70);
        _s.CreateNewPlayer(playLoc,0,0,Color.blue,new ControllerPlayer());
        _s.Spawn("Worker", 0, new Vector3(playLoc.x +1 , playLoc.y + 1, playLoc.z + 6f));
        _s.Spawn("Worker", 0, new Vector3(playLoc.x +3, playLoc.y + 1, playLoc.z + 6.5f));
        //_s.Spawn("Mech Soldier", 0, new Vector3(playLoc.x - 0.5f, playLoc.y + 1, playLoc.z + 4));
        //_s.Spawn("Mech Soldier", 0, new Vector3(playLoc.x - 1, playLoc.y + 1, playLoc.z + 2));
        //_s.Spawn("Mech Soldier", 0, new Vector3(playLoc.x - 1.5f, playLoc.y + 1, playLoc.z + 2.5f));
        //_s.Spawn("Mech Soldier", 0, new Vector3(playLoc.x - 2, playLoc.y + 1, playLoc.z + 3));
        _s.Spawn("Drill", 0, new Vector3(playLoc.x + 9.2f, playLoc.y , playLoc.z + 7.2f));
        _s.CreateNewPlayer(new Vector3(0,0,0),1,1,Color.red,new ControllerAI());
        for (int i = 0; i < furnaceList.Count; i++)
        {
            _s.Spawn("Altar", 1, new Vector3(furnaceList[i].x, furnaceList[i].y, furnaceList[i].z));
        }
        Camera.main.transform.position = _s.allPlayers[0].transform.position + new Vector3(0, 25, 0);
        _s.unlockableLevels = 1;
        _s.levelMusic = Resources.Load("Art/Music/track11") as AudioClip;
        //setear fowcircles a furnace y drill
        //Arreglar tema del navmesh empujando
    }

    public void LossConditions()
    {
       if (_s.playerRealUnitsLists[0].Count < 1 || _s.timer > 900)
        {
            _s.winningCivs.Add(_s.allPlayers[1]);
            _s.losingCivs.Add(_s.allPlayers[0]);
        }
    }

    public IConditions ServerSetter(Server s)
    {
        _s = s;
        return this;
    }

    public void WinConditions()
    {
        List<Furnace> furnaces = new List<Furnace>();
        for (int i = 0; i < _s.playerUnitsLists[0].Count; i++)
        {
            if (_s.playerUnitsLists[0][i].GetComponent<Furnace>() && !furnaces.Contains(_s.playerUnitsLists[0][i].GetComponent<Furnace>())) furnaces.Add(_s.playerUnitsLists[0][i].GetComponent<Furnace>());
        }
        for (int i = 0; i < furnaces.Count; i++)
        {
            if (furnaces[i].health <= 0) furnaces.Remove(furnaces[i]);
        }
        if (furnaces.Count>8)
        {
            _s.losingCivs.Add(_s.allPlayers[1]);
            _s.winningCivs.Add(_s.allPlayers[0]);
        }
    }
}
