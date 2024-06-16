using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOn : MonoBehaviour
{
    void Start()
    {

    }

    [SerializeField]
    private float leftBoundary;
    [SerializeField]
    private float rifgtBoundary;
    [SerializeField]
    private float moveSpeed = 1f;

    // gia su slime dang di chuyen 
    private bool _isMovingRight = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        // lay vi tri hien tai
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rifgtBoundary)
        {
            // neu vi tri hien tai cua slime > rightBoundary
            // di chuyen sang trai
            _isMovingRight = false;
        }
        else if (currentPosition.x < leftBoundary)
        {
            // neu vi tri hien tai cua slime > leftBoundary
            // di chuyen sang phai
            _isMovingRight = true;
        }
        // di chuyen ngang 
        //(1,0,0) * 1 *0.02 = (0.02, 0, 0)
        var direction = Vector3.right;
        if (_isMovingRight == false)
        {
            direction = Vector3.left;
        }
        //var direction = _isMovingRight ? Vector3.right : Vector3.left;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
