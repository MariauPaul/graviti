using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator doorRoom;
    private camerablend room;
    private bool isFirstDoorIsLock;

    // Start is called before the first frame update
    void Start()
    {                                                                // Init. Animator
        doorRoom = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OpeDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (room.isInRoom && !isFirstDoorIsLock)
            {
                gameObject.GetComponent<Collider>().enabled = false;
                isFirstDoorIsLock = true;
            }
            CloseDoor();
        }
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
