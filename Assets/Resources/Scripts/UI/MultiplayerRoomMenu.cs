using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerRoomMenu : MonoBehaviour
{
    public Transform checkmark;

    public void Update()
    {
        //if all checkmarks are active
        //SceneManager.LoadScene("SampleScene");
    }

    public void Play()
    {
        transform.GetChild(3).GetChild(5).gameObject.SetActive(!transform.GetChild(3).GetChild(5).gameObject.activeSelf);
    }

    public void Back()
    {
        SceneManager.LoadScene("MultiplayerLobby");
        //PUNSTUFF
    }
}
