using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 신체에 접촉하는 판정을 제어하는 클래스

public class BodyCol : MonoBehaviour
{
    public GameObject Player;
    public CutSceneManager cutSceneManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ActionZone")
        {
            if (other.name == "DoorZoneFront")
            {
                Player.GetComponent<PlayerAction>().onDoorFront = true;
            }
            if (other.name == "DoorZoneBack")
            {
                Player.GetComponent<PlayerAction>().onDoorBack = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ActionZone")
        {
            if (other.name == "DoorZoneFront")
            {
                Player.GetComponent<PlayerAction>().onDoorFront = false;
            }
            if (other.name == "DoorZoneBack")
            {
                Player.GetComponent<PlayerAction>().onDoorBack = false;
            }
        }
    }
}
