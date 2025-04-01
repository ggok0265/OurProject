using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    public bool isLocked; // ���� ����ִ��� ����
    public bool isOpen; // ���� �����ִ��� ����
    public bool isMoving; // ���� ���������� ����
    private Vector3 originalPos; // �������� > start���� �ڵ� ����
    
    // Ÿ�� ����
    public bool doorType; // ������ ������ < ����Ƽ ������ üũ
    public bool drawerType; // ������ ������ < ����Ƽ ������ üũ

    // ����Ÿ��
    private bool doorOpen; // �����ϴ� ����� �̸�
    private bool doorClose; // �����ϴ� ����� �̸�
    public bool right; // ��� �������� �������� < ����Ƽ���� üũ
    public bool left; // ��� �������� �������� < ����Ƽ���� üũ
    private Animator anime;
    private string animeName;

    // ���� Ÿ��
    private bool drawerOpen; // �����ϴ� ����� �̸�
    private bool drawerClose; // �����ϴ� ����� �̸�
    public bool xLine;
    public bool zLine;
    public float dist;
    private Vector3 movePos; // ��ǥ����

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