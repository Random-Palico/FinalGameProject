using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    private float moveInput;
    private float speed = 8f;
    private float jumpingPower = 20f;

    private bool isFacingLeft = false;
    private bool jumpInput = true;

    private int maxJumps = 2;
    private int jumpsLeft;

    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    public string oneWayPlatformLayerName = "OneWayPlatform";
    public string playerLayerName = "Player";

    private HealthBar healthBar;
    public bool firstTime = true;
    public bool isFalling = false;
    public Vector3 previousPos;
    public float highestPos;
    // Damage configuration
    public float fallDamageMultiplier = 2f; // Damage per unit of height fallen
    public float heightFallThreshold = 10f; // Height threshold for fall damage

    //Knock back
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        anim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
        previousPos = transform.position;
        healthBar = GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");

        Flip();
        Jump();
        Run();
        Fall();
        FallFromHeight();
        DropDown();
    }

    private void DropDown()
    {
        if (Input.GetKey(KeyCode.S) && isGrounded())
        {
            // Ignore collision with OneWayPlatform layer when pressing S
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName), LayerMask.NameToLayer(oneWayPlatformLayerName), true);
            rb.velocity = new Vector2(rb.velocity.x, -speed); // Move down
        }
        else
        {
            // Re-enable collision when not pressing S
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayerName), LayerMask.NameToLayer(oneWayPlatformLayerName), false);
        }
    }
    public void FallFromHeight()
    {
        if (!isGrounded())
        {
            if (transform.position.y < previousPos.y && firstTime)
            {
                firstTime = false;
                isFalling = true;
                highestPos = transform.position.y; // Record the highest point before falling
            }
            previousPos = transform.position;
        }

        if (isGrounded() && isFalling)
        {
            float fallDistance = highestPos - transform.position.y;

            // Only apply damage if the fall distance exceeds the threshold
            if (fallDistance > heightFallThreshold)
            {
                int damage = Mathf.FloorToInt((fallDistance - heightFallThreshold) * fallDamageMultiplier);
                TakeFallDamage(damage);
            }

            isFalling = false;
            firstTime = true;
        }
    }
    public void TakeFallDamage(int damage)
    {
        if (healthBar != null)
        {
            healthBar.TakeDamage(damage);
        }
        else
        {
            Debug.LogError("HealthBar is not assigned!");
        }
    }
    private void FixedUpdate()
    {
        if(KBCounter <= 0)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        else
        {
            if(KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForce,KBForce);
            }
            if(KnockFromRight == false)
            {
                rb.velocity = new Vector2(KBForce, KBForce);
            }
            KBCounter -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        if (isFacingLeft && moveInput < 0f || !isFacingLeft && moveInput > 0f)
        {
            isFacingLeft = !isFacingLeft;
            transform.Rotate(0,180f, 0);
        }
    }

    //Check if player touch a ground
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);      
    }
    //----------------------------------------------------------------------------------------------------------//
    //-------------------------------------------ANIMATION----------------------------------------------//
    //----------------------------------------------------------------------------------------------------------//

    private void Fall()
    {
        if (!isGrounded())
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    private void Jump()
    {
        if (isGrounded()) 
        {
            anim.SetBool("isJumping", false);
            jumpsLeft = maxJumps;
        }    
        if (jumpInput && jumpsLeft > 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            anim.SetBool("isJumping", true);
            jumpsLeft -= 1;
        }
    }

    private void Run()
    {
        if(moveInput == 0 || !isGrounded())
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

}
