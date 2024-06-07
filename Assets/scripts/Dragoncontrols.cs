using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //máu của Boss 
    private float _health = 10;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private float leftBoundary;
    [SerializeField]
    private float rightBoundary;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private bool _isMovingRight = true;

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
            //mổi lần trung arrow -10 máu của boss  
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
        //lấy vị trí hiện tại 
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightBoundary)
        {
            //nếu vị trí hiện tại  < rightboundary 
            // di chuyển trái
            _isMovingRight = false;

        }
        else if (currentPosition.x < leftBoundary)
        {
            //nếu vị trí hiện tại  < leftboundary 
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
        //xoay mặt 
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

    }
}
