using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3Trigger : MonoBehaviour
{
    private CutSceneManager cutSceneManager;
    void Start()
    {
        cutSceneManager = GetComponentInParent<CutSceneManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && cutSceneManager.transform.gameObject.GetComponent<GameManager>().missionCheck[1])
        {
            cutSceneManager.StartCutScene(3);
        }
    }
}