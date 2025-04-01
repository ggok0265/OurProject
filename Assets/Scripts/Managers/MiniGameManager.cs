using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerAction playerAction;
    public PlayerMove playerMove;

    public GameObject playerCamera;
    public GameObject lockPickCamera;
    public GameObject lockPickStartPos;
    public GameObject lockPickObj;
    public GameObject[] lockObj;
    public GameObject[] lockSafeCol;
    public GameObject[] lockPosTop;
    public GameObject[] lockPosBottom;

    public bool isLockPicking;

    Vector3 moveVec;
    float hAxis;
    public float speed = 3f;
    public int[] loc; //top = 0, bottom = 1;
    public float[] ran;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        for (int i = 0; i < 4; i++)
            ran[i] = Random.Range(7f, 9f);
    }
    void Update()
    {
        if (isLockPicking)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            moveVec = (transform.right * hAxis + transform.forward * 0).normalized;
            lockPickObj.transform.position += moveVec * speed * Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                closeLockPick();
            }

            for (int i = 0; i < 4; i++)
            {
                if (loc[i] == 0)
                {
                    lockObj[i].transform.position = Vector3.MoveTowards(lockObj[i].transform.position, lockPosBottom[i].transform.position, ran[i] * Time.deltaTime);
                }
                else if (loc[i] == 1)
                {
                    lockObj[i].transform.position = Vector3.MoveTowards(lockObj[i].transform.position, lockPosTop[i].transform.position, ran[i] * Time.deltaTime);
                }

                if (Vector3.Distance(lockObj[i].transform.position, lockPosTop[i].transform.position) < 0.1f)
                {
                    loc[i] = 0;
                }
                else if (Vector3.Distance(lockObj[i].transform.position, lockPosBottom[i].transform.position) < 0.1f)
                {
                    loc[i] = 1;
                }
            }
        }
    }
    public void onLockPick()
    {
        StartCoroutine("onLockPickScene");
    }
    public void offLockPick()
    {
        gameManager.missionCheck[4] = true;
        gameManager.GetComponent<GameManager>().database.SaveBtn();
        StartCoroutine("offLockPickScene");
    }
    public void closeLockPick()
    {
        if(isLockPicking)
        {
            StartCoroutine("closeLockPickScene");
        }
    }

    IEnumerator onLockPickScene()
    {
        isLockPicking = true;

        GetComponent<UIManager>().playerSightUI.SetActive(false);


        playerMove.playerFootSound.SetActive(false);
        playerMove.playerRunSound.SetActive(false);
        yield return new WaitForSeconds(0.2f);

        playerCamera.SetActive(false);
        lockPickCamera.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator offLockPickScene()
    {
        yield return new WaitForSeconds(0.2f);
        playerAction.pointedobj.GetComponent<BoxOpen>().isLocked = false;
        playerAction.inventory.removeItem(3); // 락픽 제거
        lockPickCamera.SetActive(false);
        playerCamera.SetActive(true);
        isLockPicking = false;

        GetComponent<UIManager>().playerSightUI.SetActive(true);
        gameManager.mouseControlForPlayer();

        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator closeLockPickScene()
    {
        yield return new WaitForSeconds(0.2f);
        lockPickCamera.SetActive(false);
        playerCamera.SetActive(true);
        isLockPicking = false;

        GetComponent<UIManager>().playerSightUI.SetActive(true);
        gameManager.mouseControlForPlayer();

        lockPickFail();
        yield return new WaitForSeconds(0.1f);
    }

    public void lockPickFail()
    {
        for (int i = 0; i < 4; i++)
        {
            lockSafeCol[i].SetActive(true);
            ran[i] = Random.Range(7f, 9f);
        }
        lockPickObj.transform.position = lockPickStartPos.transform.position;
    }
}
