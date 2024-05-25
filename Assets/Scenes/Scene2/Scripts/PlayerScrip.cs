using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float moveInput;
    public float speed;
    private bool isGround = true;
    private bool isDoubleJumpAvailable = false; // Thêm biến để kiểm tra xem nhảy kép có sẵn hay không
    public float jumpForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleJumping();
        HandleMovement();
        UpdateAnimation();
    }

    void HandleMovement()
    {
        moveInput = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * speed, rb.velocity.y);
        rb.velocity = moveVelocity;

        if (moveInput != 0)
        {
            animator.SetBool("run Player", true);
            if (moveInput < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (moveInput > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            animator.SetBool("run Player", false);
        }
    }

    void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                Jump();
            }
            else if (isDoubleJumpAvailable)
            {
                Jump();
                isDoubleJumpAvailable = false;
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        if (!isGround)
        {
            animator.SetTrigger("jump double"); // Kích hoạt animation cho nhảy kép
        }
        else
        {
            animator.SetTrigger("jump Player"); // Kích hoạt animation cho nhảy
        }
        if (!isGround && !isDoubleJumpAvailable)
        {
            isDoubleJumpAvailable = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            isDoubleJumpAvailable = false; // Reset trạng thái nhảy kép khi chạm đất
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }
    }

    void UpdateAnimation()
    {
        animator.SetBool("IsGrounded", isGround); // Cập nhật trạng thái đất
    }
}
