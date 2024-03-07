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
        rb = GetComponent<Rigidbody>(); // Récupère le Rigidbody attaché à cet objet
        rb.useGravity = false; // Désactive la gravité normale

        // Récupère la masse de l'objet
        masse = rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        // Récupère la gravité globale du monde
        Vector3 graviteMonde = Physics.gravity;

        // Calcule la gravité inverse en tenant compte de la masse de l'objet
        Vector3 graviteInverse = -graviteMonde * (1 / masse);

        // Applique la gravité inverse
        rb.AddForce(graviteInverse, ForceMode.Acceleration);
    }
}
