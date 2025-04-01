using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerCheck : MonoBehaviour
{
    public bool havePointer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pointer")
        {
            havePointer = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pointer")
        {
            havePointer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mob")
        {
            havePointer = false;
        }
    }
}
