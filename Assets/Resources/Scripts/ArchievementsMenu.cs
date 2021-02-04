using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class ArchievementsMenu : MonoBehaviour
{
    public Image[] archievements;
    string path;
    string data;
    UnlockablesHelper helper;
    public Color unhighlighted;
    public Color highlighted;

    void Start()
    {
        path = Application.streamingAssetsPath + "/Resources/Scripts/Unlockables.json";
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<UnlockablesHelper>(data);

        if (helper.hyperOffender) Highlighter(0, highlighted); else Highlighter(0, unhighlighted);
        if (helper.smelter) Highlighter(1, highlighted); else Highlighter(1, unhighlighted);
        if (helper.miner) Highlighter(2, highlighted); else Highlighter(2, unhighlighted);
        if (helper.hoarder) Highlighter(3, highlighted); else Highlighter(3, unhighlighted);
        if (helper.deathballer) Highlighter(4, highlighted); else Highlighter(4, unhighlighted);
    }

    void Highlighter(int i, Color c)
    {
        archievements[i].color = c;
        archievements[i].GetComponentInChildren<Text>().color = c;
    }
}
