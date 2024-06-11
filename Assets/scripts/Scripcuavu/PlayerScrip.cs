using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Để sử dụng UI elements như Panel

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public float speed;
    private bool isGround = true;
    private int jumpCount = 0;
    public int maxJumpCount = 2;
    public float jumpForce = 5f;
    public List<GameObject> bulletPrefabs;
    public int currentBulletIndex = 0;
    public TMP_Text bulletText;
    private int bulletCount = 10;
    public TMP_Text coinText;
    private int coinCount = 0;
    public ParticleSystem showCoinParticle;
    private bool isFacingRight = true;
    public float bulletSpeed = 10f;
    public int heartCount = 3;
    public TMP_Text heartText;
    public Vector3 respawnPosition;
    private bool isOnLadder = false;
    [SerializeField]
    private AudioClip _coinCollectSXF;
    private AudioSource _audioSource;
    // Biến để tham chiếu đến Panel "You Died"
    public GameObject youDiedPanel;

    void Start()
    {
        InitializeComponents();
        UpdateBulletText();
        UpdateCoinText();
        UpdateHeartText();
        respawnPosition = transform.position;
        _audioSource = GetComponent<AudioSource>();
        youDiedPanel.SetActive(false); // Đảm bảo Panel bắt đầu ở trạng thái tắt
    }

    void Update()
    {
        if (isOnLadder)
        {
            HandleClimbing();
        }
        else
        {
            HandleMovement();
            HandleJumping();
            HandleShooting();
            HandleChangeBulletType();
        }
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

        animator.SetBool("isRunning", moveInput != 0);

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

        Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.gravityScale = 0;
        bulletRb.velocity = shootDirection * bulletSpeed;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            jumpCount = 0;
            animator.SetBool("leocau", false);
        }
        else if (collision.gameObject.CompareTag("Spikes"))
        {
            heartCount--;
            UpdateHeartText();
            if (heartCount <= 0)
            {
                HandleDeath(); // Gọi phương thức khi nhân vật chết
            }
            else
            {
                transform.position = respawnPosition;
            }
        }
        else if (collision.gameObject.CompareTag("Boss")) // Xử lý va chạm với quái
        {
            heartCount--; // Giảm máu khi đụng quái
            UpdateHeartText(); // Cập nhật hiển thị máu trên thanh text
            if (heartCount <= 0)
            {
                HandleDeath(); // Gọi phương thức khi nhân vật chết
            }
            else
            {
                transform.position = respawnPosition; // Quay lại điểm xuất phát nếu chưa hết máu
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = true;
            animator.SetBool("leocau", true);
            rb.gravityScale = 0; // Tắt trọng lực khi leo
        }
        else if (other.CompareTag("bulletPickup"))
        {
            bulletCount++;
            UpdateBulletText();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("coinn"))
        {
            Animator animator = other.gameObject.GetComponent<Animator>();
            animator.Play("coin_kill");
            _audioSource.PlayOneShot(_coinCollectSXF);
            coinCount++;
            UpdateCoinText();
            Destroy(other.gameObject, 0.57f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = false;
            animator.SetBool("leocau", false);
            rb.gravityScale = 1; // Bật trọng lực lại khi không leo nữa
        }
    }

    void UpdateBulletText()
    {
        bulletText.text = bulletCount.ToString();
    }

    void UpdateCoinText()
    {
        coinText.text = coinCount.ToString();
    }

    void UpdateHeartText()
    {
        heartText.text = heartCount.ToString();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void HandleClimbing()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * speed);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    void HandleDeath()
    {
        youDiedPanel.SetActive(true); // Hiển thị panel "You Died"
        gameObject.SetActive(false); // Vô hiệu hóa nhân vật
    }
}
