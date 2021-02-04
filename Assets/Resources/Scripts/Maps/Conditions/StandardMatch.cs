using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardMatch : IConditions
{
    Server _s;
    List<Vector3> spawnPos = new List<Vector3>();
    List<Vector3> spawnPos2 = new List<Vector3>();
    List<int> factionList = new List<int>();
    List<int> teamList = new List<int>();
    List<Color> colorList = new List<Color>();
    List<IController> controllerList = new List<IController>();
    List<List<string>> spawnUnitsList = new List<List<string>>();

    int playerAmount;

    public StandardMatch(int p, List<Vector3> spp, List<int> fl, List<int> tl, List<Color> pc, List<IController> c)
    {
        playerAmount = p;
        for (int i = 0; i < spp.Count; i++)
        {
            spawnPos.Add(spp[i]);
        }
        for (int i = 0; i < fl.Count; i++)
        {
            factionList.Add(fl[i]);
        }
        for (int i = 0; i < tl.Count; i++)
        {
            teamList.Add(tl[i]);
        }
        for (int i = 0; i < pc.Count; i++)
        {
            colorList.Add(pc[i]);
        }
        for (int i = 0; i < c.Count; i++)
        {
            controllerList.Add(c[i]);
        }
        for (int i = 0; i < 2; i++)
        {
            spawnUnitsList.Add(new List<string>());
        }

        if(!spawnUnitsList[0].Contains("Workshop"))
        spawnUnitsList[0].Add("Workshop");
        if(!spawnUnitsList[0].Contains("Worker"))
        spawnUnitsList[0].Add("Worker");
        if(!spawnUnitsList[1].Contains("Fountain"))
        spawnUnitsList[1].Add("Fountain");
        if(!spawnUnitsList[1].Contains("Summoner"))
        spawnUnitsList[1].Add("Summoner");
    }

    public string CanvasPrompt()
    {
        return null;
    }

    public void GameStarter()
    {
        spawnPos2.Clear();
        for (int i = 0; i < spawnPos.Count; i++)
        {
            spawnPos2.Add(spawnPos[i]);
        }
        for (int i = 0; i < playerAmount; i++)
        {
            Vector3 sel;
            sel = spawnPos2[Random.Range(0, spawnPos2.Count)];
            _s.CreateNewPlayer(sel,factionList[i] ,teamList[i],colorList[i], controllerList[i]);
            spawnPos2.Remove(sel);

            _s.Spawn(spawnUnitsList[factionList[i]][0], i, new Vector3(sel.x, sel.y, sel.z));
            _s.Spawn(spawnUnitsList[factionList[i]][1], i, new Vector3(sel.x - 2, sel.y, sel.z - 4));
            _s.Spawn(spawnUnitsList[factionList[i]][1], i, new Vector3(sel.x + 2, sel.y, sel.z - 4));
        }
        Camera.main.transform.position = _s.allPlayers[0].transform.position + new Vector3(0,25,0);
        _s.levelMusic = Resources.Load("Art/Music/track12") as AudioClip;
    }

    public void LossConditions()
    {
        for (int i = 0; i < _s.allPlayers.Count; i++)
        {
            if (_s.playerRealUnitsLists[i].Count < 1)
                _s.losingCivs.Add(_s.allPlayers[i]);
        }
    }

    public IConditions ServerSetter(Server s)
    {
        _s = s;
        return this;        
    }

    public void WinConditions()
    {
        if(_s.losingCivs.Count == _s.allPlayers.Count-1)
        {
            for (int i = 0; i < _s.allPlayers.Count; i++)
            {
                if (!_s.losingCivs.Contains(_s.allPlayers[i]) && !_s.winningCivs.Contains(_s.allPlayers[i])) _s.winningCivs.Add(_s.allPlayers[i]);
            }
        }
    }
}
