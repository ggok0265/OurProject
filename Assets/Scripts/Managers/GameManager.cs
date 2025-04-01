using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SoundManager soundManager;

    public GameObject player;
    public CutSceneManager CSM;
    public DataBase database;
    public GameObject MDoor1;

    public bool nowCutScene; // ���� �ƽ��� ���� ������ ����

    public bool canPlayerControl; // �÷��̾� ��Ʈ�� ���� ����
    public bool sightControlMode; // �÷��̾� �þ� ��Ʈ�� ���

    public bool[] missionCheck; //���̴� ����Ƽ������ ����. / ~ ��° �̼��� Ŭ���� �ߴ��� ���� 
    /* 
     * 0: ī��Ű�� �� ���� ������
     * 1: �����Ҹ� ���̰� ���� ���� ��������
     * 2: ��ȭ���� �︮�� �����κ��� Ż���ϱ� 
     * 3: �����ϰ� ����ǿ� �����ϱ�
     * 4: �������� ĳ��� ����
     * 5: ���� ������ ������
     * 6: ��ĳ��
     * 7: Ű�е� ����
     * 8: ���� ����
     */

    void Start()
    {
        playerOutCutScene();
        CSM = gameObject.GetComponent<CutSceneManager>();
        database = gameObject.GetComponent<DataBase>();
        soundManager = gameObject.GetComponent<SoundManager>();

        Invoke("forInvokeLoad", 0.01f);
    }

    void forInvokeLoad()
    {
        if (SystemControl.needLoading)
        {
            database.LoadBtn();
        }
        if (!CSM.sceneNum[0])
            CSM.StartCutScene(0);
        else
        {
            CSM.scene0Camera.SetActive(false);
            CSM.playerCamera.SetActive(true);
        }
    }

    public void mouseControlForPlayer() // �÷��̾� ������ ���� ���콺 ���� �� Ŀ�� �����
    {
        sightControlMode = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void mouseControlForUI() // UI ������ ���� ���콺 ���� ���� �� Ŀ�� ���̱�
    {
        sightControlMode = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void playerInCutScene() // �ƽ� ���� �� �÷��̾� ���� 
    {
        canPlayerControl = false;
        sightControlMode = false;
        player.GetComponent<PlayerMove>().isWalkSound = false;
        player.GetComponent<PlayerMove>().playerFootSound.SetActive(false);
        player.GetComponent<PlayerMove>().playerRunSound.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void playerOutCutScene() // �ƽ� ������ �� �÷��̾� ���� 
    {
        canPlayerControl = true;
        sightControlMode = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadData()
    {
        soundManager.MusicPlay();
        missionCheck = database.UserData.MissionCheck;
        if (CSM.sceneNum[0])
        {
            CSM.PassCutScene0();
        }
        if (missionCheck[0]) MDoor1.GetComponent<Animator>().SetTrigger("Open");
    }
}