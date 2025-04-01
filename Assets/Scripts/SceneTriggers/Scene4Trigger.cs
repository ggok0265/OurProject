using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4Trigger : MonoBehaviour
{
    private CutSceneManager cutSceneManager;

    void Start()
    {
        cutSceneManager = GetComponentInParent<CutSceneManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cutSceneManager.StartCutScene(4);
        }
    }
}
