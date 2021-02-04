using UnityEngine;
using System.IO;

public class UnlockablesManager
{
    string path;
    string data;
    UnlockablesHelper helper;

    public UnlockablesManager()
    {
        path = Application.streamingAssetsPath + "/Unlockables.json";
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<UnlockablesHelper>(data);
    }

    public void UpdateLevelsUnlocked(int i)
    {
        helper.levelsUnlocked = i;
        data = JsonUtility.ToJson(helper);
        File.WriteAllText(path, data);
    }

    public void UnlockHyperOffender()
    {
        helper.hyperOffender = true;
        data = JsonUtility.ToJson(helper);
        File.WriteAllText(path, data);
    }

    public void UnlockSmelter()
    {
        helper.smelter = true;
        data = JsonUtility.ToJson(helper);
        File.WriteAllText(path, data);
    }

    public void UnlockMiner()
    {
        helper.miner = true;
        data = JsonUtility.ToJson(helper);
        File.WriteAllText(path, data);
    }

    public void UnlockHoarder()
    {
        helper.hoarder = true;
        data = JsonUtility.ToJson(helper);
        File.WriteAllText(path, data);
    }

    public void UnlockDeathballer()
    {
        helper.deathballer = true;
        data = JsonUtility.ToJson(helper);
        File.WriteAllText(path, data);
    }
}
