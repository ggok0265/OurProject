using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Data //������ Ŭ���� ����
{
    //public string Pos;
    //public string Rotation;

    public bool GetFlashLight;
    public int[] Itemcode;
    public string[] Itemname;
    public int[] Papercode;
    public string[] Papername;
    public bool[] KeyHole;

    public bool[] MissionCheck;
    public bool[] CutSceneCheck;

    public bool MechanicalDoor1Open;
    public bool OfficeDoorOpen;
    public bool OfficeDoorCrashed;
    public bool PantryDoorOpen;
    public bool PantryDoorCrashed;
    public bool ArchiveDoorOpen;
    public bool ArchiveDoorCrashed;

    public bool FoundSecretPaper;
    public bool FoundRedKey;

    public bool LockPickUseDrawerOpen;
    public bool KeyPadUseDrawerOpen;

    public Data(bool getflashlight, int[] itemcode, string[] itemname, int[] papercode, string[] papername,
        bool[] missioncheck, bool[] cutscenecheck, bool[] keyhole,
        bool mechanicaldoor1open, bool foundsecretpaper, bool foundredkey,
        bool officedooropen, bool officedoorcrashed,
        bool pantrydooropen, bool pantrydoorcrashed,
        bool archivedooropen, bool archivedoorcrashed,
        bool lockpickusedraweropen, bool keypadusedraweropen)
    {
        //Pos = pos; //ĳ���� ��ǥ ����
        //Rotation = rotation; //ĳ���Ͱ� ���� ���� ����

        GetFlashLight = getflashlight; // �ķ���
        Itemcode = itemcode; // ������ ��ȣ ����
        Itemname = itemname; // ������ �̸� ����
        Papercode = papercode; // ���� �ڵ�
        Papername = papername; // ���� �̸�
        KeyHole = keyhole; // ���� ����

        MissionCheck = missioncheck; // �̼� üũ
        
        
        MechanicalDoor1Open = mechanicaldoor1open;
        CutSceneCheck = cutscenecheck;
        FoundSecretPaper = foundsecretpaper;
        FoundRedKey = foundredkey;

        OfficeDoorOpen = officedooropen;
        OfficeDoorCrashed = officedoorcrashed;
        PantryDoorOpen = pantrydooropen;
        PantryDoorCrashed = pantrydoorcrashed;

        ArchiveDoorOpen = archivedooropen;
        ArchiveDoorCrashed = archivedoorcrashed;

        LockPickUseDrawerOpen = lockpickusedraweropen;
        KeyPadUseDrawerOpen = keypadusedraweropen;
    }
}

public class DataBase : MonoBehaviour
{
    public GameObject[] itemDB; // �� ���� ������
    public GameObject[] paperDB; // �� ���� ����
    public GameObject flashLightObj; // �ķ���
    public Data UserData; 
    public GameObject manager; // ���� �Ŵ���
    public GameObject player; // �÷��̾� 

    // �� ���� ����
    public GameObject mDoor1; // ���� ��
    public GameObject officeDoor; // �繫�� ��
    public bool officeDoorOpen; 
    public bool officeDoorBreak;

    public GameObject pantryDoor; // ����� ��
    public bool pantryDoorOpen;
    public bool pantryDoorBreak;
    public GameObject archiveDoor; // ���� ������ ��
    public bool archiveDoorOpen;
    public bool archiveDoorBreak;

    // ���� ĳ���
    public GameObject lockPickUseDrawer;
    public bool lockPickUseDrawerOpen;
    // Ű�е�
    public GameObject keyPadUseDrawer;
    public bool keyPadUseDrawerOpen;

    // ������ ������ ���
    public bool foundSecretPaper;
    public bool foundRedKey;

    public GameObject mob1;
    public GameObject mob2;

    // ��ȭ�� ��
    public GameObject fireAlm;
    // ���� �� ����
    public EBDoorCrash eBDoorCrash;

    public void SaveBtn() //���̺��ư
    {
        StartCoroutine(SaveData());
    }

