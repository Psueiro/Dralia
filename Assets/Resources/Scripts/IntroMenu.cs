using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class IntroMenu : MonoBehaviour
{
    VideoPlayer _vid;
    public float secs;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _vid = GetComponent<VideoPlayer>();
        StartCoroutine(ChangeScene(secs));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Return)) StartCoroutine(ChangeScene(0));
    }

    IEnumerator ChangeScene(float f)
    {
        yield return new WaitForSeconds(f);
        SceneManager.LoadScene("MainMenu");
    }
}