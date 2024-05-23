using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class SRC_Button : MonoBehaviour
{
    [SerializeField] private OpenDoor Complete;
    private bool didIAmGreen;
    private Animator anim;
    private GameObject iAmTheCase;

    public bool buttonIsPushByCase = false;
    private bool buttonIsPushByPlayer = false;

    [SerializeField] private bool redButton;
    [SerializeField] private bool greenButton;
    [SerializeField] private bool dualButton;

    private bool oneButton = false;
    private bool twoButton = false;

    void Start()
    {
        //if (this.tag == "Butoon")
        //{
        //    didIAmGreen = false;
        //}

        //else if (this.tag == "GreenButton")
        //{
        //    didIAmGreen = true;
        //}

        anim = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attrape")
        {
            buttonIsPushByCase = true;
            iAmTheCase = other.gameObject;
            StartCoroutine(Placement());
        }
        else if (other.tag == "Player")
        {
            buttonIsPushByPlayer = true;
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !greenButton)
        {
            buttonIsPushByPlayer = false;

            if (!buttonIsPushByCase)
            {
                anim.SetBool("isTrigger", false);
                Complete.LockDoor();
            }
        }
        else if (other.tag == "Attrape" && !greenButton)
        {
            buttonIsPushByCase = false;

            if (!buttonIsPushByPlayer)
            {
                iAmTheCase = null;
                anim.SetBool("isTrigger", false);
                Complete.LockDoor();
            }
        //}
        //if (!didIAmGreen)
        //{
        //    iAmTheCase = null;
        //    anim.SetBool("isTrigger", false);
        //    Complete.LockDoor();
        //    buttonIsPushByCase = false;
        }
    }

    IEnumerator Placement()
    {
        iAmTheCase.GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        iAmTheCase.GetComponent<Rigidbody>().isKinematic = true;
    }
}
