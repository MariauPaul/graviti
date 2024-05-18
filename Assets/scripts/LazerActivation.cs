using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LazerActivation : MonoBehaviour
{

    [SerializeField] GameObject Lazer;
    [SerializeField] SRC_Button SRC_Button;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LazerActif();
    }
 
    private void LazerActif()
    {
        if (SRC_Button.ButtonIsPush == true)
        {
            Lazer.SetActive(false);
        }
        else if (SRC_Button.ButtonIsPush == false)
        {
            Lazer.SetActive(true);
        }
    }
}
