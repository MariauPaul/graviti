using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    [SerializeField] private OpenDoor Complete;

    private void OnTriggerEnter(Collider other)
    {
        Complete.UnlockDoor();    
    }
}
