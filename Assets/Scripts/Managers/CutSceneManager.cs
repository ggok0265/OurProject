using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uIManager;
    public PlayerCutSceneManager playerCutSceneManager;
    public DataBase dataBase;

    //�ƽ� ���� ������Ʈ
    public GameObject playerCamera; // �÷��̾� ī�޶�
    
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
    public GameObject catchedCamera; // ������ ���������� ī�޶�. �ش� �����κ��� �Ҵ����
    
    public void startCutScene() // �ƽ� ���� ��, �̷�������ϴ� ��ɵ�
    {
        uIManager.onCutSceneUI();
        gameManager.nowCutScene = true;
        gameManager.playerInCutScene();
    }
    public void stopCutScene() // �ƽ� ���� ��, �̷�������ϴ� ��ɵ�
    {
        uIManager.offCutSceneUI();
        gameManager.nowCutScene = false;
        gameManager.playerOutCutScene();
    }
    public void moveICS(Vector3 destination, float moveSpeed, GameObject movingobj) // �ڷ�ƾ �ߵ�
    {
        Vector3 dest = new Vector3(destination.x, destination.y, destination.z);
        StartCoroutine(moveCorutine(dest, moveSpeed, movingobj));
    }

    //<�ƽ� ����Ʈ> �����ߴ��� ����
    public bool[] sceneNum = new bool[5]; 
    /* 
     * 0: �÷��̾ �����ǿ��� �Ͼ.
     * 1: ���Ͱ� �÷��̾ ����������.
     * 2: ���Ͱ� �� �����̿� �ִ� ���� ������
     * 3: ��ȭ���� �︮�� ���Ͱ� �Ѿƿ�.
     * 4: �����̿��� ������ Ƣ���.
     */ 
    
    public bool shutterScene2;
    public bool shutterScene3;
    public bool shutterScene4;
    public bool shutterScene5;

    //<�ƽ� ����>
    public void StartCutScene(int startNum) // ���丮 ������� ������ �ƽ� ����
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
    
    public void StartCatchScene() // �������� ������ �� �ƽ�
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

    //<�ƽ� ����>
    // ���丮 ������� ������ �ƽ�
    IEnumerator Scene0() // 0: �÷��̾ �����ǿ��� �Ͼ.
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
        uIManager.setQuestText("�̵�: W A S D");
        yield break;
    }
    IEnumerator Scene1() // 1: ���Ͱ� �÷��̾ ����������. 
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
        uIManager.setQuestText("�������� �ʿ��ϴ�.");
        yield break;
    }
    IEnumerator Scene2() // 2: �� �����̿� ���Ͱ� �� �ִ� ���� ������
    {
        startCutScene();
        playerCamera.SetActive(false);
        scene2Camera.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        scene2Camera.SetActive(false);
        playerCamera.SetActive(true);
        stopCutScene();
        uIManager.setQuestText("������ �̵��ؾ� �Ѵ�.\n��ũ����: L Ctrl");
        yield break;
    }
    IEnumerator Scene3() // 3: ��ȭ���� ���۵��Ͽ� �︲, ���Ͱ� �Ѿƿ�.
    {
        startCutScene();
        playerCamera.SetActive(false);
        shutter1Camera.SetActive(true);
        monster1.GetComponent<MonsterAI>().AI_OFF();
        yield return new WaitForSeconds(1f);
        fireBoxLight.SetActive(true);


        yield return new WaitForSeconds(3f);
        monster1.GetComponent<MonsterAI>().StartScene3Corutine();
        // ���� �̵�
        // ���� ��ȿ
        // ���� ����
        yield return new WaitForSeconds(3.5f);
        playerCamera.SetActive(true);
        shutter1Camera.SetActive(false);

        gameManager.player.transform.position = new Vector3(-16.2f, gameManager.player.transform.position.y, 38.6f);
        stopCutScene();
        uIManager.setQuestText("�޸���: L Shift");
        yield return new WaitForSeconds(7f);
        fireBoxLight.SetActive(false);
        yield break;
    }
    IEnumerator Scene4() // 4: �����̿��� ���Ͱ� ������. 
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

    IEnumerator CatchScene() // ���Ϳ��� �������� �ƽ�
    {
        StopCoroutine(Scene3());
        shutter1Camera.SetActive(false);
        startCutScene();
        playerCamera.SetActive(false);
        catchedCamera.SetActive(true);
        yield return new WaitForSeconds(6.7f);
        //playerCamera.SetActive(true); // <<== �ش� �κ� ���Ŀ� ���� ���� �������� ��ȯ�� ��
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

    // �ƽ� ��, Ư�� �޼ҵ��
    IEnumerator moveCorutine(Vector3 dest, float moveSpeed, GameObject movingobj)
    {
        while (true)
        {
            if (Vector3.Distance(movingobj.transform.position, dest) < 0.1f) // ��ǥ�������� �̵��� �ڷ�ƾ ����
            {
                yield break;
            }
            movingobj.transform.position = Vector3.MoveTowards(movingobj.transform.position, dest, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public void StartShutterScene1() // ����1 ���� ���� [�ƽ� �ƴ�!!!] 
    {
        if (!shutter1IsOpen)
        {
            shutter1IsOpen = true;
            StartCoroutine(ShutterScene1());
        }
    }
    IEnumerator ShutterScene1() // ����1 ���� [�ƽ� �ƴ�!!!]
    {
        shutter1.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.5f);
        moveICS(shutter1pos.transform.position, 4f, shutter1);
        yield return new WaitForSeconds(2);
        shutter1.SetActive(false);
        yield break;
    }
    public void PassCutScene0()// ���� ������ �����!!!
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
        for (int i = 0; i < 5 ; i++) // �� �þ������ i �ִ밪 �ø���
        {
            sceneNum[i] = dataBase.UserData.CutSceneCheck[i];
        }
    }
}
