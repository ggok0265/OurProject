using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDoor : MonoBehaviour
{
    public GameObject door;
    
    public void OpenDoor()
    {
        door.GetComponent<Animator>().SetTrigger("Open");
    }
}
