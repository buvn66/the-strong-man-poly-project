using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragoncontrols : MonoBehaviour
{

    [SerializeField]
    private float leftBoundary;
    [SerializeField]
    private float rightBoundary;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private bool _isMovingRight = true;
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
            // di chuyển trái
            _isMovingRight = false;

        }
        else if (currentPosition.x < leftBoundary)
        {
            //nếu vị trí hiện tại của Dragon ndary 
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
        //xoay mặt boss
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
