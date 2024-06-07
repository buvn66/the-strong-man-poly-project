using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
=======
    public float speed; // Tốc độ
    private bool isGround = true; // Kiểm tra xem người chơi có đang đứng trên mặt đất không
    private int jumpCount = 0; // Số lần nhảy đã thực hiện
    public int maxJumpCount = 2; // Số lần nhảy tối đa
    public float jumpForce = 5f;
    public List<GameObject> bulletPrefabs; // Danh sách các loại đạn
    public int currentBulletIndex = 0; // Chỉ mục của loại đạn hiện tại
    public TMP_Text bulletText; // Hiển thị số đạn còn lại 
    private int bulletCount = 10; // Số đạn còn lại 
    public TMP_Text coinText; // Hiển thị số đồng xu
    private int coinCount = 0; // Số đồng xu
    public ParticleSystem showCoinParticle;
    private bool isFacingRight = true; // Biến để xác định hướng của nhân vật
    public float bulletSpeed = 10f; // Tốc độ của viên đạn
    public int heartCount = 3; // Số tim hiện tại
    public TMP_Text heartText; // Hiển thị số tim còn lại
    public Vector3 respawnPosition; // Vị trí hồi sinh
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes

    void Start()
    {
        InitializeComponents();
<<<<<<< Updated upstream
        UpdateCoinText();
=======
<<<<<<< HEAD
        UpdateCoinText();
=======
        UpdateBulletText();
        UpdateCoinText();
        UpdateHeartText();
        respawnPosition = transform.position; // Đặt vị trí hồi sinh ban đầu là vị trí hiện tại của nhân vật
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
<<<<<<< Updated upstream

=======
<<<<<<< HEAD

=======
        HandleShooting();
        HandleChangeBulletType();
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
<<<<<<< Updated upstream
        animator = GetComponent<Animator>();
=======
<<<<<<< HEAD
        animator = GetComponent<Animator>();
=======
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * speed, rb.velocity.y);
        rb.velocity = moveVelocity;
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> Stashed changes

        bool isRunning = Mathf.Abs(moveInput) > 0;
        animator.SetBool("isRunning", isRunning);

<<<<<<< Updated upstream
=======
=======
        // Xác định hướng di chuyển và cập nhật hình dạng của nhân vật
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> Stashed changes
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


<<<<<<< Updated upstream
=======
=======
        if (Input.GetKeyDown(KeyCode.Space) && (isGround || jumpCount < maxJumpCount))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.F) && bulletCount > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 bulletPosition = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.5f;
        GameObject selectedBulletPrefab = bulletPrefabs[currentBulletIndex];
        GameObject bullet = Instantiate(selectedBulletPrefab, bulletPosition, Quaternion.identity);

        // Tính toán hướng bay của viên đạn dựa trên hướng nhìn của nhân vật
        Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;

        // Lấy component Rigidbody2D của viên đạn để thay đổi vận tốc
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Đảm bảo rằng viên đạn không bị ảnh hưởng bởi trọng lực và di chuyển theo hướng mong muốn
        bulletRb.gravityScale = 0;
        bulletRb.velocity = shootDirection * bulletSpeed;

        // Hủy viên đạn sau 3 giây
        Destroy(bullet, 3f);

        bulletCount--;
        UpdateBulletText();
    }

    void HandleChangeBulletType()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentBulletIndex = (currentBulletIndex + 1) % bulletPrefabs.Count;
        }
    }
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            jumpCount = 0;
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
=======
        }
        else if (collision.gameObject.tag == "brick")
        {
            coinCount++; // Số lượng coin hiện có cộng lên 1
            coinText.text = coinCount.ToString();
            showCoinParticle.Play();

            Destroy(collision.gameObject); // Xóa brick sau khi va chạm
        }
        else if (collision.gameObject.CompareTag("Spikes"))
        {
            heartCount--; // Trừ một tim
            UpdateHeartText();
            if (heartCount <= 0)
            {
                // Khi tim hết, nhân vật biến mất
                gameObject.SetActive(false);
            }
            else
            {
                // Di chuyển nhân vật về vị trí hồi sinh
                transform.position = respawnPosition;
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
<<<<<<< Updated upstream
            animator.SetBool("IsGrounded", false);
=======
<<<<<<< HEAD
            animator.SetBool("IsGrounded", false);
=======
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
<<<<<<< Updated upstream
        if (other.CompareTag("coinn"))
=======
<<<<<<< HEAD
        if (other.CompareTag("coinn"))
=======
        if (other.CompareTag("bulletPickup"))
        {
            bulletCount++;
            UpdateBulletText();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("coinn"))
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
        {
            Animator animator = other.gameObject.GetComponent<Animator>();
            animator.Play("coin_kill");

            coinCount++;
            UpdateCoinText();
            Destroy(other.gameObject, 0.57f);
        }
    }

<<<<<<< Updated upstream
=======
<<<<<<< HEAD
=======
    void UpdateBulletText()
    {
        bulletText.text = bulletCount.ToString();
    }

>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
    void UpdateCoinText()
    {
        coinText.text = coinCount.ToString();
    }

<<<<<<< Updated upstream
=======
<<<<<<< HEAD
=======
    void UpdateHeartText()
    {
        heartText.text = heartCount.ToString();
    }

    // Phương thức để đảo ngược hình dạng của nhân vật
>>>>>>> 5209313c992587e0ab42d7809a9cc8c5dfcb913a
>>>>>>> Stashed changes
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
