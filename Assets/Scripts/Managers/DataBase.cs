using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Data //데이터 클래스 생성
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
        //Pos = pos; //캐릭터 좌표 저장
        //Rotation = rotation; //캐릭터가 보는 방향 저장

        GetFlashLight = getflashlight; // 후레쉬
        Itemcode = itemcode; // 아이템 번호 저장
        Itemname = itemname; // 아이템 이름 저장
        Papercode = papercode; // 문서 코드
        Papername = papername; // 문서 이름
        KeyHole = keyhole; // 열쇠 구멍

        MissionCheck = missioncheck; // 미션 체크
        
        
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
    public GameObject[] itemDB; // 씬 내의 아이템
    public GameObject[] paperDB; // 씬 내의 서류
    public GameObject flashLightObj; // 후레쉬
    public Data UserData; 
    public GameObject manager; // 게임 매니저
    public GameObject player; // 플레이어 

    // 문 상태 설정
    public GameObject mDoor1; // 기계식 문
    public GameObject officeDoor; // 사무실 문
    public bool officeDoorOpen; 
    public bool officeDoorBreak;

    public GameObject pantryDoor; // 탕비실 문
    public bool pantryDoorOpen;
    public bool pantryDoorBreak;
    public GameObject archiveDoor; // 문서 보관실 문
    public bool archiveDoorOpen;
    public bool archiveDoorBreak;

    // 락픽 캐비닛
    public GameObject lockPickUseDrawer;
    public bool lockPickUseDrawerOpen;
    // 키패드
    public GameObject keyPadUseDrawer;
    public bool keyPadUseDrawerOpen;

    // 문서에 숨겨진 요소
    public bool foundSecretPaper;
    public bool foundRedKey;

    public GameObject mob1;
    public GameObject mob2;

    // 소화전 벨
    public GameObject fireAlm;
    // 엘베 문 관련
    public EBDoorCrash eBDoorCrash;

    public void SaveBtn() //세이브버튼
    {
        StartCoroutine(SaveData());
    }

    IEnumerator SaveData() // 세이브
    {
        //string Pos = player.transform.position.x + "/" + player.transform.position.y + "/" + player.transform.position.z; // 세이브시 임의의 포지션 받아옴
        //string Rotation = player.transform.rotation.eulerAngles.x + "/" + player.transform.rotation.eulerAngles.y + "/" + player.transform.rotation.eulerAngles.z;
        bool GetFlashLight = player.GetComponent<Inventory>().getFlashLight;
        
        int[] Itemcode = player.GetComponent<Inventory>().iteminventory;
        string[] Itemname = player.GetComponent<Inventory>().itemname;
        int[] Papercode = player.GetComponent<Inventory>().paperinventory;
        string[] Papername = player.GetComponent<Inventory>().papername;
        
        bool[] MissionCheck = manager.GetComponent<GameManager>().missionCheck;
        bool[] CutSceneCheck = manager.GetComponent<CutSceneManager>().sceneNum;
        bool[] KeyHole = player.GetComponent<PlayerAction>().isKeyHold;

        //문 정보 저장
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

        // 시스템
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

    public void LoadBtn() //로드버튼
    {
        StartCoroutine(LoadData());
        //StartCoroutine(CreateChar());

        // <<로딩>>
        player.GetComponent<Inventory>().LoadData();
        manager.GetComponent<UIManager>().LoadData();
        manager.GetComponent<GameManager>().LoadData();
        manager.GetComponent<CutSceneManager>().LoadData();
        for(int i = 0; i < 3 ; i++)
        {
            player.GetComponent<PlayerAction>().isKeyHold[i] = UserData.KeyHole[i]; // 열쇠 구멍
        }

        player.GetComponent<Inventory>().getFlashLight = UserData.GetFlashLight;

        // 문
        officeDoorBreak = UserData.OfficeDoorCrashed;
        pantryDoorOpen = UserData.PantryDoorOpen;
        pantryDoorBreak = UserData.PantryDoorCrashed;
        archiveDoorOpen = UserData.ArchiveDoorOpen;
        archiveDoorBreak = UserData.ArchiveDoorCrashed;

        lockPickUseDrawerOpen = UserData.LockPickUseDrawerOpen;


        // 문서내 숨겨진 요소
        foundSecretPaper = UserData.FoundSecretPaper;
        foundRedKey = UserData.FoundRedKey;

        // <<세팅>>
        // 인벤토리 로딩
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

        // <아이템 삭제>
        if (player.GetComponent<Inventory>().getFlashLight) // 후레쉬
        {
            flashLightObj.SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(1) || manager.GetComponent<GameManager>().missionCheck[0]) // 1: 사원증
        {
            itemDB[1].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(2) || pantryDoorOpen) // 2: 탕비실 열쇠
        {
            itemDB[2].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(3) || !lockPickUseDrawer.GetComponent<BoxOpen>().isLocked) // 3: 락픽
        {
            itemDB[3].SetActive(false); ;
        }
        // 4: 레드키 pass
        if (player.GetComponent<Inventory>().searchItem(5) || archiveDoorOpen) // 5: 문서 보관실 열쇠
        {
            itemDB[5].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(6) 
            || player.GetComponent<Inventory>().searchItem(8) 
            || manager.GetComponent<GameManager>().missionCheck[6]) // 6: 문서 보관실 열쇠
        {
            itemDB[6].SetActive(false); ;
        }
        if (player.GetComponent<Inventory>().searchItem(7)
            || player.GetComponent<Inventory>().searchItem(8)
            || manager.GetComponent<GameManager>().missionCheck[6]) // 7: 문서 보관실 열쇠
        {
            itemDB[7].SetActive(false); ;
        }
        // 8: 도장 찍힌 서류 pass
        // 9: 블루키 pass
        if (player.GetComponent<Inventory>().searchItem(10)
            || player.GetComponent<PlayerAction>().isKeyHold[2]) // 10: 그린키
        {
            itemDB[10].SetActive(false); ;
        }

        // <서류 삭제>
        for (int i = 1 ; i < 6; i++)
        {
            if(player.GetComponent<Inventory>().searchPaper(i + 20))
            {
                itemDB[i].SetActive(false);
            }
        }

        // <수학문제>
        if (manager.GetComponent<UIManager>().foundSecretPaper) 
        {
            Destroy(manager.GetComponent<UIManager>().secretPaper);
        }
        // <레드키>
        if (manager.GetComponent<UIManager>().foundRedKey)
        {
            Destroy(manager.GetComponent<UIManager>().redKey);
            Destroy(manager.GetComponent<UIManager>().redKeyImage);
        }


        // <문 상태 설정>
        if (officeDoorBreak) // 문 파괴
        {
            officeDoor.SetActive(false); 
        }
        if (pantryDoorOpen) // 문 잠김여부
        {
            pantryDoor.GetComponent<DoorSystem>().isLock = false;
        }
        if (pantryDoorBreak) // 문 파괴
        {
            pantryDoor.SetActive(false);
        }
        if (archiveDoorOpen) // 문 잠김여부
        {
            archiveDoor.GetComponent<DoorSystem>().isLock = false;
        }
        if (archiveDoorBreak) // 문 파괴
        {
            archiveDoor.SetActive(false);
        }

        // 락픽 사물함
        if (lockPickUseDrawerOpen) 
        {
            lockPickUseDrawer.GetComponent<BoxOpen>().isLocked = false;
        }
        // 키패드 금고
        if (keyPadUseDrawerOpen)
        {
            keyPadUseDrawer.GetComponent<BoxOpen>().isLocked = false;
        }

        fireAlm.SetActive(false); // 소화전 알람 끄기
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

        // <미션 번호별 상황>
        if (manager.GetComponent<GameManager>().missionCheck[8])
        {
            SystemControl.GoToLoadingSceneNewGame();
        }
        else if (manager.GetComponent<GameManager>().missionCheck[7])
        {
            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(14.378f, 17.57f, -36.659f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
             */

            // 엘베 및 셔터 파괴하기
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // 몹1 제거
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // 몬스터 AI On
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
            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(-11.428f, 17.57f, -36.431f);
            player.transform.rotation = Quaternion.Euler(0, 90, 0);

            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
             */

            // 엘베 및 셔터 파괴하기
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // 몹1 제거
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // 몬스터 AI On
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
            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(-11.428f, 17.57f, -36.431f);
            player.transform.rotation = Quaternion.Euler(0, 90, 0);

            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
             */

            // 엘베 및 셔터 파괴하기
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // 몹1 제거
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // 몬스터 AI On
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
            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(-31.81f, 17.57f, -16.784f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
             */

            // 엘베 및 셔터 파괴하기
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // 몹1 제거
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // 몬스터 AI On
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();

            mob2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
            mob2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        }
        else if (manager.GetComponent<GameManager>().missionCheck[3]) // 탕비실 들어옴
        {
            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(-31.81f, 17.57f, -16.784f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
             */

            // 엘베 및 셔터 파괴하기
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // 몹1 제거
            mob1.SetActive(false);

            mob2.SetActive(true);
            mob2.transform.position = new Vector3(-30.62f, 16.55f, -1.175f);
            mob2.transform.rotation = Quaternion.Euler(0, 90, 0);
            mob2.GetComponent<MonsterAI>().AI_ON(); // 몬스터 AI On
            mob2.GetComponent<MonsterAI>().destinationVec = mob2.transform.position;
            mob2.GetComponent<MonsterAI>().navMeshAgent.SetDestination(mob2.GetComponent<MonsterAI>().destinationVec);
            mob2.GetComponent<MonsterAI>().holdPos = mob2.transform.position;
            mob2.GetComponent<Animator>().SetTrigger("GoExit");
            mob2.GetComponent<MonsterAI>().catchZone.isCatch = false;

            mob2.GetComponent<MonsterAI>().ReStartSetting();

            mob2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
            mob2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        }
        else if (manager.GetComponent<GameManager>().missionCheck[2]) // 셔터 및 엘베 파괴, 몹1 사망 후 상황으로 로드
        {
            // 씬 4번 아직 안 본걸로 설정
            manager.GetComponent<CutSceneManager>().sceneNum[4] = false;

            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(-28.695f, 17.57f, 20.52f);
            player.transform.rotation = Quaternion.Euler(0, -180, 0);

            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
             */

            // 엘베 및 셔터 파괴하기
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = true;
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(false);
            eBDoorCrash.doorL.SetActive(false);
            eBDoorCrash.doorR.SetActive(false);

            // 몹1 제거
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
        else if (manager.GetComponent<GameManager>().missionCheck[1]) // 소화전 직전 상황으로 로드
        {
            // 씬3 아직 안 본걸로 설정
            manager.GetComponent<CutSceneManager>().sceneNum[3] = false;

            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(-2.855f, 17.57f, 38.418f);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);

            // 셔터1 활성화 셔터1 작동여부 불값을 거짓으로
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(true);
            manager.GetComponent<CutSceneManager>().shutter1.transform.position = new Vector3(-26.31593f, 17.77298f, 38.48488f);
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = false;

            // 엛베 복구
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


            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
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
        else if (manager.GetComponent<GameManager>().missionCheck[0]) // 기계문 열고 나온 상황으로 로드
        {
            // 플레이어 위치, 바라보는 방향
            player.transform.position = new Vector3(37.057f, 17.57f, 34.326f);
            player.transform.rotation = Quaternion.Euler(0, -80, 0);

            // 셔터1 활성화 셔터1 작동여부 불값을 거짓으로
            manager.GetComponent<CutSceneManager>().shutter1.SetActive(true);
            manager.GetComponent<CutSceneManager>().shutter1.transform.position = new Vector3(-26.31593f, 17.77298f, 38.48488f);
            manager.GetComponent<CutSceneManager>().shutter1IsOpen = false;

            // 엛베 복구
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
            // 몬스터 세팅
            /* 몬스터 위치, 바라보는 방향, 사수할 위치
             * AI On/Off 여부
             * (공통)AI 수치값 초기화
             */

            // 1번 컷신 아직 안 본걸로 설정
            manager.GetComponent<CutSceneManager>().sceneNum[1] = false;
            manager.GetComponent<CutSceneManager>().sceneNum[2] = false;
            // 1번 컷신 카메라 설정
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

    IEnumerator CreateChar() //저장된 캐릭터 생성
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