using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemControl : MonoBehaviour
{
    public static SystemControl Instance;
    public static bool needLoading;

    // About LoadingScene


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void GoToTitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    public static void GoToLoadingSceneNewGame()
    {
        needLoading = false;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void GoToLoadingSceneLoadGame()
    {
        needLoading = true;
        SceneManager.LoadScene("LoadingScene");
    }

    public static void GoToDeadScene()
    {
        SceneManager.LoadScene("DeadScene");
    }

    public static void GoToEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
