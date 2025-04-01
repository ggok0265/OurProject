using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public MiniGameManager miniGameManager;
    public SoundManager soundManager;
    public PlayerAction playerAction;
    Rigidbody rigid;
    public GameObject playerFootSound;
    public GameObject playerRunSound;

    public float moveSpeed;
    public float runSpeed;
    public float crawlSpeed;
    public bool isRun; // 뛰는 중인지 여부
    public bool isWalkSound; // 걷는 소리가 나는지?
    public new Camera camera;
    public bool isStanding;
    public bool isCrawling;
    public GameObject standingCol;
    public GameObject standingCameraPoint;
    public GameObject crawlingCameraPoint;
    public GameObject leftRunPos;
    public GameObject rightRunPos;

    float hAxis;
    float vAxis;
    Vector3 moveVec;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        playerAction = GetComponent<PlayerAction>();
        //camera = GetComponentInChildren<Camera>();
        moveSpeed = 3;
        runSpeed = 5;
        crawlSpeed = 0.8f;
        isStanding = true;
        isCrawling = false;
    }

    void Update()
    {
        if (!miniGameManager.isLockPicking)
        {
            move();
            run();
            crawl();
        }

        playerFootSound.GetComponent<AudioSource>().volume = soundManager.playerVolume.value;
        playerRunSound.GetComponent<AudioSource>().volume = soundManager.playerVolume.value;
    }

    private void move()
    {
        if (gameManager.canPlayerControl)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");

            if(isCrawling)
            {
                moveVec = (transform.forward * vAxis).normalized;
            }
            else
            {
                moveVec = (transform.right * hAxis + transform.forward * vAxis).normalized;
            }

            if (moveVec.magnitude == 0f || isCrawling)
            {
                playerFootSound.SetActive(false);
                playerRunSound.SetActive(false);
                isWalkSound = false;
            }

            if (!isRun && !isCrawling)
            {
                playerRunSound.SetActive(false);
                transform.position += moveVec * moveSpeed * Time.deltaTime;
                if (moveVec.magnitude > 0f)
                {
                    playerFootSound.SetActive(true);
                    isWalkSound = true;
                }
                else
                {
                    playerFootSound.SetActive(false);
                    isWalkSound = false;
                }
            }
            else if (isRun && !isCrawling)
            {
                playerFootSound.SetActive(false);
                isWalkSound = false;
                transform.position += moveVec * runSpeed * Time.deltaTime;
                if (moveVec.magnitude > 0f)
                {
                    playerRunSound.SetActive(true);
                }
                else
                {
                    playerRunSound.SetActive(false);
                }
            }
            else if(!isRun && isCrawling)
            {
                transform.position += moveVec * crawlSpeed * Time.deltaTime;
            }
        }

    }

    private void run()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") < 0 || playerAction.hide)
        {
            isRun = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") > 0 && !playerAction.hide)
        {
            isRun = true;
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") > 0 && !playerAction.hide)
        {
            isRun = true;
        }

    }

    void crawl()
    {
        if (isStanding && Input.GetKeyDown(KeyCode.LeftControl) && !isRun)
        {
            standingCol.SetActive(false);
            StartCoroutine(cameraMoving(crawlingCameraPoint, 2.5f, isStanding));
        }
        else if ((isCrawling && Input.GetKeyDown(KeyCode.LeftControl)) && !playerAction.hide || (isCrawling && isRun))
        {
            standingCol.SetActive(true);
            StartCoroutine(cameraMoving(standingCameraPoint, 2.5f, isStanding));
        }
    }

    IEnumerator cameraMoving(GameObject cameraPos, float speed, bool stand)
    {
        while (true)
        {
            if (Vector3.Distance(camera.transform.position, cameraPos.transform.position) < 0.0001f) // 목표지점으로 이동시 코루틴 종료
            {
                if (stand)
                {
                    isStanding = false;
                    isCrawling = true;
                }
                else
                {
                    isStanding = true;
                    isCrawling = false;
                }
                yield break;
            }
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, cameraPos.transform.position, speed * Time.deltaTime);
            yield return null;
        }
    }

    /*IEnumerator runCameraMoving()
    {
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, leftRunPos.transform.position, 10f * Time.deltaTime);
        yield return new WaitForSeconds(0.2f);
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, rightRunPos.transform.position, 10f * Time.deltaTime);
        yield return null;
    }*/
}