    IEnumerator SaveData() // ���̺�
    {
        //string Pos = player.transform.position.x + "/" + player.transform.position.y + "/" + player.transform.position.z; // ���̺�� ������ ������ �޾ƿ�
        //string Rotation = player.transform.rotation.eulerAngles.x + "/" + player.transform.rotation.eulerAngles.y + "/" + player.transform.rotation.eulerAngles.z;
        bool GetFlashLight = player.GetComponent<Inventory>().getFlashLight;
        
        int[] Itemcode = player.GetComponent<Inventory>().iteminventory;
        string[] Itemname = player.GetComponent<Inventory>().itemname;
        int[] Papercode = player.GetComponent<Inventory>().paperinventory;
        string[] Papername = player.GetComponent<Inventory>().papername;
        
        bool[] MissionCheck = manager.GetComponent<GameManager>().missionCheck;
        bool[] CutSceneCheck = manager.GetComponent<CutSceneManager>().sceneNum;
        bool[] KeyHole = player.GetComponent<PlayerAction>().isKeyHold;

        //�� ���� ����
        bool MechanicalDoor1Open = (manager.GetComponent<GameManager>().missionCheck[0] ? true : false);
        bool FoundSecretPaper = manager.GetComponent<UIManager>().foundSecretPaper;
        bool FoundRedKey = manager.GetComponent<UIManager>().foundRedKey;

        bool OfficeDoorOpen = !officeDoor.GetComponent<DoorSystem>().isLock;
        bool OfficeDoorBreak = officeDoor.GetComponent<DoorSystem>().isCrashed;
        bool PantryDoorOpen = !pantryDoor.GetComponent<DoorSystem>().isLock;
        bool PantryDoorBreak = pantryDoor.GetComponent<DoorSystem>().isCrashed;
        bool ArchiveDoorOpen = !archiveDoor.GetComponent<DoorSystem>().isLock;
        bool ArchiveDoorBreak = archiveDoor.GetComponent<DoorSystem>().isCrashed;

        bool LockPickUseDrawerOpen = !lockPickUseDrawer.GetComponent<BoxOpen>().isLocked;
        bool KeyPadUseDrawerOpen = !keyPadUseDrawer.GetComponent<BoxOpen>().isLocked;

        // �ý���
        UserData = new Data(
            GetFlashLight, Itemcode, Itemname, Papercode, Papername, 
            MissionCheck, CutSceneCheck, KeyHole,
            MechanicalDoor1Open, FoundSecretPaper, FoundRedKey,
            OfficeDoorOpen, OfficeDoorBreak, 
            PantryDoorOpen, PantryDoorBreak, 
            ArchiveDoorOpen, ArchiveDoorBreak,
            LockPickUseDrawerOpen, KeyPadUseDrawerOpen) ;
        string UserJson = JsonUtility.ToJson(UserData, true);
        string path = Application.dataPath + "/JsonData.json";
        File.WriteAllText(path, UserJson.ToString());

        yield return null;
    }

