using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutSceneManager : MonoBehaviour
{
    // <컷신에서 캐릭터 이동>
    public void moveICS(Vector3 destination, float moveSpeed) // 코루틴 발동
    {
        Vector3 dest = new Vector3(destination.x, transform.position.y, destination.z);
        transform.rotation = Quaternion.LookRotation(dest - transform.position).normalized; // 목표지점으로 시선 이동
        StartCoroutine(moveCorutine(dest, moveSpeed));
    }
    IEnumerator moveCorutine(Vector3 dest, float moveSpeed)
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, dest) < 0.1f) // 목표지점으로 이동시 코루틴 종료
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

    // <컷신에서의 애니메이션>
}
