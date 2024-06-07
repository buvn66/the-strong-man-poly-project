using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
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
    public int countJumpCoin = 0;
    public ParticleSystem showCoinParticle;
    private bool isFacingRight = true; // Biến để xác định hướng của nhân vật
    public float bulletSpeed = 10f; // Tốc độ của viên đạn

    void Start()
    {
        InitializeComponents();
        UpdateBulletText();
        UpdateCoinText();
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleShooting();
        HandleChangeBulletType();
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * speed, rb.velocity.y);
        rb.velocity = moveVelocity;
        // Xác định hướng di chuyển và cập nhật hình dạng của nhân vật
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            jumpCount = 0;
        }
        else if (collision.gameObject.tag == "brick")
        {
            countJumpCoin++;//check số lần nhẩy lên aen coin 

            coinCount++;//số lượng coin hiện có cộng lên 1

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
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bulletPickup"))
        {
            bulletCount++;
            UpdateBulletText();
            Destroy(other.gameObject);
            // Xác định hướng của nhân vật

        }
        else if (other.CompareTag("coinn"))
        {
            Animator animator = other.gameObject.GetComponent<Animator>();
            animator.Play("coin_kill");

            coinCount++;
            UpdateCoinText();
            Destroy(other.gameObject, 0.57f);
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
    // Phương thức để đảo ngược hình dạng của nhân vật
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
