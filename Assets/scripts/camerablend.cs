using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerablend : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera Room;
    [SerializeField] private CinemachineVirtualCamera player;
    [SerializeField]  bool CheckCam =true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CheckCam" + CheckCam);
    }
    private void ShowOverheadView() {
       player.enabled = false;
       Room.enabled = true;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (CheckCam)
            {
                player.Priority = 1;
                Debug.Log("PlqyerToRoom");
                Room.Priority = 2;
                CheckCam = false;
            }
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
            player.Priority = 2;
            Debug.Log("RoomToPlqyer");
            Room.Priority = 1;
        CheckCam = true;
    }
}



