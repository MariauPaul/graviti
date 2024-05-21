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
    [SerializeField] float deltaMoveDetection;

    private Vector3 moveDir;
    private Vector3 currentVel;
    private Vector3 goForward;

    private float horizontalInput;
    private bool isFlip = false;
    private bool canMove = true;

    [Header("Jump Parameters")]
    [SerializeField] float jumpCd;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpButtonMaxPressTime;
    [SerializeField] float maxDrag;

    private float jumpForce;
    private float jumpButtonPressTime;

    private bool isJumpReady = true;
    private bool jumpCancel;
    private bool canJump = true;
    private float jumpBuffer;

    [Header("Gravity")]
    [SerializeField] bool isGravInvert = false;
    [SerializeField] float switchGravCd;

    private bool canSwitchGrav = true;

    [Header("Other")]
    [SerializeField] LayerMask iAmCelling;
    [SerializeField] LayerMask iAmGround;
    [SerializeField] Transform footR;
    [SerializeField] Transform grabPos;
    [SerializeField] Transform grabRayCast;
    [SerializeField] private GameObject playerMesh;

    private Vector3 respawnPos;

    private GameObject itemGrab;

    private bool handFull = false;
    private bool waitingForDeath = false;

    private Rigidbody rb;
    [SerializeField] Animator animator;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        goForward = new Vector3(1, 0, 0);
    }

    private void Update()
    {
        //Debug.Log(rb.velocity.y);
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

    private void GetKey()
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

    private void Grounded()
    {
        isGrounded = Physics.Raycast(footR.position, isGravInvert ? Vector3.up : Vector3.down, 0.1f/*, iAmGround*/);

        if (isGrounded)
        {
            rb.drag = groundDrag;
            //if (rb.mass > 1)
            //{
            //    rb.mass = 1;
            //}
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

    private void Rotate()
    {
        if (!isGravInvert && !isFlip && playerMesh.transform.rotation.w != 1)
        {
            playerMesh.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isGravInvert && isFlip && playerMesh.transform.rotation.y != 1)
        {
            playerMesh.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (isGravInvert && !isFlip && playerMesh.transform.rotation.x != 1)
        {
            playerMesh.transform.localRotation = Quaternion.Euler(0, 180, 180);
        }
        else if (isGravInvert && isFlip && playerMesh.transform.rotation.z != 1)
        {
            playerMesh.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
    }

    // MovementPlayer //
    private void Movement()
    {
        moveDir = goForward * horizontalInput;

        animator.SetBool("IsWalking",!(moveDir.magnitude < deltaMoveDetection) && (moveDir.magnitude > -1f*deltaMoveDetection));
        /*
        if ((moveDir < deltaMoveDetection) && (moveDir > -1f*deltaMoveDetection)){
          animator.SetBool("isWalking",false);
        }
        else {
          animator.SetBool("isWalking",true);
        }
        */

        if (moveDir.x > 0.1)        isFlip = false;
        else if (moveDir.x < -0.1)   isFlip = true;

        if (canMove)
        {
            // air cointrol
            if (!isGrounded) rb.AddForce(moveDir.normalized * speed * 70 * rb.mass * airMultiplier, ForceMode.Force);

            // groun control
            if (isGrounded) rb.AddForce(moveDir.normalized * speed * 70 * rb.mass, ForceMode.Force);
        }
    }

    public void IOMove(bool x)
    {
        if (x)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
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
    private void Jump()
    {
        if (isJumpReady && isGrounded && canJump)
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

    private void ResetJump()
    {
        isJumpReady = true;
    }

    private void CheckJumpCancel()
    {
        jumpButtonPressTime += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpCancel = true;
        }
    }

    private void JumpCancel()
    {
        if (jumpCancel && !isJumpReady && (isGravInvert ? rb.velocity.y < 0 : rb.velocity.y > 0))
        {
            rb.AddForce((isGravInvert ? Vector3.up :Vector3.down) * 100);
        }
    }

    public void IOJump(bool x)
    {
        if (x)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void SwitchGrav()
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

    public void IOSwitchGrav(bool x)
    {
        if (!x)
        {
            canSwitchGrav = false;
        }
        else
        {
            canSwitchGrav = true;
        }
    }

    private void ResetSwitchGrave()
    {
        canSwitchGrav = true;
    }

    private void Grab()
    {
        if (handFull)
        {
            handFull= false;
            itemGrab.transform.SetParent(null);
            itemGrab.GetComponent<Rigidbody>().useGravity = true;
            itemGrab.GetComponent<Rigidbody>().drag = 10;
            StartCoroutine(DelayUnGrab());
            itemGrab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            itemGrab.GetComponent<Rigidbody>().isKinematic = false;
            itemGrab.GetComponent<Rigidbody>().AddForce(isFlip ? Vector3.left * 2000 : Vector3.right * 2000, ForceMode.Impulse);
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(grabRayCast.position, isFlip ? Vector3.left : Vector3.right, out hit, 2.5f))

            {
                if (hit.collider.tag == "Attrape")
                {
                    handFull= true;
                    itemGrab = hit.collider.gameObject;
                    itemGrab.transform.position = grabPos.position;
                    itemGrab.GetComponent<Rigidbody>().useGravity = false;
                    itemGrab.GetComponent<Rigidbody>().drag = 0;
                    itemGrab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    itemGrab.GetComponent<Rigidbody>().isKinematic = true;
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
        if (other.gameObject.tag == "Death")
        {
           Death();
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
        waitingForDeath = false;
        transform.position = respawnPos;
    }

    public void Death()
    {
        if (!waitingForDeath)
        {
            StartCoroutine(DelayDeath());
        }
    }

    IEnumerator DelayDeath()
    {
        waitingForDeath = true;
        yield return new WaitForSeconds(2);
        Respawn();
    }

    IEnumerator DelayUnGrab()
    {
        yield return new WaitForSeconds(0.2f);
        itemGrab.GetComponent<Collider>().isTrigger = false;
        itemGrab = null;
    }
}
