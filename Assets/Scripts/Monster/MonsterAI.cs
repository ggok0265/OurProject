using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    // 컴포넌트
    public GameObject Managers;
    private Animator anime;
    public NavMeshAgent navMeshAgent;
    public GameObject player;
    public GameObject catchCamera;
    private MobSound mobSound;
    public GameObject pointer;
    public CatchZone catchZone;

    // 이동
    public Vector3 destinationVec;
    public float moveSpeed; // 이동속도

    // 플레이어 감지 (시각)
    public float sightDistance;
    float sightAngle = 50f; // 시야각 범위
    public bool onSightRange; // 시야 범위 내에 플레이어가 있음 

    public float distanceFromPlayer; // 플레이어와의 거리
    private Ray trackRay;
    private Ray bottomRay; // 문 감지용 레이케스트
    public RaycastHit trackHit;
    public RaycastHit bottomHit; // 문 감지용 레이케스트
    public GameObject trackRayPos;
    public GameObject bottomRayPos; // 문 감지용 레이케스트 포지션
    public GameObject officeDoorManager; // 사무실 문 매니저
    public GameObject pantryDoorManager; // 탕비실 문 매니저
    public GameObject archiveRoomDoorManager; // 탕비실 문 매니저
    public bool onTracker;

    // 플레이어 감지 (청각)
    public bool comfort; // 0 ~ 3 정지
    public bool suspect; // 4 ~ 7 
    public bool mad; // 8 ~ 10
    public float stress; // 스트레스 양
    
    public float audibleDistance; // 가청 거리

    public Vector3 soundPos; // 소리가 난 위치
    private float temporaryStressAmount;
    private float stressTic; // 스트레스 연산 속도

    // 소리에 의한 행동
    public int backUpLevel = 1; // 틱 보정을 위한 스트레스 값 백업
    public float actTic; // 행동 틱;

    // AI 관련 값
    public bool onAI; // AI 활성화 여부
    
    public bool normal; // 평시
    public bool capture; // 시야에 들어옴

    private bool chaseTrigger; // 추적 시작을 보조해주기 위한 변수
    public bool isChasing; // 플레이어 발견 후 추적중
    public float chaseTime; // 쫓아가는 시간

    public bool isCatch; // 플레이어를 붙잡음

    public Vector3 holdPos; // 사수할 위치
    private float backToHoldTimer; // 위치 사수하러 가는 쿨타임 

    // 일반 상태 값
    private bool idle;
    private bool walk;
    private bool run;

    //문 부수기 관련
    private Vector3 savePos;
    public bool doorDetected; // 전방에 문 감지
    public bool goDoorCrash; // 문 부수러 간다.
    public bool doorCrashTrigger;
    public bool onDoorFront; // 문 앞에 있음
    public bool onDoorBack; // 문 뒤에 있음
    public GameObject door;
    public float distFromDoor;

    void Start()
    {
        anime = GetComponent<Animator>();
        mobSound = GetComponent<MobSound>();
        catchZone = GetComponentInChildren<CatchZone>();
    }

    void Update()
    {
        MonsterStop();
        AnimeManage();
        MovePointer(); // AI가 켜진 동안 Pointer의 위치를 몹의 목적지로 옮김
        if (door != null)
        {
            distFromDoor = Vector3.Distance(transform.position, door.transform.position);
        }
        if (onAI)
        {
            IsTargetInSight();
            SightRayCast();
            MainSensor();
            MainAI();
            DoorBreakOrder(); // 문 파괴 매니저로부터 정보를 받아옴 
        }
    }
    
    public void ReStartSetting() // 세로시 초기화 되는 것
    {
        StopAllCoroutines();
        sightAngle = 40f;
        backUpLevel = 1;
        actTic = 0;
        stressTic = 0;
        stress = 0;
        onTracker = false;
        capture = false;
        chaseTrigger = false;
        chaseTime = 0;
        isCatch = false;
        isChasing = false;

        goDoorCrash = false;
        doorDetected = false;
        doorCrashTrigger = false;
        onDoorFront = false;
        onDoorBack = false;
    }
    
    // <<AI 관리>>
    // 플레이어 감지
    private void MainSensor()
    {
        DistanceUpdater();
        DetectFlashLight();
        TrackerLookPlayer();


        Listening();
        StressCalculation();
        StressLevelUpdater();
        actTicTimeManage();
        if (onTracker)
        {
            capture = true;
        }
        else
        {
            capture = false;
        }
    }
    private void DistanceUpdater()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }
    private void DetectFlashLight()
    {
        if(player.GetComponent<PlayerAction>().isflashing)
        {
            sightDistance = 40f;
            sightAngle = 60f;
        }
        else
        {
            sightDistance = 15f;
            sightAngle = 50f;
        }
    }
    private void TrackerLookPlayer() // 트래커가 플레이어 추적
    {
        Vector3 standTargetPos = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z);
        Vector3 crawlTargetPos = new Vector3(player.transform.position.x, player.transform.position.y - 0.2f, player.transform.position.z);
        Vector3 targetPos;
        if(player.GetComponent<PlayerMove>().isStanding)
        {
            targetPos = standTargetPos;
        }
        else
        {
            targetPos = crawlTargetPos;
        }
        Vector3 vec = targetPos - trackRayPos.transform.position; 
        vec.Normalize(); 
        Quaternion q = Quaternion.LookRotation(vec);
        trackRayPos.transform.rotation = q;
    }
    private void IsTargetInSight() // 플레이어가 시야 범위에 있는지 체크
    {
        //타겟의 방향 
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, targetDir);

        //내적을 이용한 각 계산하기
        // thetha = cos^-1( a dot b / |a||b|)
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Debug.Log("타겟과 AI의 각도 : " + theta);
        if (theta <= sightAngle && distanceFromPlayer <= sightDistance)
        {
            onSightRange = true;
        }
        else
        {
            onSightRange = false;
        }
    }
    private void SightRayCast() // 레이캐스트
    {
        if(onSightRange)
        {
            trackRay = new Ray(trackRayPos.transform.position, trackRayPos.transform.forward);
            Debug.DrawRay(trackRayPos.transform.position, trackRayPos.transform.forward * sightDistance, Color.red);
            if (Physics.Raycast(trackRay, out trackHit, sightDistance))
            {
                if (trackHit.transform.tag == "Player" && !trackHit.transform.GetComponent<PlayerAction>().hide)
                {
                    onTracker = true;
                }
                else
                {
                    onTracker = false;
                }
            }
            else
            {
                onTracker = false;
            }
        }
        else if(!onSightRange)
        {
            onTracker = false;
        }
    }

    private void StressLevelUpdater() // 스트레스 레벨 Update 돌림
    {
        if(stress < 4)
        {
            comfort = true; // 0 ~ 3 정지
            suspect = false; // 4 ~ 7 
            mad = false; // 8 ~ 10
            audibleDistance = 8;
        }
        else if (stress < 8)
        {
            comfort = false; // 0 ~ 3 정지
            suspect = true; // 4 ~ 7 
            mad = false; // 8 ~ 10
            audibleDistance = 7;
        }
        else if (stress <= 10)
        {
            comfort = false; // 0 ~ 3 정지
            suspect = false; // 4 ~ 7 
            mad = true; // 8 ~ 10
            audibleDistance = 6;
        }
    }
    private void StressCalculation()
    {
        if (stress < 0)
        {
            stress = 0;
        }
        
        if (stress > 10)
        {
            stress = 10;
        }

        if (stressTic > 0)
        {
            stressTic -= Time.deltaTime;
        }
        else if(stressTic <= 0)
        {
            if(stress >= 0 && stress <= 10)
            {
                if (temporaryStressAmount <= 0)
                {
                    stress -= 0.01f;
                }
                else
                {
                    stress += temporaryStressAmount; //stress 더하기
                }
            }
            stressTic = 0.2f;
            temporaryStressAmount = 0;
        }
    }
    
    private void Listening()
    {
        if(distanceFromPlayer < audibleDistance && !isChasing) // 가청거리 내에 있을 경우
        {
            if (player.GetComponent<PlayerMove>().isRun)
            {
                acceptNoise(player.transform.position, 3);
            }
            else if(player.GetComponent<PlayerMove>().isWalkSound)
            {
                acceptNoise(player.transform.position, 1);
            }
        }
    }
    public void acceptNoise(Vector3 pos, int soundLevel) // 외부에서 호출 가능
    {
        float soundDistance;
        float addStress;
        soundDistance = Vector3.Distance(transform.position, pos); // 소리가 난 곳과 몬스터 사이의 거리 계산
        addStress = 0.5f * ((2 * soundLevel) / ((soundDistance)*(soundDistance)*(soundDistance))); // 스트레스 증가 값 계산
        temporaryStressAmount += addStress; // 스트레스 증가
        soundPos = pos; // 소리가 난 좌표 저장
        actBySound(); // 현재 스트레스 값에 따른 행동 실행
    }


    // 판단
    private void MainAI()
    {
        if(distanceFromPlayer < 0.5f && !isChasing)
        {
            MonsterMove(player.transform.position, false);
        }
        MonsterFoundPlayer();
        MonsterChase();
        BackToHoldPos();
        MonsterCrashDoor();
    }

    // <행동들>
    private void actTicTimeManage()
    {
        if (actTic > 0)
        {
            actTic -= Time.deltaTime;
        }
    }
    private void actBySound()
    {
        actTicAdjust();
        if (actTic <= 0)
        {
            if(comfort)
            {
                actTic = 1;
                backUpLevel = 1;
            }
            else if (suspect && !doorCrashTrigger)
            {
                if(!Managers.GetComponent<GameManager>().missionCheck[2] && !chaseTrigger)
                {
                    Managers.GetComponent<SoundManager>().ChaseMusicPlay();
                    chaseTrigger = true;
                    StartCoroutine(FindAndRoar());
                }
                else
                {
                    MonsterMove(soundPos, false);
                    backToHoldTimer = 15f;
                    actTic = 4;
                    backUpLevel = 2;
                }
            }
            else if (mad && !doorCrashTrigger)
            {
                if (!Managers.GetComponent<GameManager>().missionCheck[2] && !chaseTrigger)
                {
                    Managers.GetComponent<SoundManager>().ChaseMusicPlay();
                    chaseTrigger = true;
                    StartCoroutine(FindAndRoar());
                }
                else
                {
                    MonsterMove(soundPos, true);
                    backToHoldTimer = 20f;
                    actTic = 3;
                    backUpLevel = 3;
                }
            }
        }
    }
    private void actTicAdjust()
    {
        int updatedLevel;
        if(comfort)
        {
            updatedLevel = 1;
        }
        else if (suspect)
        {
            updatedLevel = 2;
        }
        else
        {
            updatedLevel = 3;
        }

        if (backUpLevel < updatedLevel)
        {
            actTic = 0f;
        }
    }

    private void BackToHoldPos() // 보고 별거 없으면 다시 제자리로 돌아감.
    {   if(backToHoldTimer > 0)
        {
            backToHoldTimer -= Time.deltaTime;
        }
        if(holdPos != new Vector3(0,0,0) && backToHoldTimer <= 0 && !isChasing && !isCatch && !capture && !doorCrashTrigger)
        {
            MonsterMove(holdPos, false);
        }
    }

    // 명령들
    public void MovePointer() // 포인터를 목적지로
    {
        if(!officeDoorManager.GetComponent<DoorBreakManager>().goBreak ||
            !pantryDoorManager.GetComponent<DoorBreakManager>().goBreak)
        pointer.transform.position = destinationVec;
    }
    public void MonsterMove(Vector3 dest, bool isRun)
    {
        destinationVec = new Vector3(dest.x, transform.position.y, dest.z);
        idle = false;
        if (!isRun) // 걷기
        {
            walk = true;
            run = false;
            moveSpeed = 0.9f;
        }
        else // 뛰기
        {
            walk = false;
            run = true;
            moveSpeed = 6f;
        }
        GetComponent<NavMeshAgent>().speed = moveSpeed;
        navMeshAgent.SetDestination(destinationVec);
    }
    public void MonsterStop() // 목적지에 도달했을 시 자동 정지.
    {   
        if(Vector3.Distance(destinationVec, transform.position) < 0.05f)
        {
            idle = true;
            walk = false;
            run = false;
        }
    }
    public void MonsterFoundPlayer() // 플레이어 발견 시 발동되는 트리거 
    {
        if(!isChasing && capture && !chaseTrigger && !isCatch && !doorCrashTrigger)
        {
            chaseTrigger = true;
            Managers.GetComponent<SoundManager>().ChaseMusicPlay();
            StartCoroutine(FindAndRoar());
        }
    }
    IEnumerator FindAndRoar() // 플레이어 발견 후, 포효 --> 추적 시작.
    {
        destinationVec = transform.position; 
        navMeshAgent.SetDestination(destinationVec); // 제자리 정지
        Vector3 vec = player.transform.position - transform.position;
        vec.Normalize();                             
        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q; // 플레이어를 행해 회전
        anime.SetTrigger("Mob_FoundPlayer"); // 포효 애니메이션 실행
        mobSound.RoarSound();
        yield return new WaitForSeconds(1.6f);
        yield break;
    }
    public void MonsterStartChase()
    {
        if(onAI)
        {
            Debug.Log("Chase");
            chaseTime = 5;
            isChasing = true;
            anime.SetTrigger("Mob_RoarEnd");
        }
    }

    private void MonsterChase() // 플레이어 추적
    {
        if (isChasing)
        {
            if(chaseTime > 0)
            {
                MonsterMove(player.transform.position, true);
                if (!capture)
                {
                    chaseTime -= Time.deltaTime;
                }
            }
            else if (chaseTime <= 0)// 추적을 중단
            {
                isChasing = false;
                chaseTrigger = false;
                Managers.GetComponent<SoundManager>().MusicPlay();
            }
        }
    }
    public void MonsterCatch() // 플레이어 붙잡음
    {
        isCatch = true;
        chaseTime = 0;
        StopCoroutine(OrderInScene3());
        StopCoroutine(FindAndRoar());
        anime.SetTrigger("Mob_Catch");
        destinationVec = transform.position;
        navMeshAgent.SetDestination(destinationVec); // 제자리 정지
        AI_OFF();

        Managers.GetComponent<MiniGameManager>().closeLockPick();

        Managers.GetComponent<SoundManager>().CatchSoundPlay();
        Managers.GetComponent<CutSceneManager>().catchedCamera = catchCamera;
        Managers.GetComponent<CutSceneManager>().StartCatchScene();
    }
    public void DoorBreakOrder()
    {
        if (officeDoorManager.GetComponent<DoorBreakManager>().goBreak)
        {
            door = officeDoorManager.GetComponent<DoorBreakManager>().doorObj;
            if (!goDoorCrash)
            {
                savePos = destinationVec;
                bool nowRun;
                nowRun = run ? true : false;
                if (officeDoorManager.GetComponent<DoorBreakManager>().onFront)
                {
                    MonsterMove(officeDoorManager.GetComponent<DoorBreakManager>().frontVec, nowRun);
                }
                else
                {
                    MonsterMove(officeDoorManager.GetComponent<DoorBreakManager>().backVec, nowRun);
                }
                goDoorCrash = true;
            }
        }
        else if (pantryDoorManager.GetComponent<DoorBreakManager>().goBreak)
        {
            door = pantryDoorManager.GetComponent<DoorBreakManager>().doorObj;
            if (!goDoorCrash)
            {
                savePos = destinationVec;
                bool nowRun;
                nowRun = run ? true : false;
                if (pantryDoorManager.GetComponent<DoorBreakManager>().onFront)
                {
                    MonsterMove(pantryDoorManager.GetComponent<DoorBreakManager>().frontVec, nowRun);
                }
                else
                {
                    MonsterMove(pantryDoorManager.GetComponent<DoorBreakManager>().backVec, nowRun);
                }
                goDoorCrash = true;
            }
        }
        else if (archiveRoomDoorManager.GetComponent<DoorBreakManager>().goBreak)
        {
            door = archiveRoomDoorManager.GetComponent<DoorBreakManager>().doorObj;
            if (!goDoorCrash)
            {
                savePos = destinationVec;
                bool nowRun;
                nowRun = run ? true : false;
                if (archiveRoomDoorManager.GetComponent<DoorBreakManager>().onFront)
                {
                    MonsterMove(archiveRoomDoorManager.GetComponent<DoorBreakManager>().frontVec, nowRun);
                    Debug.Log("f");
                }
                else
                {
                    MonsterMove(archiveRoomDoorManager.GetComponent<DoorBreakManager>().backVec, nowRun);
                    Debug.Log("b");
                }
                goDoorCrash = true;
            }
        }
        else
            goDoorCrash = false;
    }

    public void MonsterCrashDoor() // 문 앞에 갔을때 문 부수는 동작 시행
    {
        if ((onDoorFront || onDoorBack) && !doorCrashTrigger && goDoorCrash && distFromDoor <= 2f) 
        {
            doorCrashTrigger = true;
            StartCoroutine(DoorCrashCorutine());
        }
    }
    IEnumerator DoorCrashCorutine() // 문 부수는 동작 중에 일어나는 일들
    {
        Debug.Log("CrashStart");
        moveSpeed = 0f;
        //anime.SetTrigger("Mob_RoarEnd");
        chaseTime = 0;
        if (onDoorFront)
        {
            Vector3 dVec = new Vector3(door.GetComponent<DoorSystem>().frontVec.x, transform.position.y, door.GetComponent<DoorSystem>().frontVec.z); 
            transform.position = dVec;
        }
        else if (onDoorBack)
        {
            Vector3 dVec = new Vector3(door.GetComponent<DoorSystem>().backVec.x, transform.position.y, door.GetComponent<DoorSystem>().backVec.z);
            transform.position = dVec;
        }
        destinationVec = transform.position;
        navMeshAgent.SetDestination(destinationVec); // 제자리 정지
        Vector3 vec = door.transform.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q; // 플레이어를 행해 회전
        anime.SetTrigger("Mob_DoorCrash");
        yield break;
    }
    public void MonsterCrashDoorEnd()
    {
        Debug.Log("CrashEnd");
        doorCrashTrigger = false;
        anime.SetTrigger("Mob_DoorCrashEnd");
        MonsterMove(savePos, true); // 기억했던 장소로 가보기
    }
    public void DoorCrash() // 애니메이션 이벤트로 작동
    {
        door.GetComponent<DoorSystem>().Crash(onDoorBack);
    }

    public void AI_ON()
    {
        onAI = true;   
    }
    public void AI_OFF()
    {
        onAI = false;
    }

    private void AnimeManage()
    {
        anime.SetBool("Mob_Idle", idle);
        anime.SetBool("Mob_Walk", walk);
        anime.SetBool("Mob_Run", run);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "JumpTrigger")
        {
            if (!Managers.GetComponent<GameManager>().missionCheck[2])
            {
                destinationVec = new Vector3(-31.73f, transform.position.y, 38.49f);
                idle = false;
                walk = false;
                run = false;
                anime.SetTrigger("Mob_Jump");
                moveSpeed = 15f;
                GetComponent<NavMeshAgent>().speed = moveSpeed;
                navMeshAgent.SetDestination(destinationVec);
                gameObject.GetComponent<MobSound>().ScreemSound();
            }
        }

        if(other.gameObject.name == "DoorZoneFront")
        {
            onDoorFront = true;
            onDoorBack = false;
        }
        if (other.gameObject.name == "DoorZoneBack")
        {
            onDoorBack = true;
            onDoorFront = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "DoorZoneFront")
        {
            onDoorFront = false;
        }
        if (other.gameObject.name == "DoorZoneBack")
        {
            onDoorBack = false;
        }
    }



    // 컷신 및 게임 내 시스템을 위한 별도의 명령
    public void StartScene3Corutine()
    {
        if(!isCatch)
        {
            StartCoroutine(OrderInScene3());
        }
    }
    IEnumerator OrderInScene3()
    {
        MonsterMove(new Vector3(9.6f, 0, 38.3f), true);
        AI_OFF();

        yield return new WaitForSeconds(1.2f);

        destinationVec = transform.position;
        navMeshAgent.SetDestination(destinationVec); // 제자리 정지
        Vector3 vec = player.transform.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q;
        Managers.GetComponent<SoundManager>().ChaseMusicPlay();
        anime.SetTrigger("Mob_FoundPlayer"); // 포효 애니메이션 실행
        mobSound.RoarSound();
        yield return new WaitForSeconds(1.7f);
        anime.SetTrigger("Mob_RoarEnd");
        MonsterMove(new Vector3(-26.851f, 0, 38.5f), true);
        yield break;
    }
    public void MonsterCrash()
    {
        if (!Managers.GetComponent<GameManager>().missionCheck[2] && !isCatch)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            anime.SetTrigger("Mob_Crash");
            isCatch = true;
            Managers.GetComponent<SoundManager>().MusicPlay();
            Managers.GetComponent<GameManager>().missionCheck[2] = true;
            Managers.GetComponent<UIManager>().setQuestText("");
            Managers.GetComponent<GameManager>().database.SaveBtn(); // 저장
        }
    }
}