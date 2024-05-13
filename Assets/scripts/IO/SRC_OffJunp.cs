using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRC_OffJunp : MonoBehaviour
{
    [SerializeField] private float Delay;
    private PlayerMovement IO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IO.IOSwitchGrav(false);
            StartCoroutine(DelayeBeforeReactive());
        }

        IEnumerator DelayeBeforeReactive()
        {
            yield return new WaitForSeconds(Delay);
            IO.IOSwitchGrav(true);
        }
    }
}
