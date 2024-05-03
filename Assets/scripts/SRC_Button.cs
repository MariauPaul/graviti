using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SRC_Button : MonoBehaviour
{
    private bool rToMove = false;
    private Animator anim;
    private GameObject iAmTheCase;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("isTrigger", true);
        if (other.tag == "Attrape")
        {
            iAmTheCase = other.GetComponent<GameObject>();
            Placement();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (rToMove != true)
        {
            iAmTheCase = null;
            anim.SetBool("isTrigger", false);
        }
    }

    public void Placement()
    {
        if (iAmTheCase != null)
        {
            iAmTheCase.transform.parent = transform;
            while (iAmTheCase.transform.position != Vector3.zero)
            {
                iAmTheCase.transform.position = Vector3.MoveTowards(iAmTheCase.transform.position, Vector3.zero, 0.01f);
            }
        }
        rToMove = true;
        iAmTheCase.GetComponent<Rigidbody>().isKinematic = true;
    }
}
