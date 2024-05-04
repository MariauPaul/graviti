using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
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
            iAmTheCase = other.gameObject;
            iAmTheCase.GetComponent<Collider>().isTrigger = true;
            StartCoroutine(Placement());
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

    IEnumerator Placement()
    {
        yield return new WaitForSeconds(0.5f);
        iAmTheCase.GetComponent<Rigidbody>().isKinematic = true;
    }
}
