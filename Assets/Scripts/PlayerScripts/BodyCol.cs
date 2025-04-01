using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾��� ��ü�� �����ϴ� ������ �����ϴ� Ŭ����

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
