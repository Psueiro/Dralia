using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public delegate void CommandAction();
    Dictionary<string, CommandAction> _dic = new Dictionary<string, CommandAction>();
    Dictionary<string, string> _des = new Dictionary<string, string>();
    public Text commandText;
    Server ser;
    InputField inputs;
    Scrollbar scroll;
    public FogCameraSpawner fog;


    void Start()
    {
        inputs = transform.GetChild(transform.childCount - 1).GetComponent<InputField>();
        scroll = transform.GetChild(2).GetComponent<Scrollbar>();
        ser = FindObjectOfType<Server>();

        RegisterCommand("/AddCrystals", AddCrystals, "Adds 1000 Crystals");
        RegisterCommand("/AddMinerals", AddMinerals, "Adds 1000 Minerals");
        RegisterCommand("/help", ShowHelp, "Shows all Commands");
        RegisterCommand("/LemmeWin", LemmeWin, "Makes you the winner of the match.");
        RegisterCommand("/LemmeLose", LemmeLose, "Makes you the loser  of the match.");
        RegisterCommand("/fog", FogOfWarEnabler, "Turns the fog of war on or off");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && gameObject.activeSelf)
        {
            string command = inputs.text;
            if(_dic.ContainsKey(command))
            {
                //tentativo
               _dic[command].Invoke();
            }
            inputs.text = "";
        }
    }
    void FogOfWarEnabler()
    {
        fog.gameObject.SetActive(!fog.gameObject.activeSelf);
    }

    void RegisterCommand(string name, CommandAction com, string des)
    {
        _dic.Add(name, com);
        _des.Add(name, des);
    }

    void Write(string t)
    {
        commandText.text += t + "\n";
        scroll.value = 0;
    }

    void ShowHelp()
    {
        Write("");
        foreach (var item in _des)
        {
            Write(item.Key +"=>" +item.Value);
        }
    }

    void Clear()
    {
        commandText.text = "";
    }

    void AddCrystals()
    {
        ser.allCrystals[0] += 1000;
    }

    void AddMinerals()
    {
        ser.allMinerals[0] += 1000;
    }

    void LemmeWin()
    {
        ser.winningCivs.Add(ser.allPlayers[0]);
    }

    void LemmeLose()
    {
        ser.winningCivs.Add(ser.allPlayers[1]);
    }

}
//Requiere entender como implementar la UI en online