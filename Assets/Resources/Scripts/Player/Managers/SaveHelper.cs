using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHelper 
{
    public string mapName;
    public int playerAmount;
    public float timer;
    public List<Vector3> playerLocations = new List<Vector3>();
    public List<int> playerFactions = new List<int>();
    public List<int> playerTeams = new List<int>();
    public List<Color> playerColors = new List<Color>();
    public List<float> playerCrystals = new List<float>();
    public List<float> playerMinerals = new List<float>();
    public List<string> playerControllers = new List<string>();
    public string gameCondition;
    public List<int> unitAllegiances = new List<int>();
    public List<string> unitName = new List<string>();
    public List<Vector3> unitLocations = new List<Vector3>();
    public List<Quaternion> unitRotations = new List<Quaternion>();
    public List<float> unitHealth = new List<float>();
    public Vector3 camPos;
}
