using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//?????????? ????

public class CameraMove : MonoBehaviour
{
    public GameManager gameManager; // ???? ?????? ??????
    public GameObject Player;

    /*< ???????? ?????? ???? ???? ??????>*/
    float mouseX;
    float mouseY;
    float mouseSensitivity;
    public Transform playerBody;
    float xRotation = 0f;

    /*<???? ???????? ???????? ???? ??????>*/
    private Vector3 screenCenter;
    private Ray centerRay;
    private RaycastHit hitInfo;

    public Slider mouseSensitivitySet;
    void Start()
    {
        gameManager.mouseControlForPlayer(); // ?????? ???????? ???? ???? ??????
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // ???? ???? ??????
        mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 300f);
        mouseSensitivitySet.value = mouseSensitivity;
    }

    void Update()
    {
        gazeShift(); // ???????? ???? ????
        centerOfScreen(); // ???? ?? ???? ????
        mouseSensitivitySlider();
    }

    void gazeShift() // ???????? ???? ????
    {
        if(gameManager.sightControlMode)
        {
            mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -45f, 80f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up, mouseX);
        }
    }

    void centerOfScreen() // ???? ?? ???? ????
    {
        centerRay = Camera.main.ScreenPointToRay(screenCenter);
        Debug.DrawRay(transform.position, transform.forward * 1.5f, Color.red);
        if (Physics.Raycast(centerRay, out hitInfo, 1.5f))
        {
            Player.GetComponent<PlayerAction>().pointingObjInfo(hitInfo);
        }
        else
            Player.GetComponent<PlayerAction>().pointedobj = null;
    }

    public void mouseSensitivitySlider()
    {
        mouseSensitivity = mouseSensitivitySet.value;
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);
    }
}
