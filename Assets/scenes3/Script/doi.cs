using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doi : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float leftBoundary = 0f;
    [SerializeField]
    private float rightBoundary = 0f;
    [SerializeField]
    private float movespeed = 1f;
    // gia su dang di chuyen qua phai
    private bool isMovingRight = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //lay vi tri hien tai
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightBoundary)
        {
            // neu vi tri hien tai nho hon left thi di chuyen qua right
            isMovingRight = false;
        }
        else if (currentPosition.x < leftBoundary)
        {
            // neu vi tri hien tai nho hon right thi di chuyen qua left
            isMovingRight = true;
        }


        // tu dong di chuyen ngang
        //(1, 0, 0) * 1 * 0.02 = (0.02, 0, 0)
        var direction = Vector3.right;
        if (isMovingRight == false)
        {
            direction = Vector3.left;
        }
        //var direction = isMovingRight ? Vector3.right : Vector3.left; //cach viet tat
        transform.Translate(direction * movespeed * Time.deltaTime);

        FlipSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        var tag = other.gameObject.tag;
        Bekilled(tag);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        var tag = other.gameObject.tag;
        Bekilled(tag);
    }
    private void Bekilled(string tag)
    {
        if (tag == "viendan")
        {
            Destroy(gameObject);
        }
    }

    private void FlipSprite()
    {
        transform.localScale = isMovingRight ?
            new Vector2(-5.324105f, 4.930731f)
            : new Vector2(5.324105f, 4.930731f);
    }
}
