using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Rigidbody2D rb;
    public CircleCollider2D groundCheck;
    public float jumpCut = 0.5f; 

    private int groundLayerMask;
    [SerializeField] private Animator animator;

    private bool isFacingRight = true;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
        
        groundLayerMask = LayerMask.GetMask("Ground"); 
        animator.SetBool("isJump", false);
    }

    void Update()
    {
        bool isGrounded = groundCheck.IsTouchingLayers(groundLayerMask);
        if (isGrounded) {
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
        
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}