using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRC_Button : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("isTrigger", true);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("isTrigger", false);
    }
}
