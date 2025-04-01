using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPick : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    int check;
    bool endLockPick;
    private void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            if (miniGameManager.ran[i] != 0f) check = 1;
            else check = 0;
        }
        if (check == 0 && !endLockPick)
        {
            miniGameManager.offLockPick();
            endLockPick = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "LockObj")
        {
            miniGameManager.lockPickFail();
        }

        if(collision.gameObject.tag == "LockSafeCol")
        {
            if (collision.gameObject.name == "LockSafeCol0")
            {
                miniGameManager.ran[0] = 0f;
                miniGameManager.lockSafeCol[0].SetActive(false);
            }
            if (collision.gameObject.name == "LockSafeCol1")
            {
                miniGameManager.ran[1] = 0f;
                miniGameManager.lockSafeCol[1].SetActive(false);
            }
            if (collision.gameObject.name == "LockSafeCol2")
            {
                miniGameManager.ran[2] = 0f;
                miniGameManager.lockSafeCol[2].SetActive(false);
            }
            if (collision.gameObject.name == "LockSafeCol3")
            {
                miniGameManager.ran[3] = 0f;
                miniGameManager.lockSafeCol[3].SetActive(false);
            }
        }
    }
}
