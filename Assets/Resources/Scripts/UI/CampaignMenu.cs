using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class CampaignMenu : MonoBehaviour
{
    public Image campaignPicture;
    public Sprite[] campaignPictureChanges;
    public Color[] colorTints;
    public int currentLevel;
    public Text levelName;
    public Dictionary<int, string> levelNamesDictionary = new Dictionary<int, string>();
    public Dictionary<int, string> stageNameDictionary = new Dictionary<int, string>();
    ScenarioCreator scenarioCreator;
    List<IConditions> campaignConditions = new List<IConditions>();
    string data;
    string path;
    MapInfoHelper helper;
    string unlockPath;
    string unlockData;
    UnlockablesHelper unlockHelper;
    public AudioClip[] buttonClick;
    public AudioClip music;
    SoundManager sound;

    private void Start()
    {
        LevelNameDictionaryAdder();
        path = Application.streamingAssetsPath + "/MapInfo/" + stageNameDictionary[0] + ".json";
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<MapInfoHelper>(data);
        unlockPath = Application.streamingAssetsPath + "/Unlockables.json"; ;
        unlockData = File.ReadAllText(unlockPath);
        unlockHelper = JsonUtility.FromJson<UnlockablesHelper>(unlockData);

        campaignConditions.Add(new Campaign1().CryLocSetter(helper.cryLocations));
        campaignConditions.Add(new Campaign2());
        campaignConditions.Add(new Campaign3());
        scenarioCreator = FindObjectOfType<ScenarioCreator>();
        sound = Camera.main.GetComponent<SoundManager>();
        sound.Play(music,true);
    }

    private void Update()
    {   
        if (currentLevel < 0) currentLevel = 0;
        if (currentLevel >= levelNamesDictionary.Count) currentLevel = levelNamesDictionary.Count-1;
        campaignPicture.sprite = campaignPictureChanges[currentLevel];
        campaignPicture.color = colorTints[currentLevel];
        if(levelNamesDictionary.ContainsKey(currentLevel))
        levelName.text = levelNamesDictionary[currentLevel];
    }


    void LevelNameDictionaryAdder()
    {
        levelNamesDictionary.Add(0, "A honest mistake.");
        levelNamesDictionary.Add(1, "A retaliation");
        levelNamesDictionary.Add(2, "Revenge");

        stageNameDictionary.Add(0, "BorderPlain");
        stageNameDictionary.Add(1, "SteepHill");
        stageNameDictionary.Add(2, "FortressFight");
    }

    public void MoveUp()
    {
        if(currentLevel<unlockHelper.levelsUnlocked)
        currentLevel++;
        sound.Play(buttonClick[0]);
    }

    public void MoveDown()
    {
        currentLevel--;
        sound.Play(buttonClick[0]);
    }

    public void LoadStage()
    {
            scenarioCreator.ConditionSetter(campaignConditions[currentLevel]).MapNameSetter(stageNameDictionary[currentLevel]);
            scenarioCreator.LoadMapInfo();
        SceneManager.LoadScene("SinglePlayerSampleScene");
        sound.Play(buttonClick[0]);
    }

    public void Back()
    {
        SceneManager.LoadScene("PlayMenu");
        sound.Play(buttonClick[1]);
    }
}
