using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doi : MonoBehaviour
{
    
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
        
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightBoundary)
        {
            
            isMovingRight = false;
        }
        else if (currentPosition.x < leftBoundary)
        {
          
            isMovingRight = true;
        }


        
        var direction = Vector3.right;
        if (isMovingRight == false)
        {
            direction = Vector3.left;
        }
        
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
