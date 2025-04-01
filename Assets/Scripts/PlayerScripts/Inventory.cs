using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public PlayerAction playerAction;
    public GameManager gameManager;
    public SoundManager soundManager;
    public UIManager uIManager;
    public DataBase dataBase;
    public MiniGameManager miniGameManager;
    public bool getFlashLight;

    // <들어올땐 STACK 가는데는 순서없음>
    public int[] iteminventory = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}; // ItemCode / 0 is Null
    public string[] itemname = new string[10];
    public int[] itemamount = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}; // ItemAmount
    private static int itemmax = 10;
    public int itemcurrentCapacity = 0;
    public int itemtop = 0;

    public int[] paperinventory = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}; // ItemCode / 0 is Null
    public string[] papername = new string[10];
    private static int papermax = 10;
    public int papercurrentCapacity = 0;
    private int papertop = 0;
    private void Start()
    {
        playerAction = GetComponent<PlayerAction>();
        soundManager = gameManager.GetComponent<SoundManager>();
    }

    public void addItem(int itemCode)
    {
        if (itemCode == 9 || itemCode == 8 || itemCode == 7 || itemCode == 4 || (playerAction.pointedobj != null && playerAction.pointedobj.tag == "Item"))
        {
            bool itemalreadyIn = false;
            for (int i = 0; i <= itemcurrentCapacity; i++)
            {
                if (iteminventory[i] == itemCode)
                {
                    itemamount[i]++;
                    itemalreadyIn = true;
                    break;
                }
            }

            if (itemcurrentCapacity < itemmax && !itemalreadyIn)
            {
                if (itemtop != 0)
                {
                    for (int i = itemtop + 1; i > 0; i--)
                    {
                        iteminventory[i] = iteminventory[i - 1];
                        itemamount[i] = itemamount[i - 1];
                        itemname[i] = itemname[i - 1];
                    }
                }
                iteminventory[0] = itemCode;
                if (itemCode == 8)
                {
                    itemname[0] = "StampedDocument";
                }
                else if (itemCode == 9)
                {
                    itemname[0] = "BlueKey";
                }
                else if (itemCode == 4)
                {
                    itemname[0] = "RedKey";
                }
                else
                {
                    itemname[0] = playerAction.pointedobj.name;
                }
                itemamount[0] = 1;
                itemtop++;
                itemcurrentCapacity++;
                uIManager.setItemButton();
                gameManager.database.SaveBtn(); // save
            }
        }
        else if (itemCode == 26 || (playerAction.pointedobj != null && playerAction.pointedobj.tag == "Paper")) // 26: 임시 수학문제
        {
            int paperCode = itemCode - 20;
            bool paperalreadyIn = false;
            for (int i = 0; i <= papercurrentCapacity; i++)
            {
                if (paperinventory[i] == paperCode)
                {
                    paperalreadyIn = true;
                    break;
                }
            }

            if (papercurrentCapacity < papermax && !paperalreadyIn)
            {
                if (papertop != 0)
                {
                    for (int i = papertop + 1; i > 0; i--)
                    {
                        paperinventory[i] = paperinventory[i - 1];
                        papername[i] = papername[i - 1];
                    }
                }
                paperinventory[0] = paperCode;

                if(itemCode == 26) // 임시 수학문제
                {
                    papername[0] = "math";
                }
                else
                {
                    papername[0] = playerAction.pointedobj.name;
                }
                papertop++;
                papercurrentCapacity++;
                uIManager.setPaperButton();
            }

            //bool paperalreadyIn = false;
            //for (int i = 0; i <= papercurrentCapacity; i++)
            //{
            //    if (paperinventory[i] == itemCode)
            //    {
            //        paperalreadyIn = true;
            //        break;
            //    }
            //}
            //
            //if (papercurrentCapacity < papermax && !paperalreadyIn)
            //{
            //    if (papertop != 0)
            //    {
            //        for (int i = papertop + 1; i > 0; i--)
            //        {
            //            paperinventory[i] = paperinventory[i - 1];
            //            papername[i] = papername[i - 1];
            //        }
            //    }
            //    paperinventory[0] = itemCode;
            //    papername[0] = playerAction.pointedobj.name;
            //    papertop++;
            //    papercurrentCapacity++;
            //    uIManager.setPaperButton();
            //}
        }
        uIManager.setImageAndAmount();
    }
    public void removeItem(int itemCode)
    {
        for (int i1 = 0 ; i1 < itemcurrentCapacity ; i1++) // Find ItemCode
        {
            if (iteminventory[i1] == itemCode)
            {
                if(itemamount[i1] > 1)
                {
                    itemamount[i1]--;
                }
                else if(itemamount[i1] == 1)
                {
                    iteminventory[i1] = 0;
                    itemamount[i1] = 0;
                    for (int i2 = i1; i2 < itemcurrentCapacity ; i2++)
                    {
                        iteminventory[i2] = iteminventory[i2+1];
                        itemname[i2] = itemname[i2 + 1];
                        itemamount[i2] = itemamount[i2+1];
                    }
                    itemtop--; //
                    itemcurrentCapacity--;
                }
            }
        }
        uIManager.setImageAndAmount();
        uIManager.setItemButton();
        gameManager.database.SaveBtn(); // save
    }

    public bool searchItem(int itemCode)
    {
        for (int i = 0; i <= itemcurrentCapacity; i++)
        {
            if (iteminventory[i] == itemCode)
            {
                return true;        
            }
        }
        return false;
    }
    public bool searchPaper(int itemCode)
    {
        for (int i = 0; i <= papercurrentCapacity; i++)
        {
            if (paperinventory[i] == itemCode)
            {
                return true;
            }
        }
        return false;
    }

    public void useItem(int itemCode)
    {
        switch (itemCode)
        {
            case 0: // Input Items Name
                Debug.Log("Is Empty");
            // ItemCode0 is "null"
            break;
            case 1: // EmployeeID (사원증)
                if (searchItem(itemCode) && playerAction.pointedobj != null && playerAction.pointedobj.name == "1Terminal")
                {
                    playerAction.pointedobj.GetComponent<MechanicalDoor>().OpenDoor();
                    gameManager.missionCheck[0] = true;
                    soundManager.MechanicalDoorOpenSound(); // 문 열리는 소리 플레이
                    removeItem(itemCode);
                }
                break;
            case 2: // PantryDoorKey (탕비실 열쇠)
                if (searchItem(itemCode) && playerAction.contactWithDoor)
                {
                    gameManager.missionCheck[3] = true; // 미션3 완료 조건
                    removeItem(itemCode);
                }
                break;
            case 3: // LockPick (락픽)
                if (playerAction.pointedobj != null && playerAction.pointedobj.name == "LockPickUseDrawer")
                {
                    miniGameManager.onLockPick();
                }
                break;
            case 4: // RED KEY
                if (playerAction.pointedobj != null && playerAction.pointedobj.name == "Red_Hole")
                {
                    playerAction.isKeyHold[0] = true;
                    removeItem(itemCode);
                    soundManager.KeyHoleSound();
                }
                break;
            case 5: // ArchiveRooomDoorKey (문서 보관실 열쇠)
                if (searchItem(itemCode) && playerAction.contactWithDoor)
                {
                    gameManager.missionCheck[5] = true; // 미션5 완료 조건
                    removeItem(itemCode);
                }
                break;
            case 6: // 도장
                if (searchItem(6) && searchItem(7))
                {
                    removeItem(6);
                    removeItem(7);
                    addItem(8);
                }
                break;
            case 7: // 빈 보안문서
                if (searchItem(6) && searchItem(7))
                {
                    removeItem(6);
                    removeItem(7);
                    addItem(8);
                }
                break;
            case 8: // 도장 찍힌 보안문서
                if (playerAction.pointedobj != null && playerAction.pointedobj.transform.name == "PaperScanner")
                {
                    removeItem(8); // 보안문서 삭제
                    addItem(9); // 보안열쇠2 얻음
                    gameManager.missionCheck[6] = true; // 미션6 완료 조건
                    gameManager.database.SaveBtn(); // 저장
                }
                break;
            case 9: // BLUE KEY
                if (playerAction.pointedobj != null && playerAction.pointedobj.name == "Blue_Hole")
                {
                    playerAction.isKeyHold[2] = true;
                    removeItem(itemCode);
                    soundManager.KeyHoleSound();
                }
                break;
            case 10: // GREAN KEY
                if (playerAction.pointedobj != null && playerAction.pointedobj.name == "Green_Hole")
                {
                    playerAction.isKeyHold[1] = true;
                    removeItem(itemCode);
                    soundManager.KeyHoleSound();
                }
                break;
        }
    }

    public void LoadData()
    {
        getFlashLight = dataBase.UserData.GetFlashLight;
        for (int i = 0; i < 10; i++)
        {
            iteminventory[i] = dataBase.UserData.Itemcode[i];
            itemname[i] = dataBase.UserData.Itemname[i];
            paperinventory[i] = dataBase.UserData.Papercode[i];
            papername[i] = dataBase.UserData.Papername[i];
        }
        uIManager.setImageAndAmount();
    }
}