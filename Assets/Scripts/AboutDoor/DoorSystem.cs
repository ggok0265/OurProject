using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

// DoorSet 

public class DoorSystem : MonoBehaviour
{
    public UIManager uIManager;
    public SoundManager soundManager;

    public int keyNum; // KeyNumber: Set on Unity!
    public string keyName; // KeyName: Set on Unity!
 
    public GameObject doorAnchor; //
    public GameObject doorColl;
    public GameObject doorMesh;
    public GameObject crashedDoor;
    public GameObject crashedFront;
    public GameObject crashedBack;

    public bool isLock; // 
    public bool isOpen; // 
    public bool isCrashed; //

    public Vector3 frontVec;
    public Vector3 backVec;

    public void doorOperate(bool onArea, bool pull)
    {         
        if (!isOpen && onArea) // ???????? ????            
        {            
            if (isLock) // ???????? ????            
            {            
                uIManager.onGuideText("잠겨있다.");            
                soundManager.DoorLockedSound();            
            }            
            else if (!isLock) // ???????? ????            
            {
                doorOpen();            
            }          
        }          
        else if (isOpen)            
        {            
            doorClose();            
        }
    }

    public void doorOpen()
    {
        Debug.Log("Open");
        doorAnchor.GetComponent<Animator>().SetBool("isOpen", true); // ?? ?????? ?????????? ????
        isOpen = true;
        doorColl.SetActive(false);
        soundManager.DoorOpenSound();
    }

    public void doorClose()
    {
        Debug.Log("Close");
        doorAnchor.GetComponent<Animator>().SetBool("isOpen", false); // ?? ?????? ?????????? ????
        isOpen = false;
        doorColl.SetActive(true);
        soundManager.DoorCloseSound();
    }

    public void inputKey(int Num)
    {
        if(keyNum == Num)
        {
            uIManager.onGuideText("열쇠를 사용하여 문을 열었다.");
            isLock = false;
        }
    }

    public void Crash(bool onBack)
    {
        soundManager.DoorBreakSound(); // 소리 재생
        isCrashed = true;
        doorMesh.SetActive(false);
        doorColl.SetActive(false);
        if (onBack)
        {
            crashedFront.SetActive(true);
        }
        else
        {
            crashedBack.SetActive(true);
        }
        gameObject.SetActive(false);
    }

}