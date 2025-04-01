using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreakManager : MonoBehaviour
{
    public GameObject doorObj;
    public GameObject roomObjMo;
    public GameObject roomObjPo;
    public GameObject roomObjPl;
    public DoorSystem door;

    public mobCheck mobCh;
    public playerCheck playerCh;
    public pointerCheck pointerCh;
    public Vector3 frontVec;
    public Vector3 backVec;

    public bool onFront;
    public bool goBreak;

    private void Start()
    {
        door = doorObj.GetComponent<DoorSystem>();

        mobCh = roomObjMo.GetComponent<mobCheck>();
        playerCh = roomObjPl.GetComponent<playerCheck>();
        pointerCh = roomObjPo.GetComponent<pointerCheck>();
    }

    private void Update()
    {
        ChackBreakBool();
    }

    private void ChackBreakBool()
    {
        if (!mobCh.haveMob 
            && pointerCh.havePointer
            && !door.isCrashed
            && !door.isOpen)
        {
            goBreak = true;
            onFront = false;
        }
        else if (mobCh.haveMob
            && !pointerCh.havePointer 
            && !door.isCrashed
            && !door.isOpen)
        {
            goBreak = true;
            onFront = true;
        }
        else
        {
            goBreak = false;
            onFront = false;
        }
    }
}
