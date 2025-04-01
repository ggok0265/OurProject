using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    public bool isLocked; // 현재 잠겨있는지 여부
    public bool isOpen; // 현재 열려있는지 여부
    public bool isMoving; // 현재 동작중인지 여부
    private Vector3 originalPos; // 시작지점 > start에서 자동 설정
    
    // 타입 설정
    public bool doorType; // 도어형 수납장 < 유니티 내에서 체크
    public bool drawerType; // 서랍형 수납장 < 유니티 내에서 체크

    // 도어타입
    private bool doorOpen; // 실행하는 명령의 이름
    private bool doorClose; // 실행하는 명령의 이름
    public bool right; // 어느 방향으로 열리는지 < 유니티에서 체크
    public bool left; // 어느 방향으로 열리는지 < 유니티에서 체크
    private Animator anime;
    private string animeName;

    // 서랍 타입
    private bool drawerOpen; // 실행하는 명령의 이름
    private bool drawerClose; // 실행하는 명령의 이름
    public bool xLine;
    public bool zLine;
    public float dist;
    private Vector3 movePos; // 목표지점

    void Start()
    {
        anime = gameObject.GetComponentInParent<Animator>();
        isOpen = false;
        isMoving = false;
        originalPos = gameObject.transform.localPosition;
        if(drawerType)
        {
            if (xLine)
            {
                movePos = new Vector3(transform.localPosition.x + dist, transform.localPosition.y, transform.localPosition.z);
            }
            else if(zLine)
            {
                movePos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + dist);
            }
        }

        if(right)
        {
            animeName = "Right";
        }
        else if (!right)
        {
            animeName = "Left";
        }
    }

    void Update()
    {
        if(isMoving)
        {
            if(doorOpen)
            {
                anime.SetTrigger(animeName + "Open");
                doorOpen = false;
                isMoving = false;
                isOpen = true;
            }
            else if (drawerOpen)
            {
                if (Vector3.Distance(transform.localPosition, movePos) < 0.001f)
                {
                    drawerOpen = false;
                    isMoving = false;
                    isOpen = true;
                }
                transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, movePos, 0.5f * Time.deltaTime);
            }
            else if (doorClose)
            {
                anime.SetTrigger(animeName + "Close");
                doorClose = false;
                isMoving = false;
                isOpen = false;
            }
            else if (drawerClose)
            {
                if (Vector3.Distance(transform.localPosition, originalPos) < 0.001f)
                {
                    drawerClose = false;
                    isMoving = false;
                    isOpen = false;
                }
                transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, originalPos, 0.5f * Time.deltaTime);
            }
        }
    }

    public void Operate()
    {
        if(!isMoving && !isLocked)
        {
            if (isOpen)
            {
                if (doorType)
                {
                    isMoving = true;
                    doorClose = true;
                }
                else if (drawerType)
                {
                    isMoving = true;
                    drawerClose = true;
                }
            }
            else if (!isOpen)
            {
                if (doorType)
                {
                    isMoving = true;
                    doorOpen = true;
                }
                else if (drawerType)
                {
                    isMoving = true;
                    drawerOpen = true;
                }
            }
        }
    }
    //transform.position = Vector3.MoveTowards(gameObject.transform.position, movePos, 0.1f);
}