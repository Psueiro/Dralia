using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    Server ser;

    Dictionary<string, IController> controllerAssigner = new Dictionary<string, IController>();
    List<IController> allControllers = new List<IController>();
    Dictionary<string, IConditions> conditionSetter = new Dictionary<string, IConditions>();
    string fileName;
    string path;
    string data;
    SaveHelper helper;
    public ScenarioCreator scenarioCreator;
    public Transform saveWindow;
    public Transform loadWindow;
    public Transform pauseMenu;

    public Transform[] obstacles;

    void Start()
    {
        ser = FindObjectOfType<Server>();
        if(!controllerAssigner.ContainsKey("ControllerPlayer"))
        controllerAssigner.Add("ControllerPlayer", new ControllerPlayer());
        if(!controllerAssigner.ContainsKey("ControllerAI"))
        controllerAssigner.Add("ControllerAI", new ControllerAI());
    }

    public void LoadData()
    {
        string nam = transform.GetChild(8).GetChild(9).GetComponent<Text>().text;
        if (nam != "")
        {
            fileName = nam;
            allControllers.Clear();
            path = Application.streamingAssetsPath + "/Saves/" + fileName + ".json";
            data = File.ReadAllText(path);
            helper = JsonUtility.FromJson<SaveHelper>(data);

            AudioSource[] audio = Camera.main.gameObject.GetComponents<AudioSource>();

            for (int i = 0; i < audio.Length; i++)
            {
                Destroy(audio[i]);
            }

            Destroy(ser.gameObject);

            for (int i = 0; i < helper.playerControllers.Count; i++)
            {
                allControllers.Add(controllerAssigner[helper.playerControllers[i]]);
            }

            for (int i = 0; i < ser.allPlayers.Count; i++)
            {
                Destroy(ser.allPlayers[i].gameObject);
            }
            ClearObstacles();
            if(!conditionSetter.ContainsKey("StandardMatch"))
            conditionSetter.Add("StandardMatch", new StandardMatch(helper.playerAmount, helper.playerLocations, helper.playerFactions, helper.playerTeams, helper.playerColors, allControllers).ServerSetter(ser));
            if(!conditionSetter.ContainsKey("Campaign1"))
            {
                conditionSetter.Add("Campaign1", new Campaign1());
            }
            if(!conditionSetter.ContainsKey("Campaign2"))
            conditionSetter.Add("Campaign2", new Campaign2());
            if(!conditionSetter.ContainsKey("Campaign3"))
            conditionSetter.Add("Campaign3", new Campaign3());
            ScenarioCreator newScen = Instantiate(scenarioCreator).SavSetter(this).MapNameSetter(helper.mapName).ConditionSetter(conditionSetter[helper.gameCondition]);


            Camera.main.transform.position = helper.camPos;

            loadWindow.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void ClearObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            for (int j = 0; j < obstacles[i].childCount; j++)
            {
                Destroy(obstacles[i].GetChild(j).gameObject);
            }
        }
    }

    public void ServerUpdate()
    {
        ser.timer = helper.timer;
        ser.map = helper.mapName;
        ser.allPlayers.Clear();
        ser.allCrystals.Clear();
        ser.allMinerals.Clear();
        ser.allHouses.Clear();
        ser.playerID = 0;
        ser.playerUnitLimits.Clear();
        ser.playerUnitsLists.Clear();
        ser.playerCurrentUnitCounters.Clear();
        ser.buiPlaList.Clear();
        ser.winningCivs.Clear();
        ser.losingCivs.Clear();
        ser.SetCondition(conditionSetter[helper.gameCondition]);

        for (int i = 0; i < helper.playerAmount; i++)
        {
            ser.CreateNewPlayer(helper.playerLocations[i], helper.playerFactions[i], helper.playerTeams[i], helper.playerColors[i], controllerAssigner[helper.playerControllers[i]]);
        }

        //save unithotkeys         
        for (int i = 0; i < helper.unitName.Count; i++)
        {
            ser.Spawn(helper.unitName[i], helper.unitAllegiances[i], helper.unitLocations[i]);
        }
    }

    public SaveManager ServerSetter(Server se)
    {
        ser = se;
        return this;
    }

    public void SaveData()
    {
        fileName = transform.GetComponentInChildren<InputField>().text;
        if(fileName != "")
        {

            path = Application.streamingAssetsPath + "/Saves/" + fileName + ".json";
            File.Create(path).Dispose();
            File.WriteAllText(path, "{}");
            data = File.ReadAllText(path);
            helper = JsonUtility.FromJson<SaveHelper>(data);

            helper.mapName = ser.map;
            helper.playerAmount = int.Parse(ser.allPlayers.Count.ToString());
            helper.timer = ser.timer;
            helper.playerLocations.Clear();
            helper.playerFactions.Clear();
            helper.playerColors.Clear();
            helper.playerCrystals.Clear();
            helper.playerMinerals.Clear();
            helper.unitName.Clear();
            helper.unitLocations.Clear();
            helper.unitRotations.Clear();
            helper.unitHealth.Clear();
            helper.unitAllegiances.Clear();
            helper.playerControllers.Clear();
            helper.gameCondition = ser.gameCondition.ToString();
            helper.camPos = Camera.main.transform.position;

            for (int i = 0; i < ser.allPlayers.Count; i++)
            {
                var player = ser.allPlayers[i];
                helper.playerLocations.Add(player.transform.position);
                helper.playerFactions.Add(player.faction);
                helper.playerTeams.Add(player.team);
                helper.playerColors.Add(player.GetComponent<Civilization>().color);
                helper.playerCrystals.Add(player.crystals);
                helper.playerMinerals.Add(player.minerals);
                helper.playerControllers.Add(ser.allPlayers[i].controller.ToString());
                for (int j = 0; j < ser.playerUnitsLists[i].Count; j++)
                {
                    helper.unitName.Add(ser.playerUnitsLists[i][j].name);
                    helper.unitAllegiances.Add(i);
                    helper.unitLocations.Add(ser.unitLocations[i][j]);
                    helper.unitRotations.Add(ser.playerUnitsLists[i][j].transform.rotation);
                    helper.unitHealth.Add(ser.playerUnitsLists[i][j].health);
                }
            }

            data = JsonUtility.ToJson(helper);
            File.WriteAllText(path, data);
            transform.GetComponentInChildren<InputField>().text = "";

            saveWindow.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
