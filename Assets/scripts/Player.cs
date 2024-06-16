using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using System;

public class Player : MonoBehaviour
{


    //làm cho nhân vật duy chuyển 
    //public là hàm tồn tại ở mọi nơi 
    //private làm hàm tồ tại chỉ trong một class
    [SerializeField] //SerializeField cho phép chỉnh sửa tr edit
    public float movespeed = 5f;


    // giá tri lực nhẩy
    [SerializeField]
    private float _jumpForce = 40f;


    //kiểm tra hướng duy chuyển của nhân vật 
    private bool isMovingRight = true;


    //tham chiếu tới rigibody 2D        
    private Rigidbody2D _rigibody2D;


    //tham chiếu tới BoxCollider2D
    private BoxCollider2D _boxCollider2D;

    //tham chiếu tới animator
    private Animator _animator;

    //tham chiếu đến arrow
    [SerializeField]
    private GameObject _arrowprefab;


    ////tham chiếu đến bow
    [SerializeField]
    private Transform _bow;


    //tham chiếu đến file suond
    [SerializeField]
    private AudioClip _coinCollectSXF;


    //tham chiếu đến ngồn âm thanh 
    private AudioSource _audioSource;


    //tham chiếu đến TextMeshPro
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    private static int _score = 0;


    //tham chiếu đên panel gameover     
    [SerializeField]
    private GameObject _gameOverpanel;
    private static int _lives = 3;


    //tham  chiếu hiện số mạng 
    [SerializeField]
    private TextMeshProUGUI _livesText;
    [SerializeField]
    private GameObject[] _liveImages;
    //Double Jum
    private bool _canDoubleJum = false;


    //hàm start dùng để khởi tạo các  giá trị của biến 
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigibody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        //hiển thị điểm
        _scoreText.text = _score.ToString();
        //hiển thi heart
        for (int i = 0; i < 3; i++)
        {
            if (i < _lives)
            {
                _liveImages[i].SetActive(true);
            }
            else
            {
                _liveImages[i].SetActive(false);
            }
        }
    }


    //dùng để cập nhật trạ thái của đối tượng dựa trên thời ggian thật  
    private void Update()
    {
        Move();
        Jump();
        bow();
        FlipSprite();
    }


    //hàm sử lý bow bắng ra được arrow
    private void bow()
    {
        //neu nhan phim F thi ban dan
        if (Input.GetKeyDown(KeyCode.F))
        {
            //tao ra vien dan tai vi tri sung
            var oneArrow = Instantiate(_arrowprefab, _bow.position, Quaternion.identity);
            //cho vien dan bay theo huong nhan vat
            var velocity = new Vector2(50f, 0);
            if (isMovingRight == false)
            {
                velocity = new Vector2(-50f, 0);
            }
            oneArrow.GetComponent<Rigidbody2D>().velocity = velocity;
            //huy vien dan sau 2s
            Destroy(oneArrow, 2f);
        }
        if (Input.GetKey(KeyCode.F))
        {
            _animator.SetBool("IsAttacking", true);
        }
        //else if (Input.GetKey(KeyCode.F))
        //{
        //    _animator.SetBool("isAttacking", false);
        //}
        else
        {
            _animator.SetBool("IsAttacking", false);
        }
    }

    private void Move()
    {
        // lay gia tri trung ngang left, right, a, d
        var horizontalInput = Input.GetAxis("Horizontal");

        // Dieu khien
        //Global: so sanh voi world
        //local: so sanh voi parent

        transform.localPosition += new Vector3(horizontalInput, 0, 0)
                                  * movespeed * Time.deltaTime;
        if (horizontalInput > 0)
        {
            isMovingRight = true;
            _animator.SetBool("IsRunning", true);

        }
        else if (horizontalInput < 0)
        {
            isMovingRight = false;
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }


    //hàm sử lý Jump
    private void Jump()
    {
        //if (_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platform")) == false)
        //{
        //    return;
        //}
        //var verticalInput = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
        //if (verticalInput > 0)
        //{
        //    _rigidbody.velocity = new Vector2(0, jumpForce);
        //}

        // private void Jump()
        {
            if (_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("platform")))
            {
                _canDoubleJum = true; // reset laij khi player cham dat 
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("platform")) || _canDoubleJum)
                {
                    _rigibody2D.velocity = new Vector2(_rigibody2D.velocity.x, _jumpForce);
                    if (!_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("platform")))
                    {
                        _canDoubleJum = false; // Use double jump
                    }
                }
            }
        }
    }
    private void FlipSprite()
    {
        transform.localScale = isMovingRight ?
        new Vector2(1f, 1f) : new Vector2(-1f, 1f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //nếu va chạm với 
        if (other.gameObject.CompareTag("coins"))
        {
            // lam bien mat xu
            Destroy(other.gameObject);
            // phat ra tieng nhac
            _audioSource.PlayOneShot(_coinCollectSXF);

            //tang diem
            _score++;
            _scoreText.text = _score.ToString();
        }
        else if (other.gameObject.CompareTag("Enemies"))
        {
            //bat su kien Player cham spikes
            //mat 1 mang va reload lai man choi
            _lives -= 1;
            // xoa di 1 anh
            for (int i = 0; i < 3; i++)
            {
                if (i < _lives)
                {
                    _liveImages[i].SetActive(true);
                }
                else
                {
                    _liveImages[i].SetActive(false);
                }

            }
            if (_lives < 1)
            {
                //Hien ra thong bao GameOver
                _gameOverpanel.SetActive(true);
                //Dung game
                Time.timeScale = 0;
            }
            else
            {
                // reload lai man choi hien tai
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (other.gameObject.CompareTag("Spikes"))
        {
            //bat su kien Player cham spikes
            //mat 1 mang va reload lai man choi
            _lives -= 1;
            // xoa di 1 anh
            for (int i = 0; i < 3; i++)
            {
                if (i < _lives)
                {
                    _liveImages[i].SetActive(true);
                }
                else
                {
                    _liveImages[i].SetActive(false);
                }

            }
            if (_lives < 1)
            {
                //Hien ra thong bao GameOver
                _gameOverpanel.SetActive(true);
                //Dung game
                Time.timeScale = 0;
            }
            else
            {
                // reload lai man choi hien tai
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
    }
}
