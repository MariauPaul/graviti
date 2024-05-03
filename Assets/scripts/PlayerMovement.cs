using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Parameters")]
    [SerializeField] float speed;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float airMultiplier;
    [SerializeField] bool isGrounded;

    private Vector3 moveDir;
    private Vector3 currentVel;
    private Vector3 goForward;

    private float horizontalInput;
    private bool isFlip = false;

    [Header("Jump Parameters")]
    [SerializeField] float jumpCd;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpButtonMaxPressTime;
    [SerializeField] float maxDrag;

    private float jumpForce;
    private float jumpButtonPressTime;

    private bool isJumpReady = true;
    private bool jumpCancel;
    private float jumpBuffer;

    [Header("Gravity")]
    [SerializeField] bool isGravInvert = false;
    [SerializeField] float switchGravCd;
    // [SerializeField] Quaternion rotation;

    private bool canSwitchGrav = true;

    [Header("Other")]
    [SerializeField] LayerMask iAmCelling;
    [SerializeField] LayerMask iAmGround;
    [SerializeField] Transform footR;
    [SerializeField] Transform grabPos;
    [SerializeField] Transform grabRayCast;

    private Vector3 respawnPos;

    private GameObject itemGrab;
    private bool handFull = false;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        goForward = new Vector3(1, 0, 0);
    }

    void Update()
    {
        GetKey();
        Grounded();
        CheckJumpCancel();
        Rotate();
    }
    private void FixedUpdate()
    {
        Movement();
        SpeedVel();
        JumpCancel();
    }

    void GetKey()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpButtonPressTime = 0;
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchGrav();
            Debug.Log("Press F");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab();
        }
    }

    void Grounded()
    {
        isGrounded = Physics.Raycast(footR.position, isGravInvert ? Vector3.up : Vector3.down, 0.1f/*, iAmGround*/);

        if (isGrounded)
        {
            rb.drag = groundDrag;
            if (rb.mass > 1)
            { 
                rb.mass = 1;
            }
        }
        else
        {
            if (!isGravInvert && rb.velocity.y < -0.01)
            {
                rb.drag = Mathf.MoveTowards(rb.drag, maxDrag * -1, 0.15f);
            }
            else if(isGravInvert && rb.velocity.y > 0.01)
            {
                rb.drag = Mathf.MoveTowards(rb.drag, maxDrag * -1, 0.15f);
            }
        }
    }


    // MovementPlayer //
    void Movement()
    {
        moveDir = goForward * horizontalInput;
        if (moveDir.x > 0.1)        isFlip = false;
        else if (moveDir.x < -0.1)   isFlip = true;

        // air cointrol
        if (!isGrounded) rb.AddForce(moveDir.normalized * speed * 70 * airMultiplier, ForceMode.Force);

        // groun control
        if (isGrounded) rb.AddForce(moveDir.normalized * speed * 70, ForceMode.Force);
    }

    private void SpeedVel()
    {
        currentVel = new Vector2(rb.velocity.x, rb.velocity.y);
        if (currentVel.magnitude > speed)
        {
            Vector2 limitVel = currentVel.normalized * speed;
            rb.velocity = new Vector2(limitVel.x, rb.velocity.y);
        }
    }


    // Jump Player //
    void Jump()
    {
        if (isJumpReady && isGrounded)
        {
            isJumpReady = false;
            jumpCancel = false;

            //--------------------------------  Init. JumpForce  --------------------------------//
            jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * -2)) * rb.mass;
            jumpForce = isGravInvert? -jumpForce : jumpForce;

            //--------------------------------    Jump Impulse   --------------------------------//
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            Debug.Log(rb.velocity);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCd);
        }
    }

    void ResetJump()
    {
        isJumpReady = true;
    }

    void CheckJumpCancel()
    {
        jumpButtonPressTime += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpCancel = true;
        }
    }

    void JumpCancel()
    {
        if (jumpCancel && !isJumpReady && (isGravInvert ? rb.velocity.y < 0 : rb.velocity.y > 0))
        {
            rb.AddForce((isGravInvert ? Vector3.up :Vector3.down) * 100);
        }
    }

    void SwitchGrav()
    {
        if (canSwitchGrav && isGrounded)
        {
            canSwitchGrav = false;

            isGravInvert = !isGravInvert;
            //goForward = isGravInvert ? new Vector3(1,0,0) : new Vector3(1,0,0);

            Physics.gravity *= -1;

        Invoke(nameof(ResetSwitchGrave), switchGravCd);
        }
    }

    void Rotate()
    {
        Debug.Log(rb.rotation);
        if (!isGravInvert && transform.rotation.z != 1 && !isFlip)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isGravInvert && transform.rotation.z != 1 && isFlip)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (isGravInvert && !isFlip && transform.rotation.y != 1 && transform.rotation.x != 1)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 180);
        }
        else if(isGravInvert && transform.rotation.z != 1 && transform.rotation.x != 1 && isFlip)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }

    }

    void ResetSwitchGrave()
    {
        canSwitchGrav = true;
    }

    void Grab()
    {
        if (handFull)
        {
            handFull= false;
            itemGrab.transform.SetParent(null);
            itemGrab.GetComponent<Rigidbody>().useGravity = true;
            itemGrab.GetComponent<Rigidbody>().drag = 10;
            itemGrab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            itemGrab.GetComponent<Collider>().isTrigger = false;
            //itemGrab.GetComponent<Rigidbody>().AddForce(GetComponent.x, 2,ForceMode.Force);
            itemGrab = null;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(grabRayCast.position, Vector3.right, out hit, 1.5f))

            {
                if (hit.collider.tag == "Attrape")
                {
                    handFull= true;
                    itemGrab = hit.collider.gameObject;
                    itemGrab.transform.position = grabPos.position;
                    itemGrab.GetComponent<Rigidbody>().useGravity = false;
                    itemGrab.GetComponent<Rigidbody>().drag = 0;
                    itemGrab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    itemGrab.GetComponent<Collider>().isTrigger = true;
                    itemGrab.transform.parent = grabPos.transform;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint")
        {
            other.gameObject.SetActive(false);
            SetRespawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Attrape")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.drag = 1;
        }
        else if (collision.gameObject.tag == "Death") StartCoroutine(DelayDeath());
    }

    public void SetRespawn()
    {
        respawnPos = transform.position;
    }

    private void Respawn()
    {
        transform.position = respawnPos;
    }

    public void Death()
    {
        StartCoroutine(DelayDeath());
    }

    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(2);
        Respawn();
    }
}
