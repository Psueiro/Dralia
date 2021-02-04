using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MultiplayerLobbyMenu:MonoBehaviour
{
    public void CreateRoomToggle()
    {
        //PUNSTUFF
        transform.GetChild(8).transform.gameObject.SetActive(!transform.GetChild(8).transform.gameObject.activeSelf);
    }

    public void JoinRoom()
    {
        //PUNSTUFF
    }

    public void Back()
    {
        SceneManager.LoadScene("PlayMenu");
        //PUNSTUFF
    }
}
