using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class moovement : MonoBehaviour
{
    private Vector3 positionChange;
    private Vector3 makemedash;
    private Vector3 jump;

    [SerializeField] float dragForce;
    private bool gravityIsSwitching = false;
    private Vector2 currentVel;

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

    void Start()
    {
        jump = Vector3.up * jumpHeight * Time.deltaTime;
        positionChange = new Vector3(speed * Time.deltaTime, 0, 0);
    }

    void Update()
    {
        Debug.Log(rb.drag);
        isGrounded = Physics.Raycast(footR.position, graviteInverse ? Vector3.up : Vector3.down, 0.1f);

        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {
            

            // Appeler la fonction pour inverser la gravité
            InverserLaGravite();
        }

        //Debug.DrawRay(footR.position, Vector3.down * 0.1f, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            prendre();
        }

        if (isGrounded)
        {
            rb.drag = dragForce;
        }
        
    }

    private void FixedUpdate()
    {
        MovementPlayer();
        SpeedVel();
    }

    void MovementPlayer()
    {
            // ---------- Movement ----------
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-1, 0) * speed * 80 * Time.deltaTime, ForceMode.Force) ;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(1, 0) * speed * 80 * Time.deltaTime, ForceMode.Force);
        }
        /*if (Input.GetKey(KeyCode.A))
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
        }*/

        // ---------- Jump ----------

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            isJumping = true;
        }

        if (isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        
            isJumping = false;
        }
    }
    
    private void SpeedVel()
    { 
        currentVel = new Vector2(rb.velocity.x, 0f);
        if (currentVel.magnitude > speed)
        {
            Vector2 limitVel = currentVel.normalized * speed;
            rb.velocity = new Vector2(limitVel.x, 0);
        }
    }

    void InverserLaGravite()
    {
        jumpHeight = -jumpHeight;

        // Inverser l'état de la gravité
        graviteInverse = !graviteInverse;

        Physics.gravity = graviteInverse ? new Vector3(0, 9.81f, 0) : new Vector3(0, -9.81f, 0);
        //Debug.Log(gravityIsSwitching + "before");
        transform.localRotation = graviteInverse ? Quaternion.Euler(0, -270, 180) : Quaternion.Euler(0, 90, 0);

        //gravityIsSwitching = true;
        //Debug.Log(gravityIsSwitching);
        //rb.drag *= -1;
        //StartCoroutine(GravitySwitching());

        if (graviteInverse)
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

    /*IEnumerator GravitySwitching()
    {
        yield return new WaitForSeconds(0.2f);
        gravityIsSwitching = false;
    }*/
}