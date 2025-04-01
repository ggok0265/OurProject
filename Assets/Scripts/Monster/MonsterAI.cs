using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    // ������Ʈ
    public GameObject Managers;
    private Animator anime;
    public NavMeshAgent navMeshAgent;
    public GameObject player;
    public GameObject catchCamera;
    private MobSound mobSound;
    public GameObject pointer;
    public CatchZone catchZone;

    // �̵�
    public Vector3 destinationVec;
    public float moveSpeed; // �̵��ӵ�

    // �÷��̾� ���� (�ð�)
    public float sightDistance;
    float sightAngle = 50f; // �þ߰� ����
    public bool onSightRange; // �þ� ���� ���� �÷��̾ ���� 

    public float distanceFromPlayer; // �÷��̾���� �Ÿ�
    private Ray trackRay;
    private Ray bottomRay; // �� ������ �����ɽ�Ʈ
    public RaycastHit trackHit;
    public RaycastHit bottomHit; // �� ������ �����ɽ�Ʈ
    public GameObject trackRayPos;
    public GameObject bottomRayPos; // �� ������ �����ɽ�Ʈ ������
    public GameObject officeDoorManager; // �繫�� �� �Ŵ���
    public GameObject pantryDoorManager; // ����� �� �Ŵ���
    public GameObject archiveRoomDoorManager; // ����� �� �Ŵ���
    public bool onTracker;

    // �÷��̾� ���� (û��)
    public bool comfort; // 0 ~ 3 ����
    public bool suspect; // 4 ~ 7 
    public bool mad; // 8 ~ 10
    public float stress; // ��Ʈ���� ��
    
    public float audibleDistance; // ��û �Ÿ�

    public Vector3 soundPos; // �Ҹ��� �� ��ġ
    private float temporaryStressAmount;
    private float stressTic; // ��Ʈ���� ���� �ӵ�

    // �Ҹ��� ���� �ൿ
    public int backUpLevel = 1; // ƽ ������ ���� ��Ʈ���� �� ���
    public float actTic; // �ൿ ƽ;

    // AI ���� ��
    public bool onAI; // AI Ȱ��ȭ ����
    
    public bool normal; // ���
    public bool capture; // �þ߿� ����

    private bool chaseTrigger; // ���� ������ �������ֱ� ���� ����
    public bool isChasing; // �÷��̾� �߰� �� ������
    public float chaseTime; // �Ѿư��� �ð�

    public bool isCatch; // �÷��̾ ������

    public Vector3 holdPos; // ����� ��ġ
    private float backToHoldTimer; // ��ġ ����Ϸ� ���� ��Ÿ�� 

    // �Ϲ� ���� ��
    private bool idle;
    private bool walk;
    private bool run;

    //�� �μ��� ����
    private Vector3 savePos;
    public bool doorDetected; // ���濡 �� ����
    public bool goDoorCrash; // �� �μ��� ����.
    public bool doorCrashTrigger;
    public bool onDoorFront; // �� �տ� ����
    public bool onDoorBack; // �� �ڿ� ����
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
        MovePointer(); // AI�� ���� ���� Pointer�� ��ġ�� ���� �������� �ű�
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
            DoorBreakOrder(); // �� �ı� �Ŵ����κ��� ������ �޾ƿ� 
        }
    }
    
    public void ReStartSetting() // ���ν� �ʱ�ȭ �Ǵ� ��
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
    
    // <<AI ����>>
    // �÷��̾� ����
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
    private void TrackerLookPlayer() // Ʈ��Ŀ�� �÷��̾� ����
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
    private void IsTargetInSight() // �÷��̾ �þ� ������ �ִ��� üũ
    {
        //Ÿ���� ���� 
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, targetDir);

        //������ �̿��� �� ����ϱ�
        // thetha = cos^-1( a dot b / |a||b|)
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Debug.Log("Ÿ�ٰ� AI�� ���� : " + theta);
        if (theta <= sightAngle && distanceFromPlayer <= sightDistance)
        {
            onSightRange = true;
        }
        else
        {
            onSightRange = false;
        }
    }
    private void SightRayCast() // ����ĳ��Ʈ
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

    private void StressLevelUpdater() // ��Ʈ���� ���� Update ����
    {
        if(stress < 4)
        {
            comfort = true; // 0 ~ 3 ����
            suspect = false; // 4 ~ 7 
            mad = false; // 8 ~ 10
            audibleDistance = 8;
        }
        else if (stress < 8)
        {
            comfort = false; // 0 ~ 3 ����
            suspect = true; // 4 ~ 7 
            mad = false; // 8 ~ 10
            audibleDistance = 7;
        }
        else if (stress <= 10)
        {
            comfort = false; // 0 ~ 3 ����
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
                    stress += temporaryStressAmount; //stress ���ϱ�
                }
            }
            stressTic = 0.2f;
            temporaryStressAmount = 0;
        }
    }
    
    private void Listening()
    {
        if(distanceFromPlayer < audibleDistance && !isChasing) // ��û�Ÿ� ���� ���� ���
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
    public void acceptNoise(Vector3 pos, int soundLevel) // �ܺο��� ȣ�� ����
    {
        float soundDistance;
        float addStress;
        soundDistance = Vector3.Distance(transform.position, pos); // �Ҹ��� �� ���� ���� ������ �Ÿ� ���
        addStress = 0.5f * ((2 * soundLevel) / ((soundDistance)*(soundDistance)*(soundDistance))); // ��Ʈ���� ���� �� ���
        temporaryStressAmount += addStress; // ��Ʈ���� ����
        soundPos = pos; // �Ҹ��� �� ��ǥ ����
        actBySound(); // ���� ��Ʈ���� ���� ���� �ൿ ����
    }


    // �Ǵ�
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

    // <�ൿ��>
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

    private void BackToHoldPos() // ���� ���� ������ �ٽ� ���ڸ��� ���ư�.
    {   if(backToHoldTimer > 0)
        {
            backToHoldTimer -= Time.deltaTime;
        }
        if(holdPos != new Vector3(0,0,0) && backToHoldTimer <= 0 && !isChasing && !isCatch && !capture && !doorCrashTrigger)
        {
            MonsterMove(holdPos, false);
        }
    }

    // ��ɵ�
    public void MovePointer() // �����͸� ��������
    {
        if(!officeDoorManager.GetComponent<DoorBreakManager>().goBreak ||
            !pantryDoorManager.GetComponent<DoorBreakManager>().goBreak)
        pointer.transform.position = destinationVec;
    }
    public void MonsterMove(Vector3 dest, bool isRun)
    {
        destinationVec = new Vector3(dest.x, transform.position.y, dest.z);
        idle = false;
        if (!isRun) // �ȱ�
        {
            walk = true;
            run = false;
            moveSpeed = 0.9f;
        }
        else // �ٱ�
        {
            walk = false;
            run = true;
            moveSpeed = 6f;
        }
        GetComponent<NavMeshAgent>().speed = moveSpeed;
        navMeshAgent.SetDestination(destinationVec);
    }
    public void MonsterStop() // �������� �������� �� �ڵ� ����.
    {   
        if(Vector3.Distance(destinationVec, transform.position) < 0.05f)
        {
            idle = true;
            walk = false;
            run = false;
        }
    }
    public void MonsterFoundPlayer() // �÷��̾� �߰� �� �ߵ��Ǵ� Ʈ���� 
    {
        if(!isChasing && capture && !chaseTrigger && !isCatch && !doorCrashTrigger)
        {
            chaseTrigger = true;
            Managers.GetComponent<SoundManager>().ChaseMusicPlay();
            StartCoroutine(FindAndRoar());
        }
    }
    IEnumerator FindAndRoar() // �÷��̾� �߰� ��, ��ȿ --> ���� ����.
    {
        destinationVec = transform.position; 
        navMeshAgent.SetDestination(destinationVec); // ���ڸ� ����
        Vector3 vec = player.transform.position - transform.position;
        vec.Normalize();                             
        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q; // �÷��̾ ���� ȸ��
        anime.SetTrigger("Mob_FoundPlayer"); // ��ȿ �ִϸ��̼� ����
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

    private void MonsterChase() // �÷��̾� ����
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
            else if (chaseTime <= 0)// ������ �ߴ�
            {
                isChasing = false;
                chaseTrigger = false;
                Managers.GetComponent<SoundManager>().MusicPlay();
            }
        }
    }
    public void MonsterCatch() // �÷��̾� ������
    {
        isCatch = true;
        chaseTime = 0;
        StopCoroutine(OrderInScene3());
        StopCoroutine(FindAndRoar());
        anime.SetTrigger("Mob_Catch");
        destinationVec = transform.position;
        navMeshAgent.SetDestination(destinationVec); // ���ڸ� ����
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

    public void MonsterCrashDoor() // �� �տ� ������ �� �μ��� ���� ����
    {
        if ((onDoorFront || onDoorBack) && !doorCrashTrigger && goDoorCrash && distFromDoor <= 2f) 
        {
            doorCrashTrigger = true;
            StartCoroutine(DoorCrashCorutine());
        }
    }
    IEnumerator DoorCrashCorutine() // �� �μ��� ���� �߿� �Ͼ�� �ϵ�
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
        navMeshAgent.SetDestination(destinationVec); // ���ڸ� ����
        Vector3 vec = door.transform.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q; // �÷��̾ ���� ȸ��
        anime.SetTrigger("Mob_DoorCrash");
        yield break;
    }
    public void MonsterCrashDoorEnd()
    {
        Debug.Log("CrashEnd");
        doorCrashTrigger = false;
        anime.SetTrigger("Mob_DoorCrashEnd");
        MonsterMove(savePos, true); // ����ߴ� ��ҷ� ������
    }
    public void DoorCrash() // �ִϸ��̼� �̺�Ʈ�� �۵�
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



    // �ƽ� �� ���� �� �ý����� ���� ������ ���
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
        navMeshAgent.SetDestination(destinationVec); // ���ڸ� ����
        Vector3 vec = player.transform.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        transform.rotation = q;
        Managers.GetComponent<SoundManager>().ChaseMusicPlay();
        anime.SetTrigger("Mob_FoundPlayer"); // ��ȿ �ִϸ��̼� ����
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
            Managers.GetComponent<GameManager>().database.SaveBtn(); // ����
        }
    }
}