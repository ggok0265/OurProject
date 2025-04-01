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

    public bool nowCutScene; // 현재 컷신이 실행 중인지 여부

    public bool canPlayerControl; // 플레이어 컨트롤 가능 여부
    public bool sightControlMode; // 플레이어 시야 컨트롤 모드

    public bool[] missionCheck; //길이는 유니티내에서 지정. / ~ 번째 미션을 클리어 했는지 여부 
    /* 
     * 0: 카드키로 문 열고 나가기
     * 1: 걸음소리 죽이고 괴물 몰래 지나가기
     * 2: 소화전이 울리고 괴물로부터 탈출하기 
     * 3: 안전하게 탕비실에 도착하기
     * 4: 락픽으로 캐비넷 열기
     * 5: 문서 보관실 들어오기
     * 6: 스캐너
     * 7: 키패드 열기
     * 8: 엔딩 보기
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

    public void mouseControlForPlayer() // 플레이어 조작을 위한 마우스 고정 및 커서 숨기기
    {
        sightControlMode = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void mouseControlForUI() // UI 조작을 위한 마우스 고정 해제 및 커서 보이기
    {
        sightControlMode = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void playerInCutScene() // 컷신 중일 때 플레이어 설정 
    {
        canPlayerControl = false;
        sightControlMode = false;
        player.GetComponent<PlayerMove>().isWalkSound = false;
        player.GetComponent<PlayerMove>().playerFootSound.SetActive(false);
        player.GetComponent<PlayerMove>().playerRunSound.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void playerOutCutScene() // 컷신 끝났을 때 플레이어 설정 
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