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
    [SerializeField] private bool sawButton;

    [SerializeField] private bool triggerDoor;

    private bool oneButton = false;
    private bool twoButton = false;

    void Start()
    {
        anim = GetComponent<Animator>();
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
            if (dualButton && redButton)
            {
                oneButton = true;
                if (twoButton)
                {
                    Complete.OpeDoor();
                }
            }
            else if (dualButton && greenButton)
            {
                twoButton = true;
                if (oneButton)
                {
                    Complete.OpeDoor();
                }
            }
            else if (triggerDoor)
            {
                Complete.OpeDoor();
            }
            else if (greenButton && sawButton)
            {
                Saw.saw.StopSaw();
            }

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
        //----------------------------- RED BUTTON PLAYER -----------------------------
        if (other.tag == "Player" && redButton && !dualButton)
        {
            buttonIsPushByPlayer = false;

            if (!buttonIsPushByCase)
            {
                anim.SetBool("isTrigger", false);
                Complete.LockDoor();
                if (triggerDoor)
                {
                    Complete.CloseDoor();
                }
            }
        }
        //----------------------------- RED BUTTON CASE -----------------------------
        else if (other.tag == "Attrape" && redButton && !dualButton)
        {
            buttonIsPushByCase = false;

            if (!buttonIsPushByPlayer)
            {
                iAmTheCase = null;
                anim.SetBool("isTrigger", false);
                Complete.LockDoor();
                if (triggerDoor)
                {
                    Complete.CloseDoor();
                }
            }
        }
        //}
        //if (!didIAmGreen)
        //{
        //    iAmTheCase = null;
        //    anim.SetBool("isTrigger", false);
        //    Complete.LockDoor();
        //    buttonIsPushByCase = false;

        if (other.tag == "Player" && redButton && dualButton)
        {
            buttonIsPushByPlayer = false;

            if (!buttonIsPushByCase)
            {
                anim.SetBool("isTrigger", false);
                Complete.LockDoor();
            }
        }
        else if (other.tag == "Attrape" && redButton && dualButton)
        {
            buttonIsPushByCase = false;

            if (!buttonIsPushByPlayer)
            {
                iAmTheCase = null;
                anim.SetBool("isTrigger", false);
                Complete.LockDoor();
            }
        }
    }

    IEnumerator Placement()
    {
        iAmTheCase.GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        iAmTheCase.GetComponent<Rigidbody>().isKinematic = true;
    }
}
