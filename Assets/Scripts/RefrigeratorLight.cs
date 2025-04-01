using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefrigeratorLight : MonoBehaviour
{
    public GameObject light1;
    private bool isOn;

    void Update()
    {
        if(GetComponent<BoxOpen>().isOpen && !isOn)
        {
            light1.SetActive(true);
        }
        else if(!GetComponent<BoxOpen>().isOpen && isOn)
        {
            light1.SetActive(false);
        }
    }
}
