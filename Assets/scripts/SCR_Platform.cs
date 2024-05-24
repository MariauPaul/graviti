using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class SCR_Platform : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] private Transform start, end;

    [SerializeField] private GameObject player;

    public static SCR_Platform platform; // Singleton

    private bool Xplus = true;
    public bool platformed = false;

    private void Awake()
    {
        if (platform == null)
        {
            platform = this;
        }
    }

    void Update()
    {
        SwitchDir();
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (platformed)
        {
            if (Xplus)
            {
                transform.position = Vector3.MoveTowards(transform.position, end.position, speed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, start.position, speed);
            }
        }
    }

    private void SwitchDir()
    {
        if (transform.position.x - end.position.x < 1)
        {
            Xplus = true;
        }
        else if (start.position.x - transform.position.x < 0.1)
        {
            Xplus = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("platformed");
            collision.gameObject.transform.SetParent(transform);
            StartCoroutine(PlatformStart());
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    public void ResetPlatform()
    {
        platformed = false;
        transform.position = start.position;
        player.transform.SetParent(null);
    }

    IEnumerator PlatformStart()
    {
        yield return new WaitForSeconds(0.2f);
        platformed = true;
    }
}
