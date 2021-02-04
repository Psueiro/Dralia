using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenuButtons : MonoBehaviour
{
    public AudioClip[] clickButtons;
    public AudioClip music;
    SoundManager sound;

    void Start()
    {
        sound = Camera.main.GetComponent<SoundManager>();
        sound.Play(music,true);
    }

    public void PlayMenu()
    {
        sound.Play(clickButtons[0]);
        SceneManager.LoadScene("PlayMenu");
    }

    public void Credits()
    {
        sound.Play(clickButtons[0]);
        SceneManager.LoadScene("Credits");
    }

    public void Archievements()
    {
        sound.Play(clickButtons[0]);
        SceneManager.LoadScene("Archievements");
    }

    public void HowToPlay()
    {
        sound.Play(clickButtons[0]);
        SceneManager.LoadScene("HowToPlay");
    }

    public void Exit()
    {
        sound.Play(clickButtons[1]);
        Application.Quit();
    }
}
