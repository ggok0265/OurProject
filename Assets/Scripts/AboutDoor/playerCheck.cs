using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCheck : MonoBehaviour
{
    public bool havePlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            havePlayer = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            havePlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mob")
        {
            havePlayer = false;
        }
    }
}
