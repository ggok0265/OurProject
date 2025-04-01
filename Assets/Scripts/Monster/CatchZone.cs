using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchZone : MonoBehaviour
{
    public bool isCatch;
    
    void Start()
    {
        isCatch = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isCatch)
        {
            isCatch = true;
            gameObject.GetComponentInParent<MonsterAI>().MonsterCatch();
        }
    }
}
