using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uIManager;
    public PlayerCutSceneManager playerCutSceneManager;
    public DataBase dataBase;

    //컷신 관련 오브젝트
    public GameObject playerCamera; // 플레이어 카메라
    
    //0
    public GameObject scene0Camera; 
    public GameObject scene0Object;
    //1
    public GameObject scene1Camera; 
    public GameObject scene1Object;
    public GameObject scene1CameraPos;
    public GameObject monster1;
    public GameObject monster1MovePos;
    //2
    public GameObject scene2Camera;
    public GameObject scene2Object;
    //3
    public GameObject scene3Object;
    public GameObject fireBoxLight;
    //4
    public GameObject scene4Camera;
    public GameObject monster2;

    public GameObject shutter1Camera;
    public GameObject shutter1;
    public GameObject shutter1pos;
    public bool shutter1IsOpen;

    public GameObject shutter2Camera;
    public GameObject shutter2;
    public GameObject shutter2pos;
    public GameObject shutter3Camera;
    public GameObject shutter3;
    public GameObject shutter3pos;
    public GameObject shutter4Camera;
    public GameObject shutter4;
    public GameObject shutter4pos;
    public GameObject shutter5Camera;
    public GameObject shutter5;
    public GameObject shutter5pos;
    public GameObject catchedCamera; // 몹한테 잡혀죽을때 카메라. 해당 몹으로부터 할당받음
    
    public void startCutScene() // 컷신 시작 시, 이루어져야하는 명령들
    {
        uIManager.onCutSceneUI();
        gameManager.nowCutScene = true;
        gameManager.playerInCutScene();
    }
    public void stopCutScene() // 컷신 종료 시, 이루어져야하는 명령들
    {
        uIManager.offCutSceneUI();
        gameManager.nowCutScene = false;
        gameManager.playerOutCutScene();
    }
    public void moveICS(Vector3 destination, float moveSpeed, GameObject movingobj) // 코루틴 발동
    {
        Vector3 dest = new Vector3(destination.x, destination.y, destination.z);
        StartCoroutine(moveCorutine(dest, moveSpeed, movingobj));
    }

    //<컷신 리스트> 실행했는지 여부
    public bool[] sceneNum = new bool[5]; 
    /* 
     * 0: 플레이어가 수술실에서 일어남.
     * 1: 몬스터가 플레이어를 스쳐지나감.
     * 2: 몬스터가 길 모퉁이에 있는 것을 보여줌
     * 3: 소화전이 울리고 몬스터가 쫓아옴.
     * 4: 모퉁이에서 괴물이 튀어나옴.
     */ 
    
    public bool shutterScene2;
    public bool shutterScene3;
    public bool shutterScene4;
    public bool shutterScene5;

    //<컷신 실행>
    public void StartCutScene(int startNum) // 스토리 순서대로 나오는 컷신 시작
    {
        //string sceneCoroutineName;
        if (!sceneNum[startNum])
        {
            sceneNum[startNum] = true;
            //sceneCoroutineName = "Scene" + startNum;
            //StartCoroutine(sceneCoroutineName);
            switch (startNum)
            {
                case 0:
                    StartCoroutine(Scene0());
                    break;
                case 1:
                    StartCoroutine(Scene1()); 
                    break;
                case 2:
                    StartCoroutine(Scene2());
                    break;
                case 3:
                    StartCoroutine(Scene3());
                    break;
                case 4:
                    StartCoroutine(Scene4());
                    break;
            }
        }
    }
    
    public void StartCatchScene() // 몬스터한테 잡혔을 때 컷신
    {
        StartCoroutine(CatchScene());
    }
    public void startShutterScene2()
    {
        if (!shutterScene2)
        {
            shutterScene2 = true;
            StartCoroutine(ShutterScene2());
        }
    }
    public void startShutterScene3()
    {
        if (!shutterScene3)
        {
            shutterScene3 = true;
            StartCoroutine(ShutterScene3());
        }
    }
    public void startShutterScene4()
    {
        if (!shutterScene4)
        {
            shutterScene4 = true;
            StartCoroutine(ShutterScene4());
        }
    }
    public void startShutterScene5()
    {
        if (!shutterScene5)
        {
            shutterScene5 = true;
            StartCoroutine(ShutterScene5());
        }
    }

    //<컷신 장면들>
    // 스토리 순서대로 나오는 컷신
    IEnumerator Scene0() // 0: 플레이어가 수술실에서 일어남.
    {
        Debug.Log("start0");
        startCutScene();
        playerCamera.SetActive(false);
        scene0Camera.SetActive(true);
        yield return new WaitForSeconds(27);
        scene0Camera.SetActive(false);
        playerCamera.SetActive(true);
        scene0Object.SetActive(false);
        Debug.Log("cut0");
        stopCutScene();
        uIManager.setQuestText("이동: W A S D");
        yield break;
    }
    IEnumerator Scene1() // 1: 몬스터가 플레이어를 스쳐지나감. 
    {
        startCutScene();
        playerCamera.SetActive(false);
        scene1Camera.SetActive(true);
        monster1.GetComponent<MonsterAI>().MonsterMove(new Vector3(11.2f, 0 ,35), false);
        monster1.GetComponent<MonsterAI>().holdPos = new Vector3(11.2f, 0, 35);
        yield return new WaitForSeconds(2.5f);
        scene1Object.GetComponent<Scene1Trigger>().startChase = true;
        yield return new WaitForSeconds(2);
        scene1Object.GetComponent<Scene1Trigger>().startChase = false;
        yield return new WaitForSeconds(3);
        monster1.GetComponent<MonsterAI>().AI_ON();
        monster1.GetComponent<Transform>().position = new Vector3(11.2f, 0, 38);
        monster1.GetComponent<MonsterAI>().MonsterMove(new Vector3(11.2f, 0, 35), false);
        playerCamera.SetActive(true);
        scene1Camera.SetActive(false);
        stopCutScene();
        uIManager.setQuestText("손전등이 필요하다.");
        yield break;
    }
    IEnumerator Scene2() // 2: 길 모퉁이에 몬스터가 서 있는 것을 보여줌
    {
        startCutScene();
        playerCamera.SetActive(false);
        scene2Camera.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        scene2Camera.SetActive(false);
        playerCamera.SetActive(true);
        stopCutScene();
        uIManager.setQuestText("조용히 이동해야 한다.\n웅크리기: L Ctrl");
        yield break;
    }
    IEnumerator Scene3() // 3: 소화전이 오작동하여 울림, 몬스터가 쫓아옴.
    {
        startCutScene();
        playerCamera.SetActive(false);
        shutter1Camera.SetActive(true);
        monster1.GetComponent<MonsterAI>().AI_OFF();
        yield return new WaitForSeconds(1f);
        fireBoxLight.SetActive(true);


        yield return new WaitForSeconds(3f);
        monster1.GetComponent<MonsterAI>().StartScene3Corutine();
        // 몬스터 이동
        // 몬스터 포효
        // 몬스터 추적
        yield return new WaitForSeconds(3.5f);
        playerCamera.SetActive(true);
        shutter1Camera.SetActive(false);

        gameManager.player.transform.position = new Vector3(-16.2f, gameManager.player.transform.position.y, 38.6f);
        stopCutScene();
        uIManager.setQuestText("달리기: L Shift");
        yield return new WaitForSeconds(7f);
        fireBoxLight.SetActive(false);
        yield break;
    }
    IEnumerator Scene4() // 4: 모퉁이에서 몬스터가 지나감. 
    {
        startCutScene();
        playerCamera.SetActive(false);
        scene4Camera.SetActive(true);
        monster2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-28f, 0, -1.175f), false);
        yield return new WaitForSeconds(2f);
        monster2.GetComponent<MonsterAI>().MonsterMove(new Vector3(-22.36f, 0, -27.806f), false);
        monster2.GetComponent<MonsterAI>().holdPos = new Vector3(-22.36f, 0, -27.806f);
        yield return new WaitForSeconds(2f);
        monster2.GetComponent<MonsterAI>().AI_ON();
        playerCamera.SetActive(true);
        scene4Camera.SetActive(false);
        stopCutScene();
        yield break;
    }

    IEnumerator CatchScene() // 몬스터에게 잡혔을때 컷신
    {
        StopCoroutine(Scene3());
        shutter1Camera.SetActive(false);
        startCutScene();
        playerCamera.SetActive(false);
        catchedCamera.SetActive(true);
        yield return new WaitForSeconds(6.7f);
        //playerCamera.SetActive(true); // <<== 해당 부분 추후에 게임 오버 로직으로 전환할 것
        //catchedCamera.SetActive(false);
        gameManager.mouseControlForUI();
        SystemControl.GoToDeadScene();
        //stopCutScene();
    }

    IEnumerator ShutterScene2()
    {
        startCutScene();
        yield return new WaitForSeconds(0.3f);

        playerCamera.SetActive(false);
        shutter2Camera.SetActive(true);

        yield return new WaitForSeconds(1f);

        moveICS(shutter2pos.transform.position, 1f, shutter2);

        yield return new WaitForSeconds(5);

        playerCamera.SetActive(true);
        shutter2Camera.SetActive(false);
        Destroy(shutter2);
        Destroy(shutter2pos);
        Destroy(shutter2Camera);

        yield return new WaitForSeconds(0.3f);
        stopCutScene();
    }
    IEnumerator ShutterScene3()
    {
        startCutScene();
        yield return new WaitForSeconds(0.3f);

        playerCamera.SetActive(false);
        shutter3Camera.SetActive(true);

        yield return new WaitForSeconds(1f);

        moveICS(shutter3pos.transform.position, 1f, shutter3);

        yield return new WaitForSeconds(5);

        playerCamera.SetActive(true);
        shutter3Camera.SetActive(false);
        Destroy(shutter3);
        Destroy(shutter3pos);
        Destroy(shutter3Camera);

        yield return new WaitForSeconds(0.3f);
        stopCutScene();
    }
    IEnumerator ShutterScene4()
    {
        startCutScene();
        yield return new WaitForSeconds(0.3f);

        playerCamera.SetActive(false);
        shutter4Camera.SetActive(true);

        yield return new WaitForSeconds(1f);

        moveICS(shutter4pos.transform.position, 1f, shutter4);

        yield return new WaitForSeconds(5);

        playerCamera.SetActive(true);
        shutter4Camera.SetActive(false);
        Destroy(shutter4);
        Destroy(shutter4pos);
        Destroy(shutter4Camera);

        yield return new WaitForSeconds(0.3f);
        stopCutScene();
    }
    IEnumerator ShutterScene5()
    {
        startCutScene();
        yield return new WaitForSeconds(0.3f);

        playerCamera.SetActive(false);
        shutter5Camera.SetActive(true);

        yield return new WaitForSeconds(1f);

        moveICS(shutter5pos.transform.position, 1f, shutter5);

        yield return new WaitForSeconds(5);

        playerCamera.SetActive(true);
        shutter5Camera.SetActive(false);
        Destroy(shutter5);
        Destroy(shutter5pos);
        Destroy(shutter5Camera);

        yield return new WaitForSeconds(0.3f);
        stopCutScene();
    }

    // 컷신 외, 특수 메소드들
    IEnumerator moveCorutine(Vector3 dest, float moveSpeed, GameObject movingobj)
    {
        while (true)
        {
            if (Vector3.Distance(movingobj.transform.position, dest) < 0.1f) // 목표지점으로 이동시 코루틴 종료
            {
                yield break;
            }
            movingobj.transform.position = Vector3.MoveTowards(movingobj.transform.position, dest, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public void StartShutterScene1() // 셔터1 열기 시작 [컷신 아님!!!] 
    {
        if (!shutter1IsOpen)
        {
            shutter1IsOpen = true;
            StartCoroutine(ShutterScene1());
        }
    }
    IEnumerator ShutterScene1() // 셔터1 열기 [컷신 아님!!!]
    {
        shutter1.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);
        moveICS(shutter1pos.transform.position, 4f, shutter1);
        yield return new WaitForSeconds(2);
        shutter1.SetActive(false);
        yield break;
    }
    public void PassCutScene0()// 프젝 끝나고 지울것!!!
    {
        playerCamera.SetActive(true);
        scene0Camera.SetActive(false);
        scene0Object.SetActive(false);
        uIManager.offCutSceneUI();
        gameManager.nowCutScene = false;
        gameManager.playerOutCutScene();
        StopCoroutine(Scene0());
        Debug.Log("stop0");
    }

    public void LoadData()
    {
        for (int i = 0; i < 5 ; i++) // 씬 늘어날때마다 i 최대값 늘릴것
        {
            sceneNum[i] = dataBase.UserData.CutSceneCheck[i];
        }
    }
}
