using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Rigidbody2D rb;
    public CircleCollider2D groundCheck;
    public float jumpCut = 0.5f;

    private int groundLayerMask;
    private int resetLayerMask;
    private Vector3 spawnPosition;
    [SerializeField] private Animator animator;

    private bool isFacingRight = true;

    // --- NEW: Attack Variables ---
    public GameObject attackHitbox;   // You will drag your new hitbox object here
    public float attackDuration = 0.2f; // How long the hitbox stays active
    private float attackTimer = 0f;     // A timer to count down the attack

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        groundLayerMask = LayerMask.GetMask("Ground");
        animator.SetBool("isJump", false);

        // Make sure the hitbox is turned off when the game starts!
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
        resetLayerMask = LayerMask.GetMask("Reset");
        spawnPosition = transform.position;
    }

    void Update()
    {
        // --- NEW: ATTACK INPUT ---
        // Only allow attacking if 'J' is pressed AND we aren't already attacking
        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0)
        {
            Attack();
        }

        // --- NEW: ATTACK TIMER ---
        // If the timer is running, count it down. When it hits 0, turn off the hitbox.
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0 && attackHitbox != null)
            {
                attackHitbox.SetActive(false);
            }
        }


        bool isGrounded = groundCheck.IsTouchingLayers(groundLayerMask);
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            animator.SetBool("isJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJump", true);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCut);
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // --- NEW: Attack Logic ---
    private void Attack()
    {
        // Turn on the invisible collision box
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }

        // Start the countdown timer
        attackTimer = attackDuration;

        // Trigger the attack animation in the Animator
        // (Make sure to add a "Trigger" parameter named "Attack" in your Animator!)
        animator.SetTrigger("Attack");
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & resetLayerMask) != 0)
        {
            Respawn();
        }
    }
    private void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = spawnPosition;
    }
}