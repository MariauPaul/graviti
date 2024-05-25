using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private Animator saw1;
    [SerializeField] private Animator saw2;
    [SerializeField] private Animator saw3;

    public static Saw saw;

    private void Awake()
    { 
        if (saw == null) { saw = this; }
    }

    public void StopSaw()
    {
        saw1.enabled= false; saw2.enabled = false; saw3.enabled = false;
        saw1.GetComponent<Collider>().enabled = false;
        saw2.GetComponent<Collider>().enabled = false;
        saw3.GetComponent<Collider>().enabled = false;
    }
}
