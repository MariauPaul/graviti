using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRC_OnJump : MonoBehaviour
{
    private PlayerMovement IO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IO.IOSwitchGrav(true);
        }
    }
}
