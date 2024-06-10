using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nam : MonoBehaviour
{
   
    [SerializeField]
    private float leftBoundary = 0f;
    [SerializeField]
    private float rightBoundary = 0f;
    [SerializeField]
    private float movespeed = 1f;
    
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
        if (tag == "arrow")
        {
            Destroy(gameObject);
        }
    }

    private void FlipSprite()
    {
        transform.localScale = isMovingRight ?
            new Vector2(-8.466015f, 9.145375f)
            : new Vector2(8.466015f, 9.145375f);
    }
}
