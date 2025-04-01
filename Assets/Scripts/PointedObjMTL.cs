using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointedObjMTL : MonoBehaviour
{
    public GameObject[] obj;
    private bool selected;
    private Material[] originalMtl;
    public Material selectedMtl;

    //private float speed = 10;
    public float time;


    private void Start()
    {
        originalMtl = new Material[obj.Length];
        selected = false;
        SetOriginalMtl();
    }
    private void Update()
    {
        if(selected)
        {
            if(time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                setMtl(false);
            }
        }
    }

    public void setMtl(bool sel)
    {
        selected = sel;
        if(selected)
        {
            time = 0.1f;
        }
        TurnMtl();
    }

    void SetOriginalMtl()
    {
        for(int num = 0; num < obj.Length ; num++)
        {
            originalMtl[num] = obj[num].GetComponent<MeshRenderer>().material;
        }
    }

    void TurnMtl()
    {
        if(!selected)
        {
            for (int num = 0; num < obj.Length; num++)
            {
                obj[num].GetComponent<MeshRenderer>().material = originalMtl[num];
            }
        }
        else if(selected)
        {
            for (int num = 0; num < obj.Length; num++)
            {
                obj[num].GetComponent<MeshRenderer>().material = selectedMtl;
            }
        }
    }

}
