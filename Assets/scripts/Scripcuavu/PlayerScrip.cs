using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private bool isGround = true;
    private int jumpCount = 0;
    public int maxJumpCount = 2;
    public float jumpForce = 5f;
    public TMP_Text coinText;
    private int coinCount = 0;
    public int countJumpCoin = 0;
    public ParticleSystem showCoinParticle;
    private bool isFacingRight = true;
    private Animator animator;

    void Start()
    {
        InitializeComponents();
        UpdateCoinText();
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();

    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * speed, rb.velocity.y);
        rb.velocity = moveVelocity;

        bool isRunning = Mathf.Abs(moveInput) > 0;
        animator.SetBool("isRunning", isRunning);

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Kiểm tra nếu nhân vật đang đứng trên mặt đất hoặc chưa đạt tới maxJumpCount
            if (isGround || jumpCount < maxJumpCount)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpCount++;

                // Kích hoạt animation nhảy ngay lập tức khi nhân vật bắt đầu nhảy
                animator.SetTrigger("Jump");

                // Nếu không đứng trên mặt đất ngay sau khi nhảy, kích hoạt animation double jump
                if (!isGround && jumpCount == 2)
                {
                    animator.SetTrigger("DoubleJump");
                }
            }
        }
        // Kiểm tra khi nhân vật rời khỏi mặt đất
        else if (!isGround)
        {
            // Nếu đã nhấn nhảy lần 2 và nhân vật rời khỏi mặt đất, kích hoạt animation DoubleJump
            if (jumpCount == 2)
            {
                animator.SetTrigger("DoubleJump");
            }
            // Nếu chỉ nhấn nhảy 1 lần và nhân vật rời khỏi mặt đất, kích hoạt animation Jump
            else if (jumpCount == 1)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(rb.velocity.x, verticalInput * speed);
            rb.velocity = climbVelocity;

            bool isClimbing = Mathf.Abs(verticalInput) > 0;
            animator.SetBool("isClimbing", isClimbing);

            if (isClimbing)
            {
                // Khi nhân vật đang leo lên cầu thang, vô hiệu hóa trọng lực để ngăn nhân vật rơi xuống
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = 1;
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            jumpCount = 0;
            animator.SetBool("IsGrounded", true);
        }
        else if (collision.gameObject.tag == "brick")
        {
            countJumpCoin++;
            coinCount++;
            coinText.text = coinCount + "";
            showCoinParticle.Play();

            if (countJumpCoin == 5)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
            animator.SetBool("IsGrounded", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("coinn"))
        {
            Animator animator = other.gameObject.GetComponent<Animator>();
            animator.Play("coin_kill");

            coinCount++;
            UpdateCoinText();
            Destroy(other.gameObject, 0.57f);
        }
    }

    void UpdateCoinText()
    {
        coinText.text = coinCount.ToString();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
