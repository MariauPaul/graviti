using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class SRC_Button : MonoBehaviour
{
    [SerializeField] private OpenDoor Complete;
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
            StartCoroutine(Placement());
        }
        if (other.tag == "Attrape" || other.tag == "Player")
        {
            Complete.UnlockDoor();
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
        rToMove = true;
        iAmTheCase.GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        iAmTheCase.GetComponent<Rigidbody>().isKinematic = true;
        rToMove = false;
    }
}
