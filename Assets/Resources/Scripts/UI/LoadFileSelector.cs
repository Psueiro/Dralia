using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class LoadFileSelector : MonoBehaviour
{
    UIManager _ui;
    Text _fileName;
    string fileName;
    string path;
    string data;
    SaveHelper helper;

    private void Start()
    {
        _ui = FindObjectOfType<UIManager>();
        _fileName = transform.GetChild(0).GetComponent<Text>();
    }


    public void Click()
    {
        path = Application.streamingAssetsPath + "/Saves/" + _fileName.text + ".json";
        data = File.ReadAllText(path);
        helper = JsonUtility.FromJson<SaveHelper>(data);
        transform.parent.parent.parent.parent.parent.GetChild(4).GetComponent<Text>().text = "Map: " + helper.mapName;
        transform.parent.parent.parent.parent.parent.GetChild(5).GetComponent<Text>().text = "Time: " + helper.timer.ToString();
        transform.parent.parent.parent.parent.parent.GetChild(6).GetComponent<Text>().text = "Players: " + helper.playerAmount.ToString();
        transform.parent.parent.parent.parent.parent.GetChild(9).GetComponent<Text>().text = _fileName.text;
    }

}
