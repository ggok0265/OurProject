using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutSceneManager : MonoBehaviour
{
    // <�ƽſ��� ĳ���� �̵�>
    public void moveICS(Vector3 destination, float moveSpeed) // �ڷ�ƾ �ߵ�
    {
        Vector3 dest = new Vector3(destination.x, transform.position.y, destination.z);
        transform.rotation = Quaternion.LookRotation(dest - transform.position).normalized; // ��ǥ�������� �ü� �̵�
        StartCoroutine(moveCorutine(dest, moveSpeed));
    }
    IEnumerator moveCorutine(Vector3 dest, float moveSpeed)
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, dest) < 0.1f) // ��ǥ�������� �̵��� �ڷ�ƾ ����
            {
                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, dest, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void teleportICS(Vector3 destination)
    {
        Vector3 dest = new Vector3(destination.x, destination.y, destination.z);
        transform.position = dest;
    }

    // <�ƽſ����� �ִϸ��̼�>
}
