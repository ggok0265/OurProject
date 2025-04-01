using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashedDoor : MonoBehaviour
{
    public float xVec;
    public float zVec;

    private void OnEnable()
    {
        Invoke("DeleteObj", 5);
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(xVec, 0, zVec) * 500 * Time.deltaTime, ForceMode.Impulse);
    }

    private void DeleteObj()
    {
        gameObject.SetActive(false);
    }
}
