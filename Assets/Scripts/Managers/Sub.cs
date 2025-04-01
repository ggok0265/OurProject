using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sub : MonoBehaviour
{
    public void GoToTitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void Continue()
    {
        SystemControl.GoToLoadingSceneLoadGame();
    }

    public void GoToDeadScene()
    {
        SceneManager.LoadScene("DeadScene");
    }

    public void GoToEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
