using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public Text NowLoading;
    public Slider LoadingBar;

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("SampleScene");

        while (!operation.isDone)
        {
            yield return null;
            if (LoadingBar.value < 0.9f)
            {
                LoadingBar.value = Mathf.MoveTowards(LoadingBar.value, 0.9f, Time.deltaTime);
            }
            else if (LoadingBar.value >= 0.9f)
            {
                LoadingBar.value = Mathf.MoveTowards(LoadingBar.value, 1f, Time.deltaTime);
            }

            if (LoadingBar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}
