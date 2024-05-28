using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemiescontrols : MonoBehaviour
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
        // Lấy vị trí hiện tại của quái vật
        var currentPosition = transform.localPosition;

        if (currentPosition.x > rightBoundary)
        {
            // Nếu vị trí hiện tại của quái vật > rightBoundary, di chuyển trái
            _isMovingRight = false;
        }
        else if (currentPosition.x < leftBoundary)
        {
            // Nếu vị trí hiện tại của quái vật < leftBoundary, di chuyển phải
            _isMovingRight = true;
        }

        // Tự động di chuyển ngang
        var direction = Vector3.right;
        if (!_isMovingRight)
        {
            direction = Vector3.left;
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Xoay mặt quái vật theo hướng di chuyển
        // Scale hiện tại
        var currentScale = transform.localScale;
        if (_isMovingRight && currentScale.x < 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
        }
        else if (!_isMovingRight && currentScale.x > 0)
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
        }
        transform.localScale = currentScale;
    }

    // Giết quái vật và làm biến mất viên đạn
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            // Nếu chạm tới viên đạn thì quái vật chết
            Destroy(gameObject);
            // Viên đạn biến mất
            Destroy(collision.gameObject);
        }
    }
}
