using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    private Rigidbody2D Lad;
    public float climbSpeed = 3f;
    private bool isClimbing = false;

    //Animation
    //private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        Lad = GetComponent<Rigidbody2D>();
        //_animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            Lad.gravityScale = 0f;
            //_animator.SetBool("isClimbingg", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            Lad.gravityScale = 1f;
            //_animator.SetBool("isClimbingg", false);
        }
    }
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            float ClimbInput = Input.GetAxisRaw("Vertical");
            Lad.velocity = new Vector2(Lad.velocity.x, ClimbInput * climbSpeed);
        }
    }
}
