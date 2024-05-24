using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class camerablend : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camRoom;                                              // Room Camera
    private CinemachineVirtualCamera camPlayer;                                                     // Player Camera
    private static bool camIsOnPlayer = true;                                                       // Prevent CamGlitch;

    [SerializeField] private float timeBeforeDestroy;
    private PlayableDirector TL;

    [SerializeField] private GameObject EnterDoor, ExitDoor;
    private OpenDoor enterDoorSCR, exitDoorSCR;

    public bool isInRoom;
    private bool roomCompleted = false;

    void Start()
    {                                                                                               // init. Cams
        //EnterDoor = transform.GetComponentInChildren<OpenDoor>(gameObject.CompareTag("Enter"));
        //ExitDoor = transform.GetComponentInChildren<OpenDoor>(gameObject.CompareTag("Exit"));
        enterDoorSCR = EnterDoor.GetComponent<OpenDoor>();
        exitDoorSCR = ExitDoor.GetComponent<OpenDoor>();

        camRoom = GetComponentInChildren<CinemachineVirtualCamera>();
        camPlayer = GameObject.Find("camPos").GetComponentInChildren<CinemachineVirtualCamera>();
        camRoom.Priority = 0;

        TL = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {                                                                                               // Switch cBrain from player to room
        if (other.tag == "Player" && camIsOnPlayer)
        {
            enterDoorSCR.isPlayerInARoom(true); exitDoorSCR.isPlayerInARoom(true);
            isInRoom = true;
            camPlayer.Priority = 1;
            camRoom.Priority = 2;
            camIsOnPlayer = false;

            //TL.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {                                                                                               // Switch cBrain from player to room
        if (other.tag == "Player" && !camIsOnPlayer)
        {
            enterDoorSCR.isPlayerInARoom(false); exitDoorSCR.isPlayerInARoom(false);
            isInRoom = false;
            camPlayer.Priority = 2;
            camRoom.Priority = 1;
            camIsOnPlayer = true;
            if (roomCompleted)
            {
                TL.Play();
                StartCoroutine(Destroy());
            }
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(transform.parent);
    }

    public void Completed()
    {
        roomCompleted = true; // a modif
    }
}



