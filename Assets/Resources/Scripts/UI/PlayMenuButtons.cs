using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayMenuButtons : MonoBehaviour
{
    public AudioClip[] clickButtons;
    public AudioClip music;
    SoundManager sound;

    void Start()
    {
        sound = Camera.main.GetComponent<SoundManager>();
        sound.Play(music, true);
    }

    public void Campaign()
    {
        sound.Play(clickButtons[0]);
        SceneManager.LoadScene("CampaignMenu");
    }

    public void Single()
    {
        sound.Play(clickButtons[0]);
        SceneManager.LoadScene("SingleMenu");
    }

    public void Back()
    {
        sound.Play(clickButtons[1]);
        SceneManager.LoadScene("MainMenu");
    }
}
