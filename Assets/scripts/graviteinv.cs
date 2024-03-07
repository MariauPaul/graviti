using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graviteinv : MonoBehaviour
{
    private Rigidbody rb;
    private float masse;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // R�cup�re le Rigidbody attach� � cet objet
        rb.useGravity = false; // D�sactive la gravit� normale

        // R�cup�re la masse de l'objet
        masse = rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        // R�cup�re la gravit� globale du monde
        Vector3 graviteMonde = Physics.gravity;

        // Calcule la gravit� inverse en tenant compte de la masse de l'objet
        Vector3 graviteInverse = -graviteMonde * (1 / masse);

        // Applique la gravit� inverse
        rb.AddForce(graviteInverse, ForceMode.Acceleration);
    }
}
