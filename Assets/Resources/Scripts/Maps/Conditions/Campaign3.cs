using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaign3 : IConditions
{
    Server _s;

    public string CanvasPrompt()
    {
        return "-Build your own army" + "\n" + "-Attack and destroy the opposing stronghold located northwest." + "\n" + "-Destroy the tank";
    }

    public void GameStarter()
    {
        //Player
        {
            Vector3 playLoc = new Vector3(70, 0, -70);
            _s.CreateNewPlayer(playLoc, 1, 0, Color.blue, new ControllerPlayer());
            {
                _s.Spawn("Altar", 0, new Vector3(58.4f, 0, -80.2f));
                _s.Spawn("Gravity Portal", 0, new Vector3(75.8f, 0, -63.2f));

                //1 zig
                _s.Spawn("Ziggurat", 0, new Vector3(playLoc.x + 3, playLoc.y + 1, playLoc.z - 3));

                //3 sums
                _s.Spawn("Summoner", 0, new Vector3(playLoc.x + 1, playLoc.y + 1, playLoc.z - 7));
                //_s.Spawn("Summoner", 0, new Vector3(playLoc.x + 5, playLoc.y + 1, playLoc.z - 7));
                //_s.Spawn("Summoner", 0, new Vector3(playLoc.x + 3, playLoc.y + 1, playLoc.z - 7));

                //4 devils
                _s.Spawn("Devil", 0, new Vector3(playLoc.x + 3, playLoc.y + 1, playLoc.z + 1));
                _s.Spawn("Devil", 0, new Vector3(playLoc.x - 1 , playLoc.y + 1, playLoc.z + 1));
                //_s.Spawn("Devil", 0, new Vector3(playLoc.x - 3, playLoc.y + 1, playLoc.z - 1));
                //_s.Spawn("Devil", 0, new Vector3(playLoc.x - 5, playLoc.y + 1, playLoc.z - 2));
                //_s.Spawn("Devil", 0, new Vector3(playLoc.x - 5, playLoc.y + 1, playLoc.z - 4));

                //3 golems
                _s.Spawn("Golem", 0, new Vector3(playLoc.x - 8, playLoc.y + 1, playLoc.z + 5));
                _s.Spawn("Golem", 0, new Vector3(playLoc.x - 12, playLoc.y + 1, playLoc.z  ));
                //_s.Spawn("Golem", 0, new Vector3(playLoc.x - 16, playLoc.y + 1, playLoc.z - 5));
            }
        }

        //Enemy
        {
            Vector3 enemyLoc = new Vector3(0, 0, 70);
            _s.CreateNewPlayer(enemyLoc, 0, 1, Color.red, new ControllerAI());

            for (int i = 0; i < 10; i++)
            {
                _s.Spawn("House", 1, new Vector3(-40, enemyLoc.y + 1, 98 - 4 * i));
                _s.Spawn("House", 1, new Vector3(40, enemyLoc.y + 1, 98 - 4 * i));
            }

            for (int i = 0; i < 14; i++)
            {
                _s.Spawn("House", 1, new Vector3(-12 - 2 * i, enemyLoc.y + 1, 60));
                _s.Spawn("House", 1, new Vector3(12 + 2 * i, enemyLoc.y + 1, 60));
            }

            for (int i = 0; i < 5; i++)
            {
                _s.Spawn("Scorpion", 1, new Vector3(12 + 5 * i, enemyLoc.y + 1, 48));
                _s.Spawn("Scorpion", 1, new Vector3(-12 - 5 * i, enemyLoc.y + 1, 48));
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    _s.Spawn("Mech Soldier", 1, new Vector3(14.5f + 5 * i, enemyLoc.y + 1, 48));
            //    //_s.Spawn("Mech Soldier", 1, new Vector3(-14.5f - 5 * i, enemyLoc.y + 1, 48));
            //    //_s.Spawn("Mech Soldier", 1, new Vector3(10 + 3 * i, enemyLoc.y + 1, 65));
            //    //_s.Spawn("Mech Soldier", 1, new Vector3(-10 - 3 * i, enemyLoc.y + 1, 65));
            //}

            _s.Spawn("Tank", 1, new Vector3(enemyLoc.x, enemyLoc.y + 1, 60));

            //_s.Spawn("Barracks", 1, new Vector3(enemyLoc.x + 20, enemyLoc.y + 1, 80));
            //_s.Spawn("Barracks", 1, new Vector3(enemyLoc.x - 20, enemyLoc.y + 1, 80));

            _s.Spawn("Drill", 1, new Vector3(22.7f, 0, 53));
            _s.Spawn("Drill", 1, new Vector3(33.8f, 0, 88.4f));
            _s.Spawn("Drill", 1, new Vector3(-82.2f, 0, 91.5f));
            _s.Spawn("Drill", 1, new Vector3(-53f, 0, 76.9f));

            _s.Spawn("Furnace", 1, new Vector3(-64.4f, 2.9f, 64.9f));
            _s.Spawn("Furnace", 1, new Vector3(68.5f, 2.9f, 86.2f));
            _s.Spawn("Furnace", 1, new Vector3(49.6f, 2.9f, 55.4f));
            _s.Spawn("Furnace", 1, new Vector3(-44.4f, 2.9f, 49.2f));
        }

        Camera.main.transform.position = _s.allPlayers[0].transform.position + new Vector3(0, 25, 0);
        _s.unlockableLevels = 3;
        _s.levelMusic = Resources.Load("Art/Music/track13") as AudioClip;
    }

    public void LossConditions()
    {
        if (_s.playerRealUnitsLists[0].Count <= 0)
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
        if (_s.playerRealUnitsLists[1].Count < 1)
        {
            _s.losingCivs.Add(_s.allPlayers[1]);
            _s.winningCivs.Add(_s.allPlayers[0]);
        }
    }
}
