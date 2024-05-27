using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TrrigerElement : MonoBehaviour
{
    [SerializeField] private GameObject activeGO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activeGO.GetComponent<Animator>().SetTrigger("Activate");
            Debug.Log("Activated");
        }
    }
}