    public void LoadBtn() //�ε��ư
    {
        StartCoroutine(LoadData());
        //StartCoroutine(CreateChar());

        // <<�ε�>>
        player.GetComponent<Inventory>().LoadData();
        manager.GetComponent<UIManager>().LoadData();
        manager.GetComponent<GameManager>().LoadData();
        manager.GetComponent<CutSceneManager>().LoadData();
        for(int i = 0; i < 3 ; i++)
        {
            player.GetComponent<PlayerAction>().isKeyHold[i] = UserData.KeyHole[i]; // ���� ����
        }

        player.GetComponent<Inventory>().getFlashLight = UserData.GetFlashLight;

        // ��
        officeDoorBreak = UserData.OfficeDoorCrashed;
        pantryDoorOpen = UserData.PantryDoorOpen;
        pantryDoorBreak = UserData.PantryDoorCrashed;
        archiveDoorOpen = UserData.ArchiveDoorOpen;
        archiveDoorBreak = UserData.ArchiveDoorCrashed;

        lockPickUseDrawerOpen = UserData.LockPickUseDrawerOpen;


        // ������ ������ ���
        foundSecretPaper = UserData.FoundSecretPaper;
        foundRedKey = UserData.FoundRedKey;

        // <<����>>
        // �κ��丮 �ε�
        for (int i = 0; i < 10; i++)
        {
            if(player.GetComponent<Inventory>().iteminventory[i] != 0)
            {
                Destroy(itemDB[player.GetComponent<Inventory>().iteminventory[i]]);
            }
            if (player.GetComponent<Inventory>().paperinventory[i] != 0)
            {
                Destroy(paperDB[player.GetComponent<Inventory>().paperinventory[i]]);
            }
        }

        // <������ ����>
        if (player.GetComponent<Inventory>().getFlashLight) // �ķ���
        {
            flashLightObj.SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(1) || manager.GetComponent<GameManager>().missionCheck[0]) // 1: �����
        {
            itemDB[1].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(2) || pantryDoorOpen) // 2: ����� ����
        {
            itemDB[2].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(3) || !lockPickUseDrawer.GetComponent<BoxOpen>().isLocked) // 3: ����
        {
            itemDB[3].SetActive(false); ;
        }
        // 4: ����Ű pass
        if (player.GetComponent<Inventory>().searchItem(5) || archiveDoorOpen) // 5: ���� ������ ����
        {
            itemDB[5].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(6) 
            || player.GetComponent<Inventory>().searchItem(8) 
            || manager.GetComponent<GameManager>().missionCheck[6]) // 6: ���� ������ ����
        {
            itemDB[6].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(7)
            || player.GetComponent<Inventory>().searchItem(8)
            || manager.GetComponent<GameManager>().missionCheck[6]) // 7: ���� ������ ����
        {
            itemDB[7].SetActive(false); ;
        }
        // 8: ���� ���� ���� pass
        // 9: ���Ű pass
        if (player.GetComponent<Inventory>().searchItem(10)
            || player.GetComponent<PlayerAction>().isKeyHold[2]) // 10: �׸�Ű
        {
            itemDB[10].SetActive(false); ;
        }

        // <���� ����>
        for (int i = 1 ; i < 6; i++)
        {
            if(player.GetComponent<Inventory>().searchPaper(i + 20))
            {
                itemDB[i].SetActive(false);
            }
        }

        // <���й���>
        if (manager.GetComponent<UIManager>().foundSecretPaper) 
        {
            Destroy(manager.GetComponent<UIManager>().secretPaper);
        }
        // <����Ű>
        if (manager.GetComponent<UIManager>().foundRedKey)
        {
            Destroy(manager.GetComponent<UIManager>().redKey);
            Destroy(manager.GetComponent<UIManager>().redKeyImage);
        }


        // <�� ���� ����>
        if (officeDoorBreak) // �� �ı�
        {
            officeDoor.SetActive(false); 
        }
        if (pantryDoorOpen) // �� ��迩��
        {
            pantryDoor.GetComponent<DoorSystem>().isLock = false;
        }
        if (pantryDoorBreak) // �� �ı�
        {
            pantryDoor.SetActive(false);
        }
        if (archiveDoorOpen) // �� ��迩��
        {
            archiveDoor.GetComponent<DoorSystem>().isLock = false;
        }
        if (archiveDoorBreak) // �� �ı�
        {
            archiveDoor.SetActive(false);
        }

        // ���� �繰��
        if (lockPickUseDrawerOpen) 
        {
            lockPickUseDrawer.GetComponent<BoxOpen>().isLocked = false;
        }
        // Ű�е� �ݰ�
        if (keyPadUseDrawerOpen)
        {
            keyPadUseDrawer.GetComponent<BoxOpen>().isLocked = false;
        }

        fireAlm.SetActive(false); // ��ȭ�� �˶� ����
        if(player.GetComponent<Inventory>().getFlashLight)
        {
            manager.GetComponent<UIManager>().flashlightIcon.SetActive(true);
            flashLightObj.SetActive(false);
        }
        else
        {
            manager.GetComponent<UIManager>().flashlightIcon.SetActive(false);
            flashLightObj.SetActive(true);
        }
        player.GetComponent<PlayerAction>().isflashing = false;
        player.GetComponent<PlayerAction>().flashLight.SetActive(false);

        // <�̼� ��ȣ�� ��Ȳ>
        if (manager.GetComponent<GameManager>().missionCheck[8])
        {
            SystemControl.GoToLoadingSceneNewGame();
        }
        else if (manager.GetComponent<GameManager>().missionCheck[7])
        {
            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(14.378f, 17.57f, -36.659f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            // ���� �� ���� �ı��ϱ�
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // ��1 ����
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // ���� AI On
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();

            mob2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
            mob2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        }
        else if (manager.GetComponent<GameManager>().missionCheck[6])
        {
            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(-11.428f, 17.57f, -36.431f);
            player.transform.rotation = Quaternion.Euler(0, 90, 0);

            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            // ���� �� ���� �ı��ϱ�
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // ��1 ����
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // ���� AI On
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();

            mob2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
            mob2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        }
        else if (manager.GetComponent<GameManager>().missionCheck[5])
        {
            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(-11.428f, 17.57f, -36.431f);
            player.transform.rotation = Quaternion.Euler(0, 90, 0);

            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            // ���� �� ���� �ı��ϱ�
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // ��1 ����
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // ���� AI On
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();

            mob2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
            mob2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        }
        else if (manager.GetComponent<GameManager>().missionCheck[4])
        {
            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(-31.81f, 17.57f, -16.784f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            // ���� �� ���� �ı��ϱ�
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // ��1 ����
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // ���� AI On
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();

            mob2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
            mob2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        }
        else if (manager.GetComponent<GameManager>().missionCheck[3]) // ����� ����
        {
            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(-31.81f, 17.57f, -16.784f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            // ���� �� ���� �ı��ϱ�
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // ��1 ����
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // ���� AI On
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();

            mob2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
            mob2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        }
        else if (manager.GetComponent<GameManager>().missionCheck[2]) // ���� �� ���� �ı�, ��1 ��� �� ��Ȳ���� �ε�
        {
            // �� 4�� ���� �� ���ɷ� ����
            manager.GetComponent<CutSceneManager>().sceneNum[4] = false;

            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(-28.695f, 17.57f, 20.52f);
            player.transform.rotation = Quaternion.Euler(0, -180, 0);

            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            // ���� �� ���� �ı��ϱ�
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // ��1 ����
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_OFF();
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();
        }
        else if (manager.GetComponent<GameManager>().missionCheck[1]) // ��ȭ�� ���� ��Ȳ���� �ε�
        {
            // ��3 ���� �� ���ɷ� ����
            manager.GetComponent<CutSceneManager>().sceneNum[3] = false;

            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(-2.855f, 17.57f, 38.418f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // ����1 Ȱ��ȭ ����1 �۵����� �Ұ��� ��������
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(true);
            manager.GetComponent<CutSceneManager>().shutter1.transform.position = new Vector3(-26.31593f, 17.77298f, 38.48488f);
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = false;

            // �r�� ����
            eBDoorCrash.trigger = false;
            eBDoorCrash.stoper = false;
            eBDoorCrash.timer = 0;

            eBDoorCrash.doorR.SetActive(true);
            eBDoorCrash.doorR.GetComponent<Rigidbody>().isKinematic = true;
            eBDoorCrash.doorR.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            eBDoorCrash.doorR.transform.position = new Vector3(-31.79997f, 17.76076f, 38.48f);
            eBDoorCrash.doorR.transform.rotation = Quaternion.Euler(0, 0, 0);

            eBDoorCrash.doorL.SetActive(true);
            eBDoorCrash.doorL.GetComponent<Rigidbody>().isKinematic = true;
            eBDoorCrash.doorL.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            eBDoorCrash.doorL.transform.position = new Vector3(-31.79997f, 17.76076f, 38.487f);
            eBDoorCrash.doorL.transform.rotation = Quaternion.Euler(0, 0, 0);


            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            mob1.SetActive(true);
            mob1.GetComponent<Rigidbody>().isKinematic = true;
            mob1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            mob1.transform.position = new Vector3(11.2f, 16.533f, 35);
            mob1.transform.rotation = Quaternion.Euler(0, -180, 0);
            mob1.GetComponent<MonsterAI>().navMeshAgent.enabled = true;
            mob1.GetComponent<MonsterAI>().AI_ON();
            mob1.GetComponent<MonsterAI>().destinationVec = mob1.transform.position;
            mob1.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob1.GetComponent<MonsterAI>().destinationVec);
            mob1.GetComponent<MonsterAI>().holdPos = mob1.transform.position;
            mob1.GetComponent<Animator>().SetTrigger("GoExit");
            mob1.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_OFF();
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob1.GetComponent<MonsterAI>().ReStartSetting();
            mob2.GetComponent<MonsterAI>().ReStartSetting();
        }
        else if (manager.GetComponent<GameManager>().missionCheck[0]) // ��蹮 ���� ���� ��Ȳ���� �ε�
        {
            // �÷��̾� ��ġ, �ٶ󺸴� ����
            player.transform.position = new Vector3(37.057f, 17.57f, 34.326f);
            player.transform.rotation = Quaternion.Euler(0, -80, 0);

            // ����1 Ȱ��ȭ ����1 �۵����� �Ұ��� ��������
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(true);
            manager.GetComponent<CutSceneManager>().shutter1.transform.position = new Vector3(-26.31593f, 17.77298f, 38.48488f);
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = false;

            // �r�� ����
            eBDoorCrash.trigger = false;
            eBDoorCrash.stoper = false;
            eBDoorCrash.timer = 0;

            eBDoorCrash.doorR.SetActive(true);
            eBDoorCrash.doorR.GetComponent<Rigidbody>().isKinematic = true;
            eBDoorCrash.doorR.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            eBDoorCrash.doorR.transform.position = new Vector3(-31.79997f, 17.76076f, 38.48f);
            eBDoorCrash.doorR.transform.rotation = Quaternion.Euler(0, 0, 0);

            eBDoorCrash.doorL.SetActive(true);
            eBDoorCrash.doorL.GetComponent<Rigidbody>().isKinematic = true;
            eBDoorCrash.doorL.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            eBDoorCrash.doorL.transform.position = new Vector3(-31.79997f, 17.76076f, 38.487f);
            eBDoorCrash.doorL.transform.rotation = Quaternion.Euler(0, 0, 0);
            // ���� ����
            /* ���� ��ġ, �ٶ󺸴� ����, ����� ��ġ
             * AI On/Off ����
             * (����)AI ��ġ�� �ʱ�ȭ
             */

            // 1�� �ƽ� ���� �� ���ɷ� ����
            manager.GetComponent<CutSceneManager>().sceneNum[1] = false;
            manager.GetComponent<CutSceneManager>().sceneNum[2] = false;
            // 1�� �ƽ� ī�޶� ����
            manager.GetComponent<CutSceneManager>().scene1Object.GetComponent<Scene1Trigger>().scene1CameraPos.transform.rotation = Quaternion.Euler(0, 55, 0);

            mob1.SetActive(true);
            mob1.GetComponent<Rigidbody>().isKinematic = true;
            mob1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            mob1.transform.position = new Vector3(36.659f, 16.533f, 38.5f);
            mob1.transform.rotation = Quaternion.Euler(0, -90, 0);
            mob1.GetComponent<MonsterAI>().navMeshAgent.enabled = true;
            mob1.GetComponent<MonsterAI>().AI_OFF();
            mob1.GetComponent<MonsterAI>().destinationVec = mob1.transform.position;
            mob1.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob1.GetComponent<MonsterAI>().destinationVec);
            mob1.GetComponent<MonsterAI>().holdPos = mob1.transform.position; 
            mob1.GetComponent<Animator>().SetTrigger("GoExit");
            mob1.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_OFF();
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob1.GetComponent<MonsterAI>().ReStartSetting();
            mob2.GetComponent<MonsterAI>().ReStartSetting();
        }
        else
        {
            SystemControl.GoToLoadingSceneNewGame();
        }
    }

    IEnumerator LoadData()
    {
        string path = Application.dataPath + "/JsonData.json";
        string json = File.ReadAllText(path);
        UserData = JsonUtility.FromJson<Data>(json);

        yield return null;
    }

    IEnumerator CreateChar() //����� ĳ���� ����
    {
        //string[] tmpPosArray = UserData.Pos.Split('/');
        //string[] tmpRoArray = UserData.Rotation.Split('/');
        //
        //Vector3 TmpPos = new Vector3(float.Parse(tmpPosArray[0]), float.Parse(tmpPosArray[1]), float.Parse(tmpPosArray[2]));
        //Vector3 TmpRo = new Vector3(float.Parse(tmpRoArray[0]), float.Parse(tmpRoArray[1]), float.Parse(tmpRoArray[2]));

        //player.transform.position = (TmpPos);
        //player.transform.rotation = Quaternion.Euler(TmpRo);
        yield return null;
    }
}