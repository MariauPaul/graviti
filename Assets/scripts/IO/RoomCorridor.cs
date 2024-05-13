using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class RoomCorridor : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestroy;
    private PlayableDirector TL;

    void Start()
    { 
        TL = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            TL.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            TL.Play();
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(transform.parent);
    }
}
