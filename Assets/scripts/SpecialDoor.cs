using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDoor : MonoBehaviour
{
    [SerializeField] private OpenDoor OpenClose;
    [SerializeField] private Animator invertStateofthisDoor;

    private void Update()
    {
        bool checkState = invertStateofthisDoor.GetBool("SomeOneIsThere");
        if (checkState) OpenClose.CloseDoor();
        else OpenClose.OpeDoor();
    }
}
