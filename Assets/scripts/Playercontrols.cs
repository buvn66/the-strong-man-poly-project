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


    //kiểm tra hướng duy chuyển của nhân vật 
    private bool isMovingRight = true;


    //tham chiếu tới rigibody 2D        
    private Rigidbody2D _rigibody2D;


    //tham chiếu tới BoxCollider2D
    private BoxCollider2D _boxCollider2D;

    //tham chiếu tới animator
    //private Animator _animator;

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



    //hàm start dùng để khởi tạo các  giá trị của biến 
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigibody2D = GetComponent<Rigidbody2D>();
        //_animator = GetComponent<Animator>();
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
    }


    //hàm sử lý bow bắng ra được arrow
    private void bow()
    {
        if (Input.GetKeyDown(KeyCode.F))
        //nếu người chơi nhấn phím F
        {
            //tạo ra viên đạn tại vị trí của súng
            var arrow = Instantiate(_arrowprefab, _bow.position, Quaternion.identity);
            //cho viên đạn bay theo hướng của nhân vật
            var velocity = new Vector3(50f, 0);
            if (isMovingRight == false)
            {
                velocity.x *= -1;
            }
            arrow.GetComponent<Rigidbody2D>().velocity = velocity;
            //huy viên đạn sao 2s
            Destroy(arrow, 1f);
        }
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
            //_animator.SetBool("Isrunning", true);
            //_animator.SetBool("Isjump", true);
        }
        else if (horizontalInput < 0)
        {
            //qua trái 
            isMovingRight = false;
            //_animator.SetBool("Isrunning", false);
            //_animator.SetBool("Isjump", false);
        }
        else
        {
            //đứng yên 
            //_animator.SetBool("Isrunning", false);
        }
        //xoay nhân vật 
        transform.localScale = isMovingRight ?
            new Vector2(1f, 1f)
            : new Vector2(-1f, 1);
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        //nếu va chạm với 
        if (other.gameObject.CompareTag("coins"))
        {
            //biến mất đồng xu
            Destroy(other.gameObject);
            //phát ra tiếng nhạc
            _audioSource.PlayOneShot(_coinCollectSXF);
            //tăng điêm
            _score += 1;
            //hiểm thị điểm
            _scoreText.text = _score.ToString();
        }
        //nếu va chạm với Enemies
        else if (other.gameObject.CompareTag("Enemies"))
        {
            _lives -= 1;
            //hiển thi live images
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
            //reload game 
            if (_lives > 0)
            {
                //reload game 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            //hiện gameover panel
            else
            {
                //hiện gameover panel
                _gameOverpanel.SetActive(true);
                //dừng game 
                Time.timeScale = 0;
            }
        }
        //đụng vào boss
        else if (other.gameObject.CompareTag("boss"))
        {
            _lives -= 1;
            //hiển thi live images
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
            if (_lives > 0)
            {
                //reload game 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                //hiện gameover panel
                _gameOverpanel.SetActive(true);
                //dừng game 
                Time.timeScale = 0;
            }
        }
        // đụng vào bẫy
        else if (other.gameObject.CompareTag("trap"))
        {
            _lives -= 1;
            //hiển thi live images
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
            if (_lives > 0)
            {
                //reload game 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                //hiện gameover panel
                _gameOverpanel.SetActive(true);
                //dừng game 
                Time.timeScale = 0;
            }
        }

    }
}
