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

    // ������
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

    public void pointingObjInfo(RaycastHit pointedObj) // �����ִ� ������Ʈ ���� �޾ƿ�
    {
        if (pointedObj.transform.name == "DoorObj" && !pointedObj.transform.GetComponent<DoorSystem>().isCrashed) // �� �۵�
        {
            contactWithDoor = true;
            uIManager.onActionGuideText("�� ����ݱ�(E/��Ŭ��)");
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
        if (pointedObj.transform.tag == "Item" || pointedObj.transform.tag == "Paper") // ������/���� �Ա�
        {
            pointedobj = pointedObj.transform.gameObject;
            uIManager.onActionGuideText("������ �ݱ�(E/��Ŭ��)");
            pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            if (interact)
            {      
                if(pointedObj.transform.name == "FlashLight")
                {
                    inventory.getFlashLight = true;
                    uIManager.flashlightIcon.SetActive(true);
                    uIManager.onGuideText("F�� ������ �������� ų �� ���� �� ����.");
                    uIManager.setQuestText("");
                }
                else
                {
                    inventory.addItem(pointedObj.transform.GetComponent<Item>().itemCode);
                    uIManager.onGuideText("�������� ȹ���Ͽ���.");
                }
                Debug.Log(pointedObj.transform.GetComponent<Item>().itemCode);
                soundManager.GetItemSound();
                pointedObj.transform.gameObject.SetActive(false);
                gameManager.database.SaveBtn(); // save
            }
        }

        if(pointedObj.transform.tag == "DD") // ����, ������ ���� �ݱ�
        {
            pointedobj = pointedObj.transform.gameObject;
            if (pointedObj.transform.name == "LockPickUseDrawer")
            {
                if (!gameManager.missionCheck[4] && !inventory.searchItem(3) && !miniGameManager.isLockPicking)
                {
                    uIManager.onActionGuideText("�ڹ��踦 �� �� �������ϴ�.");
                    pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                }
                else if(gameManager.missionCheck[4])
                {
                    uIManager.onActionGuideText("�� ����(E/��Ŭ��)");
                    pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                    if (interact)
                    {
                        soundManager.LockerOpenSound();
                        pointedobj.GetComponent<BoxOpen>().Operate();
                    }
                }
                else if (inventory.searchItem(3))
                {
                    uIManager.onActionGuideText("�ڹ��� ����(E/��Ŭ��)");
                    pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                    if (interact) inventory.useItem(3);
                }
            }
            else if(pointedObj.transform.name == "SafeKeyPad" && pointedObj.transform.GetComponent<BoxOpen>().isLocked) // Ű�е� Ȱ��ȭ
            {
                //pointedobj = pointedObj.transform.gameObject;
                uIManager.onActionGuideText("��й�ȣ ����(E/��Ŭ��)");
                if(interact)
                    uIManager.onPassKeypad();
            }
            else
            {
                uIManager.onActionGuideText("����/�ݱ�(E/��Ŭ��)");
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
                    uIManager.onGuideText("����ִ�.");
                }
            }
        }

        if (pointedObj.transform.name == "ShutterButton1") 
        {
            if (!cutSceneManager.shutter1IsOpen && gameManager.missionCheck[1])
            {
                uIManager.onActionGuideText("�۵���Ű��(E/��Ŭ��)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            }
            if (interact)
                cutSceneManager.StartShutterScene1();
        }
        if (pointedObj.transform.name == "ShutterButton3")
        {
            if (!cutSceneManager.shutterScene3)
            {
                uIManager.onActionGuideText("�۵���Ű��(E/��Ŭ��)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            }
            if (interact)
                cutSceneManager.startShutterScene3();
        }
        if (pointedObj.transform.name == "ShutterButton5")
        {
            if (!cutSceneManager.shutterScene5)
            {
                uIManager.onActionGuideText("�۵���Ű��(E/��Ŭ��)");
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
                uIManager.onActionGuideText("�������� �ʿ��ϴ�.");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                if (interact && !inventory.searchItem(1))
                {
                    soundManager.MechanicalDoorLockedSound();
                }
            }
            if (inventory.searchItem(1))
            {
                uIManager.onActionGuideText("�� ����(E/��Ŭ��)");
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
                uIManager.onActionGuideText("���踦 ���� �� ���� �� ����.");

                if (inventory.searchItem(4))
                {
                    uIManager.onActionGuideText("���� �ֱ�(E/��Ŭ��)");
                    if (interact) { 
                        inventory.useItem(4);
                    }
                }
            }
            if (pointedObj.transform.name == "Green_Hole" && !isKeyHold[1])
            {
                uIManager.onActionGuideText("���踦 ���� �� ���� �� ����.");
                if (inventory.searchItem(10))
                {
                    uIManager.onActionGuideText("���� �ֱ�(E/��Ŭ��)");
                    if (interact)
                    {
                        inventory.useItem(10);
                    }
                }
            }
            if (pointedObj.transform.name == "Blue_Hole" && !isKeyHold[2])
            {
                uIManager.onActionGuideText("���踦 ���� �� ���� �� ����.");

                if (inventory.searchItem(9))
                {
                    uIManager.onActionGuideText("���� �ֱ�(E/��Ŭ��)");
                    if (interact)
                    {
                        inventory.useItem(9);
                    }
                }
            }
        }
        if(pointedObj.transform.tag == "CanAction" && pointedObj.transform.name == "PaperScanner") // ��ĳ�� �̿�
        {
            pointedobj = pointedObj.transform.gameObject;
            pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
            if (inventory.searchItem(8))
            {
                uIManager.onActionGuideText("��ĳ�ʸ� ����� �� ���� �� ����.");
                if(interact)
                {
                    inventory.removeItem(8); // ���ȹ��� ����
                    inventory.addItem(9); // ���ȿ���2 ����
                    soundManager.PrintSound();
                    gameManager.missionCheck[6] = true; // �̼�6 �Ϸ� ����
                }
            }
            else if(!inventory.searchItem(9))
            {
                uIManager.onActionGuideText("���� ���� ���� ������ �ʿ��ϴ�.");
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
                uIManager.onActionGuideText("Ż���ϱ�(E/��Ŭ��)");
                pointedObj.transform.GetComponent<PointedObjMTL>().setMtl(true);
                if (interact)
                {
                    gameManager.missionCheck[8] = true;
                    gameManager.database.SaveBtn();
                    SystemControl.GoToEndScene();
                    // �ḻ ����
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
        if (other.tag == "Dead") // ���� ����
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
