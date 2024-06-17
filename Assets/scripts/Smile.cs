using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Smile : MonoBehaviour
{
    [SerializeField]
    private float leftBoundary;
    [SerializeField]
    private float rifgtBoundary;
    [SerializeField]
    private float moveSpeed = 1f;

    // gia su slime dang di chuyen 
    private bool _isMovingRight = true;
    // Start is called before the first frame update
    void Start()
    {

    }

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


        // scale hien tai
        // mat : trai > 0, phai < 0
        var currentScale = transform.localScale;
        if (

            (_isMovingRight == false && currentScale.x > 0) ||
          (_isMovingRight == true && currentScale.x < 0)

          )
        {
            currentScale.x *= -1;
        }
        else if (_isMovingRight == true && currentScale.x < 0)
        {
            currentScale.x *= -1;
        }
        transform.localScale = currentScale;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var name = other.gameObject.name;
        var tag = other.gameObject.tag;
        if (tag == "arrow")
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        var tag = other.gameObject.tag;
        if (tag == "arrow")
        {
            Destroy(gameObject);
        }
    }
}