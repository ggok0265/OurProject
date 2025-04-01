using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Trigger : MonoBehaviour
{
    private CutSceneManager cutSceneManager;
    public GameObject player;
    public GameObject scene1CameraPos;

    public bool startChase;

    void Start()
    {
        cutSceneManager = GetComponentInParent<CutSceneManager>();
    }

    void Update()
    {
        if(startChase)
        {
            scene1CameraPos.transform.Rotate(new Vector3(0, -50 * Time.deltaTime, 0));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            cutSceneManager.StartCutScene(1);
        }
    }
}
