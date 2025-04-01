using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCyclePos : MonoBehaviour
{
    public int num;
    private GameManager gameManager;
    public GameObject monster2;

    public GameObject cyclePos1;
    public GameObject cyclePos2;
    public GameObject cyclePos3;

    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mob")
        {
            if (gameManager.missionCheck[3])
            {
                if(num == 1)
                {
                    monster2.GetComponent<MonsterAI>().holdPos = cyclePos2.transform.position;
                }
                else if(num == 2)
                {
                    monster2.GetComponent<MonsterAI>().holdPos = cyclePos3.transform.position;
                }
                else
                {
                    monster2.GetComponent<MonsterAI>().holdPos = cyclePos1.transform.position;
                }
            }
            else
            {
                if (num == 2)
                {
                    monster2.GetComponent<MonsterAI>().holdPos = cyclePos1.transform.position;
                }
                else
                {
                    monster2.GetComponent<MonsterAI>().holdPos = cyclePos2.transform.position;
                }
            }
        }
    }
}
