using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // van toc chuyen dong ngang cua nhan vat
    // public, private, protected, internal, protected internal
    [SerializeField]
    private float moveSpeed = 5f; // 5m/s

    [SerializeField]
    private float jumpForce = 5f;

    // Rigidbody 2D: Vat ly
    private Rigidbody2D _rigidbody;

    // Bien kiem tra huong di
    private bool _isMovingRight = true;

    // tham chieu den mui ten trong prefabs
    public GameObject arrowPrefabs;

    // tham chieu den vi tri cua mui ten
    public Transform arrowTransform;

    // Collider 2D: va cham
    private BoxCollider2D _boxCollider2D;

    //Animation
    private Animator _animator;

    // Tham chieu den TextMeshPro diem so
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    private static int _score = 0;

    // tham chieu TextMeshPro thoi gian
    [SerializeField]
    private TextMeshProUGUI _timeText;
    private static float _time = 0;

    //Phat nhac
    //tham chieu den AudioSource
    //tham chieu den audiClip
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _coinCollectSXF;

    //khai bao bien quan li so mang cua nhan vat
    [SerializeField]
    private static int lives = 3;

    // tham chieu den 3 hinh anh mang
    [SerializeField]
    private GameObject[] _livesImages;

    //khai bao tham chieu den GameOverPanel
    [SerializeField]
    private GameObject _gameOverPanel;

    //Double Jum
    private bool _canDoubleJum = false;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        // Gan gia tri mac dinh cho diem so
        _scoreText.text = _score.ToString();

        for (int i = 0; i < 3; i++)
        {
            if (i < lives)
            {
                _livesImages[i].SetActive(true);
            }
            else
            {
                _livesImages[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        FlipSprite();
        Fire();

        //Dem thoi gian
        _time += Time.deltaTime;
        _timeText.text = $"{_time}";
    }

    private void Fire()
    {
        //neu nhan phim F thi ban dan
        if (Input.GetKeyDown(KeyCode.F))
        {
            //tao ra vien dan tai vi tri sung
            var oneArrow = Instantiate(arrowPrefabs, arrowTransform.position, Quaternion.identity);
            //cho vien dan bay theo huong nhan vat
            var velocity = new Vector2(50f, 0);
            if (_isMovingRight == false)
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
                                  * moveSpeed * Time.deltaTime;
        if (horizontalInput > 0)
        {
            _isMovingRight = true;
            _animator.SetBool("IsRunning", true);

        }
        else if (horizontalInput < 0)
        {
            _isMovingRight = false;
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }

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
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
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
        transform.localScale = _isMovingRight ?
        new Vector2(1f, 1f) : new Vector2(-1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            lives -= 1;
            // xoa di 1 anh
            for (int i = 0; i < 3; i++)
            {
                if (i < lives)
                {
                    _livesImages[i].SetActive(true);
                }
                else
                {
                    _livesImages[i].SetActive(false);
                }

            }
            if (lives < 1)
            {
                //Hien ra thong bao GameOver
                _gameOverPanel.SetActive(true);
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
            lives -= 1;
            // xoa di 1 anh
            for (int i = 0; i < 3; i++)
            {
                if (i < lives)
                {
                    _livesImages[i].SetActive(true);
                }
                else
                {
                    _livesImages[i].SetActive(false);
                }

            }
            if (lives < 1)
            {
                //Hien ra thong bao GameOver
                _gameOverPanel.SetActive(true);
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
