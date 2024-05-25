using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed; // T?c ??
    private bool isGround = true; // Ki?m tra xem ng??i ch?i có ?ang ??ng trên m?t ??t không
    private int jumpCount = 0; // S? l?n nh?y ?ã th?c hi?n
    public int maxJumpCount = 2; // S? l?n nh?y t?i ?a
    public float jumpForce = 5f;
    public List<GameObject> bulletPrefabs; // Danh sách các lo?i ??n
    public int currentBulletIndex = 0; // Ch? m?c c?a lo?i ??n hi?n t?i
    public TMP_Text bulletText; // Hi?n th? s? ??n còn l?i 
    private int bulletCount = 10; // S? ??n còn l?i 
    public TMP_Text coinText; // Hi?n th? s? ??ng xu
    private int coinCount = 0; // S? ??ng xu
    public GameObject boxColorfull;
    public int countJumpCoin = 0;
    public ParticleSystem showCoinParticle;
    private bool isFacingRight = true; // Bi?n ?? xác ??nh h??ng c?a nhân v?t
    public float bulletSpeed = 10f; // T?c ?? c?a viên ??n

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
        // Xác ??nh h??ng di chuy?n và c?p nh?t hình d?ng c?a nhân v?t
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
        if (Input.GetKeyDown(KeyCode.B) && bulletCount > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 bulletPosition = transform.position + Vector3.right * 0.5f;
        GameObject selectedBulletPrefab = bulletPrefabs[currentBulletIndex];
        GameObject bullet = Instantiate(selectedBulletPrefab, bulletPosition, Quaternion.identity);

        // Tính toán h??ng bay c?a viên ??n d?a trên h??ng nhìn c?a nhân v?t
        Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;

        // ??o h??ng viên ??n n?u nhân v?t ?ang quay trái
        if (!isFacingRight)
        {
            bullet.transform.Rotate(0f, 180f, 0f);
        }

        // L?y component Rigidbody2D c?a viên ??n ?? thay ??i v?n t?c
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Thay ??i v?n t?c c?a viên ??n ?? nó di chuy?n theo h??ng mong mu?n
        bulletRb.velocity = shootDirection * bulletSpeed;

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
            countJumpCoin++;//check s? l?n nh?y lên aen coin 

            coinCount++;//s? l??ng coin hi?n có c?ng lên 1

            coinText.text = coinCount + "";
            showCoinParticle.Play();

            if (countJumpCoin == 5)
            {
                Destroy(collision.gameObject);
                Instantiate(boxColorfull,
                    collision.gameObject.transform.position,
                    collision.gameObject.transform.rotation);
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
            // Xác ??nh h??ng c?a nhân v?t

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
    // Ph??ng th?c ?? ??o ng??c hình d?ng c?a nhân v?t
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
