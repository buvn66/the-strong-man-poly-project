using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //máu của Boss 
    private float _health = 10;

    [SerializeField]
    private Slider _healthSlider;
=======
using UnityEngine;

public class Dragoncontrols : MonoBehaviour
{
>>>>>>> Stashed changes

    [SerializeField]
    private float leftBoundary;
    [SerializeField]
    private float rightBoundary;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private bool _isMovingRight = true;
<<<<<<< Updated upstream

    private void Start()
    {
        _healthSlider.value = _health;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            //hủy viên đạn 
            Destroy(other.gameObject);
            //mổi lần trung bullet -10 máu của boss  
            _health -= 1;
            _healthSlider.value = _health;
            if (_health <= 0)
            {
               

                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        //lấy vị trí hiện tại của ốc
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightBoundary)
        {
<<<<<<< HEAD
            //nếu vị trí hiện tại  < rightboundary 
=======
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //lấy vị trí hiện tại dragon
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightBoundary)
        {
            //nếu vị trí hiện tại Dragon tboundary 
>>>>>>> Stashed changes
=======
            //nếu vị trí hiện tại của ốc < rightboundary 
>>>>>>> parent of c5b48b4 (mini boss)
            // di chuyển trái
            _isMovingRight = false;

        }
        else if (currentPosition.x < leftBoundary)
        {
<<<<<<< HEAD
<<<<<<< Updated upstream
            //nếu vị trí hiện tại  < leftboundary 
=======
            //nếu vị trí hiện tại của Dragon ndary 
>>>>>>> Stashed changes
=======
            //nếu vị trí hiện tại của ốc < leftboundary 
>>>>>>> parent of c5b48b4 (mini boss)
            // di chuyển phải   
            _isMovingRight = true;
        }

        //tự động di chuyển ngang
        var direction = Vector3.right;
        if (_isMovingRight == false)
        {
            direction = Vector3.left;
        }
        transform.Translate(direction * moveSpeed * Time.deltaTime);
<<<<<<< HEAD
<<<<<<< Updated upstream
        //xoay mặt 
=======
        //xoay mặt boss
>>>>>>> Stashed changes
=======
        //xoay mặt enemies
>>>>>>> parent of c5b48b4 (mini boss)
        //scale hiện tại
        var currentScale = transform.localScale;
        if (_isMovingRight && currentScale.x > 0)
        {
            currentScale.x *= -1;
        }
        else if (_isMovingRight == false && currentScale.x < 0)
        {
            currentScale.x *= -1;
        }
        transform.localScale = currentScale;

<<<<<<< Updated upstream
    }
}
=======
        }
    }
>>>>>>> Stashed changes
