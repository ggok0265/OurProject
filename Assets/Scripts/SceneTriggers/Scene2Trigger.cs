using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Trigger : MonoBehaviour
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
            cutSceneManager.StartCutScene(2);
        }
    }
}
