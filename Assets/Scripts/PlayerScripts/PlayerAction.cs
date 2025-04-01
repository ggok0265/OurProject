using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ?????????? ???????? ?????????? ???????? ??????

public class PlayerAction : MonoBehaviour
{
    public Inventory inventory;
    public GameManager gameManager;
    public SoundManager soundManager;
    public UIManager uIManager;
    public CutSceneManager cutSceneManager;
    public MiniGameManager miniGameManager;

    public bool contactWithDoor;
    public bool canOpenDoor;
    public bool onDoorFront;
    public bool onDoorBack;

    public bool hide;

    // 손전등
    public bool isflashing;
    public GameObject flashLight;

    //int itemIndex = 0;
    //public string[] getItem = new string[50];
    public GameObject pointedobj;
    public bool interact;

    public bool[] isKeyHold; //RGB 012

    void Start()
    {
        inventory = GetComponent<Inventory>();
        isflashing = false;
    }

    void Update()
    {
        InputInteractButton();
        useFlashLight();
    }

    private void InputInteractButton()
    {
        if (gameManager.sightControlMode)
        {
            interact = Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0);
        }
    }

    private void useFlashLight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inventory.getFlashLight && !isflashing && !hide)
            {
                isflashing = true;
                flashLight.SetActive(true);
            }
            else if (inventory.getFlashLight && isflashing)
            {
                isflashing = false;
                flashLight.SetActive(false);
            }
        }
    }

    public void pointingObjInfo(RaycastHit pointedObj) // 보고있는 오브젝트 값을 받아옴
    {
        if (pointedObj.transform.name == "DoorObj" && !pointedObj.transform.GetComponent<DoorSystem>().isCrashed) // 문 작동
        {
            contactWithDoor = true;
            uIManager.onActionGuideText("문 열고닫기(E/좌클릭)");
            pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            if (interact)
            {
                if (pointedObj.transform.GetComponent<DoorSystem>().isLock)
                {
                    if (inventory.searchItem(pointedObj.transform.GetComponent<DoorSystem>().keyNum))
                    {
                        inventory.useItem(pointedObj.transform.GetComponent<DoorSystem>().keyNum);
                        pointedObj.transform.GetComponent<DoorSystem>().inputKey(pointedObj.transform.GetComponent<DoorSystem>().keyNum);
                    }
                }
                pointedObj.transform.GetComponent<DoorSystem>().doorOperate(onDoorFront || onDoorBack, onDoorFront);
            }
        }
        if (pointedObj.transform.tag == "Item" || pointedObj.transform.tag == "Paper") // 아이템/서류 먹기
        {
            pointedobj = pointedObj.transform.gameObject;
            uIManager.onActionGuideText("아이템 줍기(E/좌클릭)");
            pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            if (interact)
            {      
                if(pointedObj.transform.name == "FlashLight")
                {
                    inventory.getFlashLight = true;
                    uIManager.flashlightIcon.SetActive(true);
                    uIManager.onGuideText("F를 눌러서 손전등을 킬 수 있을 것 같다.");
                    uIManager.setQuestText("");
                }
                else
                {
                    inventory.addItem(pointedObj.transform.GetComponent<Item>().itemCode);
                    uIManager.onGuideText("아이템을 획득하였다.");
                }
                Debug.Log(pointedObj.transform.GetComponent<Item>().itemCode);
                soundManager.GetItemSound();
                pointedObj.transform.gameObject.SetActive(false);
                gameManager.database.SaveBtn(); // save
            }
        }

        if(pointedObj.transform.tag == "DD") // 서랍, 수납장 열고 닫기
        {
            pointedobj = pointedObj.transform.gameObject;
            if (pointedObj.transform.name == "LockPickUseDrawer")
            {
                if (!gameManager.missionCheck[4] && !inventory.searchItem(3) && !miniGameManager.isLockPicking)
                {
                    uIManager.onActionGuideText("자물쇠를 딸 수 있을듯하다.");
                    pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                }
                else if(gameManager.missionCheck[4])
                {
                    uIManager.onActionGuideText("문 열기(E/좌클릭)");
                    pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                    if (interact)
                    {
                        soundManager.LockerOpenSound();
                        pointedobj.GetComponent<BoxOpen>().Operate();
                    }
                }
                else if (inventory.searchItem(3))
                {
                    uIManager.onActionGuideText("자물쇠 따기(E/좌클릭)");
                    pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                    if (interact) inventory.useItem(3);
                }
            }
            else if(pointedObj.transform.name == "SafeKeyPad" && pointedObj.transform.GetComponent<BoxOpen>().isLocked) // 키패드 활성화
            {
                //pointedobj = pointedObj.transform.gameObject;
                uIManager.onActionGuideText("비밀번호 해제(E/좌클릭)");
                if(interact)
                    uIManager.onPassKeypad();
            }
            else
            {
                uIManager.onActionGuideText("열기/닫기(E/좌클릭)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                if (interact && !pointedobj.GetComponent<BoxOpen>().isLocked)
                {
                    pointedobj.GetComponent<BoxOpen>().Operate();
                    if(pointedobj.GetComponent<BoxOpen>().drawerType)
                    {
                        soundManager.DrawerSound();
                    }
                }
                else if (interact && pointedobj.GetComponent<BoxOpen>().isLocked)
                {
                    uIManager.onGuideText("잠겨있다.");
                }
            }
        }

        if (pointedObj.transform.name == "ShutterButton1") 
        {
            if (!cutSceneManager.shutter1IsOpen && gameManager.missionCheck[1])
            {
                uIManager.onActionGuideText("작동시키기(E/좌클릭)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            }
            if (interact)
                cutSceneManager.StartShutterScene1();
        }
        if (pointedObj.transform.name == "ShutterButton3")
        {
            if (!cutSceneManager.shutterScene3)
            {
                uIManager.onActionGuideText("작동시키기(E/좌클릭)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            }
            if (interact)
                cutSceneManager.startShutterScene3();
        }
        if (pointedObj.transform.name == "ShutterButton5")
        {
            if (!cutSceneManager.shutterScene5)
            {
                uIManager.onActionGuideText("작동시키기(E/좌클릭)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            }
            if (interact)
                cutSceneManager.startShutterScene5();
        }

        if (pointedObj.transform.name == "1Terminal")
        {
            if (!gameManager.missionCheck[0])
            {
                pointedobj = pointedObj.transform.gameObject;
                uIManager.onActionGuideText("출입증이 필요하다.");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                if (interact && !inventory.searchItem(1))
                {
                    soundManager.MechanicalDoorLockedSound();
                }
            }
            if (inventory.searchItem(1))
            {
                uIManager.onActionGuideText("문 열기(E/좌클릭)");
                if(interact)
                {
                    inventory.useItem(1);
                    uIManager.setQuestText("");
                }
            }
        }

        if(pointedObj.transform.tag == "KeyHole")
        {
            pointedobj = pointedObj.transform.gameObject;
            pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            if (pointedObj.transform.name == "Red_Hole" && !isKeyHold[0])
            {
                uIManager.onActionGuideText("열쇠를 넣을 수 있을 것 같다.");

                if (inventory.searchItem(4))
                {
                    uIManager.onActionGuideText("열쇠 넣기(E/좌클릭)");
                    if (interact) { 
                        inventory.useItem(4);
                    }
                }
            }
            if (pointedObj.transform.name == "Green_Hole" && !isKeyHold[1])
            {
                uIManager.onActionGuideText("열쇠를 넣을 수 있을 것 같다.");
                if (inventory.searchItem(10))
                {
                    uIManager.onActionGuideText("열쇠 넣기(E/좌클릭)");
                    if (interact)
                    {
                        inventory.useItem(10);
                    }
                }
            }
            if (pointedObj.transform.name == "Blue_Hole" && !isKeyHold[2])
            {
                uIManager.onActionGuideText("열쇠를 넣을 수 있을 것 같다.");

                if (inventory.searchItem(9))
                {
                    uIManager.onActionGuideText("열쇠 넣기(E/좌클릭)");
                    if (interact)
                    {
                        inventory.useItem(9);
                    }
                }
            }
        }
        if(pointedObj.transform.tag == "CanAction" && pointedObj.transform.name == "PaperScanner") // 스캐너 미완
        {
            pointedobj = pointedObj.transform.gameObject;
            pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            if (inventory.searchItem(8))
            {
                uIManager.onActionGuideText("스캐너를 사용할 수 있을 것 같다.");
                if(interact)
                {
                    inventory.removeItem(8); // 보안문서 삭제
                    inventory.addItem(9); // 보안열쇠2 얻음
                    soundManager.PrintSound();
                    gameManager.missionCheck[6] = true; // 미션6 완료 조건
                }
            }
            else if(!inventory.searchItem(9))
            {
                uIManager.onActionGuideText("도장 찍힌 보안 문서가 필요하다.");
            }
            else
            {
                return;
            }
        }

        if(pointedObj.transform.tag == "Finish")
        {
            if(isKeyHold[0] && isKeyHold[1] && isKeyHold[2])
            {
                uIManager.onActionGuideText("탈출하기(E/좌클릭)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                if (interact)
                {
                    gameManager.missionCheck[8] = true;
                    gameManager.database.SaveBtn();
                    SystemControl.GoToEndScene();
                    // 결말 실행
                }
            }
        }

        if(pointedObj.transform.tag == "Untagged")
        {
            pointedobj = null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dead") // 낙사 엔딩
        {
            Time.timeScale = 0.2f;
            soundManager.DeadSound();
            Invoke("forInvoke", 1f);
        }
    }

    void forInvoke()
    {
        gameManager.mouseControlForUI();
        Time.timeScale = 1f;
        SystemControl.GoToDeadScene();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "PlayerHide")
        {
            hide = true;
            isflashing = false;
            flashLight.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerHide")
        {
            hide = false;
        }
    }
}
