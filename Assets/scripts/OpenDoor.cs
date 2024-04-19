using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator doorRoom;

    // Start is called before the first frame update
    void Start()
    {                                                       // Init. Animator
        doorRoom = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            doorRoom.SetTrigger("Open");                    // Animation need to do not be with exitTime
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            doorRoom.SetTrigger("Close");                   // Animation need to do not be with exitTime
        }
    }
}
