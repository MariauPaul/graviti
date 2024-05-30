using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_Menu : MonoBehaviour
{
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject quit;

    [SerializeField] private GameObject cursorP;
    [SerializeField] private GameObject cursorS;
    [SerializeField] private GameObject cursorQ;

    [SerializeField] private Sprite wPlay;
    [SerializeField] private Sprite gPlay;
    [SerializeField] private Sprite wSetting;
    [SerializeField] private Sprite gSetting;
    [SerializeField] private Sprite wQuit;
    [SerializeField] private Sprite gQuit;

    [SerializeField] private CinemachineVirtualCamera camSetting;

    private int selected = 0;
    private float vertical = 0;
    private bool action = false;
    private bool isBlinking;

    private SpriteRenderer activeCursor;

    private void Start()
    {
        activeCursor = cursorP.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        selected = Mathf.Clamp(selected, 0, 3);
        Debug.Log(selected);
        GetKey();
        StateUI();
    }

    private void GetKey()
    {
        vertical = Input.GetAxis("Vertical");

        if (vertical > 0.3 && !action)
        {
            action = true;
            selected--;
        }
        else if (vertical < -0.3 && !action)
        {
            action = true;
            selected++;
        }
        else if (vertical > -0.01 && vertical < 0.01 && action)
        {
            action = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            switch (selected)
            {
                case 1:
                    SceneManager.LoadScene(1);
                    break;
                case 2:
                    camSetting.Priority = 2;
                    break;
                case 3:
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }

    private void StateUI()
    {
        switch (selected)
        {
            case 1:
                play.GetComponent<SpriteRenderer>().sprite = wPlay;
                setting.GetComponent<SpriteRenderer>().sprite = gSetting;
                quit.GetComponent<SpriteRenderer>().sprite = gQuit;
                cursorP.SetActive(true);
                cursorS.SetActive(false);
                cursorQ.SetActive(false);
                ChangeActiveCursor(cursorP.GetComponent<SpriteRenderer>());
                break;
            case 2:
                play.GetComponent<SpriteRenderer>().sprite = gPlay;
                setting.GetComponent<SpriteRenderer>().sprite = wSetting;
                quit.GetComponent<SpriteRenderer>().sprite = gQuit;
                cursorP.SetActive(false);
                cursorS.SetActive(true);
                cursorQ.SetActive(false);
                ChangeActiveCursor(cursorS.GetComponent<SpriteRenderer>());
                break;
            case 3:
                play.GetComponent<SpriteRenderer>().sprite = gPlay;
                setting.GetComponent<SpriteRenderer>().sprite = gSetting;
                quit.GetComponent<SpriteRenderer>().sprite = wQuit;
                cursorP.SetActive(false);
                cursorS.SetActive(false);
                cursorQ.SetActive(true);
                ChangeActiveCursor(cursorQ.GetComponent<SpriteRenderer>());
                break;
            default:
                play.GetComponent<SpriteRenderer>().sprite = gPlay;
                setting.GetComponent<SpriteRenderer>().sprite = gSetting;
                quit.GetComponent<SpriteRenderer>().sprite = gQuit;
                cursorP.SetActive(false);
                cursorS.SetActive(false);
                cursorQ.SetActive(false);
                StopBlink();
                break;
        }
    }

    IEnumerator Blink()
    {
        while (isBlinking)
        {
            activeCursor.enabled = !activeCursor.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator WaitingToBlink()
    {
        yield return new WaitForSeconds(0.5f);
        StartBlink();
    }

    private void ChangeActiveCursor(SpriteRenderer newCursor)
    {
        if (activeCursor != newCursor)
        {
            StopBlink();
            activeCursor = newCursor;
            StartCoroutine(WaitingToBlink());
        }
    }

    private void StartBlink()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            StartCoroutine(Blink());
        }
    }

    private void StopBlink()
    {
        if (isBlinking)
        {
            isBlinking = false;
            StopCoroutine(Blink());
            activeCursor.enabled = true;
        }
    }
}