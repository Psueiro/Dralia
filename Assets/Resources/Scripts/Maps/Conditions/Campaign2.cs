using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaign2 : IConditions
{
    Server _s;

    public string CanvasPrompt()
    {
        return "-Build your own army using Academies and repel the opposing forces incoming from the east." + "\n" + "-Destroy their Furnaces and build your own Altars in their place." + "\n" + "-No enemy forces may stay alive.";
    }

    public void GameStarter()
    {
        //Player
        {
            Vector3 playLoc = new Vector3(-70, 0, 00);
            _s.CreateNewPlayer(playLoc, 1, 0, Color.red, new ControllerPlayer());
            _s.Spawn("Ziggurat", 0, new Vector3(playLoc.x - 14, playLoc.y, playLoc.z));
            _s.Spawn("Summoner", 0, new Vector3(playLoc.x, playLoc.y + 1, playLoc.z));
            _s.Spawn("Summoner", 0, new Vector3(playLoc.x + 1, playLoc.y + 1, playLoc.z + 1));
            //_s.Spawn("Summoner", 0, new Vector3(playLoc.x + 1, playLoc.y + 1, playLoc.z - 1));
            //_s.Spawn("Summoner", 0, new Vector3(playLoc.x - 1, playLoc.y + 1, playLoc.z + 1));
            //_s.Spawn("Summoner", 0, new Vector3(playLoc.x - 1, playLoc.y + 1, playLoc.z - 1));
            _s.Spawn("Golem", 0, new Vector3(playLoc.x - 12, playLoc.y + 1, playLoc.z + 6));
            _s.Spawn("Golem", 0, new Vector3(playLoc.x - 8, playLoc.y + 1, playLoc.z + 3));
            //_s.Spawn("Golem", 0, new Vector3(playLoc.x - 4, playLoc.y + 1, playLoc.z));
            //_s.Spawn("Golem", 0, new Vector3(playLoc.x - 8, playLoc.y + 1, playLoc.z - 3));
            //_s.Spawn("Golem", 0, new Vector3(playLoc.x - 12, playLoc.y + 1, playLoc.z - 6));

            _s.Spawn("Altar", 0, new Vector3(-95.4f, 0, -14));
            _s.Spawn("Altar", 0, new Vector3(-73.6f, 0, 72.6f));
            _s.Spawn("Gravity Portal", 0, new Vector3(-70.5f, 0, -30.2f));
            _s.Spawn("Gravity Portal", 0, new Vector3(-37.6f, 0, -23.4f));
            _s.Spawn("Gravity Portal", 0, new Vector3(-56.4f, 0, 21.4f));
        }

        //Enemy
        {
            Vector3 enemyLoc = new Vector3(70, 0, 0);
            _s.CreateNewPlayer(new Vector3(0, 0, 0), 0, 1, Color.blue, new ControllerAI());

            {
                _s.Spawn("House", 1, new Vector3(enemyLoc.x, enemyLoc.y + 1, enemyLoc.z));
                _s.Spawn("Furnace", 1, new Vector3(76.3f, 2.9f, 14.6f));
                _s.Spawn("Scorpion", 1, new Vector3(76.3f - 4, enemyLoc.y + 1, 14.6f - 3));
                _s.Spawn("Scorpion", 1, new Vector3(76.3f - 2, enemyLoc.y + 1, 14.6f - 4));
                _s.Spawn("Scorpion", 1, new Vector3(76.3f, enemyLoc.y + 1, 14.6f - 4));

                _s.Spawn("Furnace", 1, new Vector3(76.3f, 2.9f, -58.4f));
                _s.Spawn("Scorpion", 1, new Vector3(76.3f - 4, enemyLoc.y + 1, -58.4f));
                _s.Spawn("Scorpion", 1, new Vector3(76.3f - 2, enemyLoc.y + 1, -58.4f + 2));
                _s.Spawn("Scorpion", 1, new Vector3(76.3f, enemyLoc.y + 1, -58.4f + 2));

                _s.Spawn("Furnace", 1, new Vector3(11.46f, 2.9f, -4.87f));
                _s.Spawn("Scorpion", 1, new Vector3(11.46f - 4, enemyLoc.y + 1, -4.87f + 2));
                _s.Spawn("Scorpion", 1, new Vector3(11.46f - 6, enemyLoc.y + 1, -4.87f));
                _s.Spawn("Scorpion", 1, new Vector3(11.46f - 4, enemyLoc.y + 1, -4.87f - 2));

                _s.Spawn("Furnace", 1, new Vector3(21.6f, 2.9f, 72.6f));
                _s.Spawn("Scorpion", 1, new Vector3(21.6f - 4, enemyLoc.y + 1, 72.6f - 3));
                _s.Spawn("Scorpion", 1, new Vector3(21.6f - 2, enemyLoc.y + 1, 72.6f - 4));
                _s.Spawn("Scorpion", 1, new Vector3(21.6f, enemyLoc.y + 1, 72.6f - 4));

                _s.Spawn("Drill", 1, new Vector3(63.6f, 0, -27.2f));
                _s.Spawn("Drill", 1, new Vector3(63.6f, 0, 56.7f));
                _s.Spawn("Drill", 1, new Vector3(52.3f, 0, -72.3f));
                _s.Spawn("Drill", 1, new Vector3(10.7f, 0, -55.11f));
            }
        }


        Camera.main.transform.position = _s.allPlayers[0].transform.position + new Vector3(0, 25, 0);
        _s.unlockableLevels = 2;
        _s.levelMusic = Resources.Load("Art/Music/track6") as AudioClip;
    }

    public void LossConditions()
    {
        if (_s.playerRealUnitsLists[0].Count < 1)
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
            if (_s.playerUnitsLists[0][i].GetComponent<Furnace>() && !furnaces.Contains(_s.playerUnitsLists[0][i].GetComponent<Furnace>()))
                furnaces.Add(_s.playerUnitsLists[0][i].GetComponent<Furnace>());
        }
        for (int i = 0; i < furnaces.Count; i++)
        {
            if (furnaces[i].health <= 0) furnaces.Remove(furnaces[i]);
        }
        if (_s.playerUnitsLists[1].Count < 1 && furnaces.Count > 6)
        {
            _s.losingCivs.Add(_s.allPlayers[1]);
            _s.winningCivs.Add(_s.allPlayers[0]);
        }
    }
}