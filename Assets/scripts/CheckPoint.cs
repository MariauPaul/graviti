using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] PlayerMovement scriptSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            scriptSpawn.SetRespawn();
            gameObject.SetActive(false);
        }
    }
}
