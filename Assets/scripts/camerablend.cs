using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerablend : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] CinemachineVirtualCamera camRoom;                                                       // Room Camera
    private CinemachineVirtualCamera camPlayer;                                                     // Player Camera
    private static bool camIsOnPlayer = true;                                                       // Prevent CamGlitch;

    void Start()
    {                                                                                               // init. Cams
        camRoom = GetComponentInChildren<CinemachineVirtualCamera>();
        Player = GameObject.FindWithTag("Player");
        camPlayer = Player.GetComponentInChildren<CinemachineVirtualCamera>();
        camRoom.Priority = 0;

    }

    private void OnTriggerEnter(Collider other)
    {                                                                                               // Switch cBrain from player to room
        if (other.tag == "Player" && camIsOnPlayer)
        {
            camPlayer.Priority = 1;
            camRoom.Priority = 2;
            camIsOnPlayer = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {                                                                                               // Switch cBrain from player to room
        if (other.tag == "Player" && !camIsOnPlayer)
        {
            camPlayer.Priority = 2;
            camRoom.Priority = 1;
            camIsOnPlayer = true;
        }
    }
}



