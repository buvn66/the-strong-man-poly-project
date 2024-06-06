using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using System;

public class Playercontrols : MonoBehaviour
{


    //làm cho nhân vật duy chuyển 
    //public là hàm tồn tại ở mọi nơi 
    //private làm hàm tồ tại chỉ trong một class
    [SerializeField] //SerializeField cho phép chỉnh sửa tr edit
    public float movespeed = 5f;


    // giá tri lực nhẩy
    [SerializeField]
    private float _jumpForce = 40f;

    //cầu thang
   // [SerializeField] float climbingspeed = 5f;


    //kiểm tra hướng duy  chuyển của nhân vật 
    private bool isMovingRight = true;


    //tham chiếu tới rigibody 2D        
    private Rigidbody2D _rigibody2D;


    //tham chiếu tới BoxCollider2D
    private BoxCollider2D _boxCollider2D;

    //tham chiếu tới animator
    private Animator _animator;

    //hàm start dùng để khởi tạo các  giá trị của biến 
    float _startgravityscale;
    // Thêm biến moveInput ở đây
   // private Vector2 moveInput; 
    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigibody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _startgravityscale = _rigibody2D.gravityScale;
    }


    //dùng để cập nhật trạ thái của đối tượng dựa trên thời ggian thật  
    private void Update()
    {
        Move();
        Jump();
        Fire();
       // ClimbLander();
    }

    private void Fire()
    {
        //neu nhan phim F thi ban dan
        //if (Input.GetKeyDown(KeyCode.F))
        //{
            //tao ra vien dan tai vi tri sung
           // var oneArrow = Instantiate(arrowPrefabs, arrowTransform.position, Quaternion.identity);
            //cho vien dan bay theo huong nhan vat
            //var velocity = new Vector2(50f, 0);
            //if (_isMovingRight == false)
            //{
                //velocity = new Vector2(-50f, 0);
           // }
            //oneArrow.GetComponent<Rigidbody2D>().velocity = velocity;
            //huy vien dan sau 2s
            //Destroy(oneArrow, 2f);
        //}
       // if (Input.GetKey(KeyCode.F))
        //{
            _animator.SetBool("isAttacking", true);
        //}
        //else if (Input.GetKey(KeyCode.F))
        //{
        //    _animator.SetBool("isAttacking", false);
        //}
      // else
        //{
         //   _animator.SetBool("isAttacking", false);
       //}
    }

    private void Move()
    {
        //horizontalInput lắng nghe các phím điều hướng 
        var horizontalInput = Input.GetAxis("Horizontal");
        //điều khiển phải trái
        transform.localPosition += new Vector3(horizontalInput, 0, 0)
            * movespeed * Time.deltaTime;
        //+= là lấy giá tri ban đầu tạo ra giá trị mới

        if (horizontalInput > 0)
        {
            //qua phải
            isMovingRight = true;
            _animator.SetBool("isRunning", true);
            //_animator.SetBool("Isjump", true);
        }
        else if (horizontalInput < 0)
        {
            //qua trái 
            isMovingRight = false;
            _animator.SetBool("isRunning", false);
            //_animator.SetBool("Isjump", false);
        }
        else
        {
            //đứng yên 
            //_animator.SetBool("isRunning", false);
        }
        //xoay nhân vật 
        transform.localScale = isMovingRight ?
            new Vector2(1f, 1f)
            : new Vector2(-1f, 1);
        // Gán giá trị cho moveInput
        //moveInput = new Vector2(horizontalInput, Input.GetAxis("Vertical")); 
    }


    //hàm sử lý Jump
    private void Jump()
    {
        //kiểm tra nhân vật còn trên mặt đất hay không
        var Check = _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("platform"));
        if (Check == false)
        {
            return;
        }
        var verticalInput = Input.GetAxis("Jump");
        if (verticalInput > 0)
        {
            //1. tạo lực nhảy lên trên.
            //_rigidbody2D.AddForce(new Vector2(0, _jumpForce));
            _rigibody2D.velocity = new Vector2(_rigibody2D.velocity.x, _jumpForce);
        }
    }
    //void ClimbLander()
    //{
       // if (!_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        //{
          //  _rigibody2D.gravityScale = _startgravityscale;
            //return;
       // }
      //  _rigibody2D.velocity = new Vector2(_rigibody2D.velocity.x, moveInput.y * climbingspeed);
       // _rigibody2D.gravityScale = 0;

   // }
}
