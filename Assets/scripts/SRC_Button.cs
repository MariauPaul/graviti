using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class SRC_Button : MonoBehaviour
{
    [SerializeField] private OpenDoor Complete;
    private bool didIAmGreen;
    private Animator anim;
    private GameObject iAmTheCase;

    public bool ButtonIsPush = false; 

    void Start()
    {
        if (this.tag == "Butoon")
        {
            didIAmGreen = false;
        }

        else if (this.tag == "GreenButton")
        {
            didIAmGreen = true;
        }

        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attrape")
        {
            iAmTheCase = other.gameObject;
            StartCoroutine(Placement());
        }
        if (other.tag == "Attrape" || other.tag == "Player")
        {
            Complete.UnlockDoor();
            if (Complete.tag == "Jointure")
            {
                Complete.OpeDoor();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        anim.SetBool("isTrigger", true);
        ButtonIsPush=true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!didIAmGreen)
        {
            iAmTheCase = null;
            anim.SetBool("isTrigger", false);
            Complete.LockDoor();
            ButtonIsPush = false;
        }
    }

    IEnumerator Placement()
    {
        iAmTheCase.GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        iAmTheCase.GetComponent<Rigidbody>().isKinematic = true;
    }
}
