using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(EndingScene());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SystemControl.GoToTitleScene();
        }
    }

    IEnumerator EndingScene()
    {
        yield return new WaitForSeconds(87f);
        SystemControl.GoToTitleScene();
        yield break;
    }
}
