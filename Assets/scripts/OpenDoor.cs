using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator doorRoom;
    private camerablend room;
    private bool isInRoom;
    private bool doorIsLock;

    // Start is called before the first frame update
    void Start()
    {                                                                // Init. Animator
        if (this.tag == "Enter")
        { 
            doorIsLock = false;
        }

        else if (this.tag == "Exit")
        {
            doorIsLock = true;
        }

        doorRoom = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!doorIsLock)
            {
                OpeDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.tag == "Enter")
        {
            if (other.tag == "Player")
            {
                if (isInRoom && !doorIsLock)
                {
                    doorIsLock = true;
                }
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                if (!isInRoom && !doorIsLock)
                {
                    doorIsLock = true;
                }
            }

        }
        CloseDoor();
    }

    public void isPlayerInARoom(bool x)
    {
        if (x)
        {
            isInRoom = true;
        }
        else
        {
            isInRoom = false;
        }
    }

    public void UnlockDoor()
    {
        doorIsLock = false;
    }

    public void OpeDoor()
    {
        doorRoom.SetBool("SomeOneIsThere", true);                   // Animation need exitTime 0/I
    }

    public void CloseDoor()
    {
        doorRoom.SetBool("SomeOneIsThere", false);                  // Animation need exitTime 0/I
    }
}
