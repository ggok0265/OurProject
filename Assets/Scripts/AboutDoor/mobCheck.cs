using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobCheck : MonoBehaviour
{
    public bool haveMob;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mob")
        {
            haveMob = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Mob")
        {
            haveMob = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mob")
        {
            haveMob = false;
        }
    }
}
