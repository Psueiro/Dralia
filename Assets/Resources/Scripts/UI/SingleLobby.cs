using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SingleLobby : MonoBehaviour
{
    string path;
    string data;
    Dropdown mapDropDown;
    Dropdown[] playerFactions = new Dropdown[8];
    Dropdown[] playerColors = new Dropdown[8];
    Dropdown[] playerTeams = new Dropdown[8];
    Text mapName;
    Text mapResourceAmount;
    Text mapSize;
    Text maxPlayers;
    List<string> mapNames = new List<string>();
    Image mapPreview;
    ScenarioCreator scenarioCreator;
    public List<Color> colorList = new List<Color>();

    Transform playerList;

    DirectoryInfo dirInfo;
    MapInfoHelper helper;

    int currentPlayerAmount;

    public AudioClip[] buttonClicks;
    public AudioClip music;
    SoundManager sound;

    private void Start()
    {
        ColorAdder();
        scenarioCreator = FindObjectOfType<ScenarioCreator>();
        currentPlayerAmount = 2;
        mapDropDown = transform.GetChild(6).GetChild(0).GetComponent<Dropdown>();
        dirInfo = new DirectoryInfo(Application.streamingAssetsPath + "/MapInfo");
        MapDropDownSetter(dirInfo);
        mapName = transform.GetChild(7).GetChild(0).GetComponent<Text>();
        mapSize = transform.GetChild(7).GetChild(1).GetComponent<Text>();
        mapResourceAmount = transform.GetChild(7).GetChild(2).GetComponent<Text>();
        maxPlayers = transform.GetChild(7).GetChild(3).GetComponent<Text>();
        mapPreview = transform.GetChild(5).GetChild(0).GetChild(1).GetComponent<Image>();

        playerList = transform.GetChild(3);
        ColorDropdownSetter();

        sound = Camera.main.GetComponent<SoundManager>();
        sound.Play(music, true);
    }

    private void Update()
    {
        MapInfoChanger();
        AutomatedPlayerRemover();
        TeamDropdownRegulator();
        ColorChangerRegulator();
    }

    void MapInfoChanger()
    {
        mapName.text = mapNames[mapDropDown.value];
        path = Application.streamingAssetsPath + "/MapInfo/" + mapName.text + ".json";
        LoadMapInfo();
        //Debug.Log(Resources.Load("MapInfo/Previews/mchad"));
        mapPreview.sprite = Resources.Load<Sprite>("MapInfo/Previews/" + mapName.text);
        mapSize.text = "Map Size: " + MapSizeConverter(helper.mapBoundaries[0], helper.mapBoundaries[1], helper.mapBoundaries[2], helper.mapBoundaries[3]);
        mapResourceAmount.text = "Resources: " + MapResourceConverter(helper.cryLocations.Count, helper.minLocations.Count);
        maxPlayers.text = "Max Players: " + helper.playerMaxAmount.ToString();
    }

    string MapSizeConverter(float startX, float endX, float startY, float endY)
    {
        if (endX - startX + endY - startY > 400)
            return "Large";
        else if (endX - startX + endY - startY < 200)
            return "Small";
        else return "Medium";
    }

    string MapResourceConverter(int cry, int min)
    {
        if (cry + min > 20)
            return "Abundant";
        else return "Scarce";
    }

    void ColorDropdownSetter()
    {
        for (int i = 0; i < playerList.childCount; i++)
        {
            playerFactions[i] = playerList.GetChild(i).GetChild(2).GetChild(0).GetComponent<Dropdown>();
            playerColors[i] = playerList.GetChild(i).GetChild(3).GetChild(0).GetComponent<Dropdown>();
            playerTeams[i] = playerList.GetChild(i).transform.GetChild(4).GetChild(0).GetComponent<Dropdown>();
            playerColors[i].value = i;
        }
    }

    void ColorSelector()
    {
        for (int i = 0; i < playerList.childCount; i++)
        {
            playerColors[i].value = i;
        }
    }

    void TeamDropdownRegulator()
    {
        if (currentPlayerAmount == 2 && playerTeams[0].value == playerTeams[1].value)
        {
            if (playerTeams[1].value < playerTeams[1].options.Count - 1)
                playerTeams[1].value++;
            else playerTeams[1].value = 0;
        }
    }

    void ColorChangerRegulator()
    {
        for (int i = 0; i < playerColors.Length; i++)
        {
            for (int j = playerColors.Length - 1; j >= 0; j--)
            {
                if (playerColors[i] != playerColors[j] && playerColors[i].value == playerColors[j].value)
                {
                    if (playerColors[i].value > 0)
                    {
                        playerColors[i].value--;
                        break;
                    }
                    else playerColors[i].value = playerColors.Length - 1;
                    break;
                }
            }
        }
    }

    void MapDropDownSetter(DirectoryInfo dir)
    {
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Extension.Contains("json"))
            {
                mapDropDown.options.Add(new Dropdown.OptionData() { text = file.Name.Remove(file.Name.Length - 5) });
                mapNames.Add(file.Name.Remove(file.Name.Length - 5));
            }
        }
    }

    public void AddPlayer()
    {
        sound.Play(buttonClicks[0]);
        for (int i = 0; i < playerList.childCount; i++)
        {
            if (playerList.GetChild(i).gameObject.activeSelf == false && i < helper.playerMaxAmount)
            {
                playerList.GetChild(i).gameObject.SetActive(true);
                currentPlayerAmount++;
                break;
            }
        }
    }

    public void RemovePlayer()
    {
        for (int i = playerList.childCount - 1; i >= 0; i--)
        {
            if (playerList.GetChild(i).gameObject.activeSelf == true)
            {
                playerList.GetChild(i).gameObject.SetActive(false);
                currentPlayerAmount--;
                break;
            }
        }
    }

    public void AutomatedPlayerRemover()
    {
        for (int i = playerList.childCount - 1; i >= 0; i--)
        {
            if (i >= helper.playerMaxAmount && playerList.GetChild(i).gameObject.activeSelf == true)
            {
                playerList.GetChild(i).gameObject.SetActive(false);
                currentPlayerAmount--;
                break;
            }
        }
    }

    public void Play()
    {
        List<int> factionList = new List<int>();
        List<int> teamList = new List<int>();
        List<Color> colList = new List<Color>();
        List<IController> conList = new List<IController>();
        for (int i = 0; i < currentPlayerAmount; i++)
        {
            factionList.Add(playerFactions[i].value);
            teamList.Add(playerTeams[i].value);
            colList.Add(colorList[playerColors[i].value]);
            if (i == 0)
                conList.Add(new ControllerPlayer());
            else conList.Add(new ControllerAI());
        }

        scenarioCreator.ConditionSetter(
            new StandardMatch(currentPlayerAmount, helper.spawnableAreas, factionList, teamList, colList, conList)).MapNameSetter(mapName.text);
        scenarioCreator.LoadMapInfo();
        sound.Play(buttonClicks[0]);
        SceneManager.LoadScene("SinglePlayerSampleScene");
    }

    void ColorAdder()
    {
        colorList.Add(Color.green);
        colorList.Add(Color.red);
        colorList.Add(Color.blue);
        colorList.Add(Color.yellow);
        colorList.Add(new Color(1, 0.5f, 0));
        colorList.Add(new Color(0.5f, 0, 0.5f));
        colorList.Add(new Color(0.8f, 0.3f, 0.8f));
        colorList.Add(new Color(0.5f, 0.2f, 0));
        colorList.Add(Color.grey);
    }

    public void Back()
    {
        sound.Play(buttonClicks[1]);
        Destroy(scenarioCreator.gameObject);
        SceneManager.LoadScene("PlayMenu");
    }

    void LoadMapInfo()
    {
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<MapInfoHelper>(data);
    }

}
