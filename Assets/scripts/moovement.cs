using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class moovement : MonoBehaviour
{
    private Vector3 positionChange;
    private Vector3 makemedash;
    private Vector3 jump;

    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float dashForce;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform bras;
    [SerializeField] private Rigidbody rb;
    private bool isJumping;

    [SerializeField] private Transform footR;
    public float vitesseInversionGravite = 1f;


    private bool graviteInverse = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    void MovementPlayer()
    {
        isGrounded = Physics.Raycast(footR.position, graviteInverse ? Vector3.up : Vector3.down, 0.1f);

        // ---------- Movement ----------

        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //rb.velocity = new Vector3(speed, 0, 0);
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if (Input.GetKeyUp(KeyCode.A)) {
            rb.velocity = Vector3.zero;
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            rb.velocity = Vector3.zero;
        }


            // ---------- Jump ----------

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            isJumping = true;
        }

        if (isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            //Debug.Log("Jump");
            isJumping = false;
        }
    }


        


    void Start()
    {

        jump = Vector3.up * jumpHeight * Time.deltaTime;
        positionChange = new Vector3(speed * Time.deltaTime, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {
            // Inverser l'état de la gravité
            graviteInverse = !graviteInverse;

            // Appeler la fonction pour inverser la gravité
            InverserLaGravite();
        }

        //Debug.DrawRay(footR.position, Vector3.down * 0.1f, Color.red);

        MovementPlayer();

        if (Input.GetKeyDown(KeyCode.E))
        {
            prendre();
        }
    }

    void InverserLaGravite()
    {
        jumpHeight = -jumpHeight;

        Physics.gravity = graviteInverse ? new Vector3(0, 9.81f, 0) : new Vector3(0, -9.81f, 0);
        transform.localRotation = graviteInverse ? Quaternion.Euler(0, -270, 180) : Quaternion.Euler(0, 90, 0);
        if(graviteInverse)
        {
            transform.Translate(new Vector3(0, -2f, 0));
            
        }
        else
        {
            transform.Translate(new Vector3(0, -2f, 0));
        }

    }

    void prendre()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.left, out hit, 1.5f))

        {

            if (hit.collider.tag == "Attrape")
            {
                hit.collider.transform.position = bras.position;
                hit.collider.GetComponent<Rigidbody>().useGravity = false;
                hit.collider.GetComponent<Rigidbody>().drag = 10;
                hit.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                hit.collider.GetComponent<Collider>().isTrigger = true;
                hit.collider.transform.parent = bras.transform;
            }

        }
    }
}