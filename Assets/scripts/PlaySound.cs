using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    bool triggerbox;
    bool alreadyplayed = false;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();   
        m_AudioSource.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyplayed)
        {
            m_AudioSource.enabled = true;
            alreadyplayed = true;
        m_AudioSource.Play();
        }
       
    }
}
