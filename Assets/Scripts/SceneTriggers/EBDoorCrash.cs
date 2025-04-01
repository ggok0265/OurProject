using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBDoorCrash : MonoBehaviour
{
    public GameObject doorR;
    public GameObject doorL;
    public GameObject Mob1;

    public bool trigger;
    public bool stoper;
    public float timer;

    void FixedUpdate()
    {
        if(trigger && !stoper)
        {
            timer += Time.deltaTime;
            Mob1.transform.position = Vector3.MoveTowards(Mob1.transform.position, new Vector3(-33.68f,17.4f,38.49f), 50 * Time.deltaTime);
            doorR.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, -1) * 10, ForceMode.Impulse);
            doorL.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 1) * 10, ForceMode.Impulse);
            if (timer > 0.2f)
            {
                stoper = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Mob1")
        {
            if (!trigger)
            {
                gameObject.GetComponent<AudioSource>().Play();
                Mob1.GetComponent<MonsterAI>().MonsterCrash();
                doorR.GetComponent<Rigidbody>().isKinematic = false;
                doorR.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                doorL.GetComponent<Rigidbody>().isKinematic = false;
                doorL.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                trigger = true;
            }
        }
    }
}
