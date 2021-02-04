using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenus : MonoBehaviour
{
    public ScenarioCreator scenarioCreator;

    public void LoadMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadPlayMenu()
    {
        SceneManager.LoadScene("PlayMenu");
    }

    public void SaveMenu()
    {
        Transform t = transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 1);
        t.gameObject.SetActive(!t.gameObject.activeSelf);
    }

    public void LoadMenu()
    {
        Transform t = transform.GetChild(1).GetChild(transform.GetChild(1).childCount - 2);
        t.gameObject.SetActive(!t.gameObject.activeSelf);
    }

    public void RestartLevel()
    {
        Server ser = FindObjectOfType<Server>();
        ScenarioCreator newScen = Instantiate(scenarioCreator);
        newScen.MapNameSetter(ser.map).ConditionSetter(ser.gameCondition);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Return()
    {
        gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
