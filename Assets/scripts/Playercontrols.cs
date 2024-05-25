using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrols : MonoBehaviour
{
    // Làm cho nhân vật di chuyển 
    [SerializeField]
    public float movespeed = 5f;

    // Giá trị lực nhảy
    [SerializeField]
    private float _jumpForce = 40f;

    // Kiểm tra hướng di chuyển của nhân vật 
    private bool isMovingRight = true;

    // Tham chiếu tới Rigidbody2D        
    private Rigidbody2D _rigidbody2D;

    // Tham chiếu tới BoxCollider2D
    private BoxCollider2D _boxCollider2D;

    // Tham chiếu tới Animator
    private Animator _animator;

    // Hàm start dùng để khởi tạo các giá trị của biến 
    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Dùng để cập nhật trạng thái của đối tượng dựa trên thời gian thực  
    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        // Lấy giá trị input từ người dùng
        var horizontalInput = Input.GetAxis("Horizontal");

        // Cập nhật vị trí của nhân vật dựa trên input
        transform.localPosition += new Vector3(horizontalInput, 0, 0) * movespeed * Time.deltaTime;

        // Cập nhật animation dựa trên trạng thái di chuyển
        if (horizontalInput != 0)
        {
            _animator.SetBool("IsRunning", true);

            // Xác định hướng di chuyển của nhân vật và cập nhật hướng mặt của nhân vật
            if (horizontalInput > 0)
            {
                isMovingRight = true;
            }
            else if (horizontalInput < 0)
            {
                isMovingRight = false;
            }

            // Xoay nhân vật theo hướng di chuyển
            transform.localScale = isMovingRight ? new Vector2(1f, 1f) : new Vector2(-1f, 1f);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }

    // Hàm xử lý Jump
    private void Jump()
    {
        // Kiểm tra nhân vật có đang trên mặt đất hay không
        var isGrounded = _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("platform"));

        if (!isGrounded)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            // Tạo lực nhảy lên trên.
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            _animator.SetTrigger("Jump");
        }
    }
}