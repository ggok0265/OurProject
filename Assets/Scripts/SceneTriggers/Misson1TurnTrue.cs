using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misson1TurnTrue : MonoBehaviour
{
    private GameManager gameManager;
    private CutSceneManager cutSceneManager;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        cutSceneManager = GetComponentInParent<CutSceneManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject mob1;
            mob1 = cutSceneManager.monster1;
                
            if(!mob1.GetComponent<MonsterAI>().isChasing)
            {
                gameManager.GetComponent<UIManager>().setQuestText("");
                gameManager.missionCheck[1] = true;
                gameManager.database.SaveBtn(); // ¿˙¿Â
            } 
        }
    }
}
