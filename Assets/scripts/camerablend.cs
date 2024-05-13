using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.ProBuilder.Shapes;

public class camerablend : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camRoom;                                              // Room Camera
    private CinemachineVirtualCamera camPlayer;                                                     // Player Camera
    private static bool camIsOnPlayer = true;                                                       // Prevent CamGlitch;

    [SerializeField] private float timeBeforeDestroy;
    private PlayableDirector TL;

    private Collider EnterDoor, ExitDoor;

    public bool isInRoom = false;
    private bool roomCompleted = false;

    void Start()
    {                                                                                               // init. Cams
        //EnterDoor = transform.GetComponentInChildren<Collider>(gameObject.CompareTag("Enter"));
        //ExitDoor = transform.GetComponentInChildren<Collider>(gameObject.CompareTag("Exit"));
        camRoom = GetComponentInChildren<CinemachineVirtualCamera>();
        camPlayer = GameObject.Find("camPos").GetComponentInChildren<CinemachineVirtualCamera>();
        camRoom.Priority = 0;
        TL = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {                                                                                               // Switch cBrain from player to room
        if (other.tag == "Player" && camIsOnPlayer)
        {
            isInRoom = true;
            camPlayer.Priority = 1;
            camRoom.Priority = 2;
            camIsOnPlayer = false;

            TL.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {                                                                                               // Switch cBrain from player to room
        if (other.tag == "Player" && !camIsOnPlayer)
        {
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
    {roomCompleted = true;}
}



