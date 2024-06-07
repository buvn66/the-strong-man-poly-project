using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemiescript : MonoBehaviour
{
    public float moveSpeed = 2f; // Tốc độ di chuyển của quái
    private Transform player; // Đối tượng nhân vật (Player)
    public float start, end; // điểm bắt đầu , điểm kết thúc 
    private bool isLeft = true;
    public float followDistance = 3f; // Khoảng cách để nhân vật lại gần con quái
    public GameObject coinPrefab; // Prefab của coin
    public int minCoinCount = 1; // Số lượng coin tối thiểu được sinh ra
    public int maxCoinCount = 3; // Số lượng coin tối đa được sinh ra
    private int coinCount = 0; // Số coin
    public TMP_Text coinText; // TextMeshPro để hiển thị số coin
    private bool isDead = false; // Trạng thái khi quái vật bị trúng đạn

    void Start()
    {
        // Tìm đối tượng nhân vật (Player)
        player = GameObject.FindGameObjectWithTag("Boss").transform;
        // Cập nhật số coin lên TMPro
        UpdateCoinText();
    }

    void Update()
    {
        // Kiểm tra xem quái vật có bị trúng đạn không
        if (isDead)
        {
            // Di chuyển quái vật xuống dưới
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            return;
        }

        // Kiểm tra xem nhân vật có tồn tại không
        if (player != null)
        {
            // Tính toán khoảng cách giữa quái và nhân vật
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Kiểm tra xem nhân vật có nằm trong khoảng cách để bắt đầu đuổi không
            if (distanceToPlayer <= followDistance)
            {
                // Tính toán hướng di chuyển của nhân vật để tiếp cận con quái
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // Di chuyển quái theo hướng tính toán được
                transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
            }
        }

        // Di chuyển con quái giữa hai điểm start và end
        MoveBetweenPoints();
    }

    // Hàm di chuyển con quái giữa hai điểm start và end
    void MoveBetweenPoints()
    {
        var bossX = transform.position.x;
        var scaleBoss = transform.localScale;

        if (bossX <= start)
        {
            isLeft = false;
        }
        if (bossX >= end)
        {
            isLeft = true;
        }
        if (isLeft)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 1.5f);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * 1.5f);
        }
        scaleBoss.x = isLeft ? 1f : -1f;
        transform.localScale = scaleBoss;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            isLeft = !isLeft;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bulet")
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();//đạn băn squais sẽ rơi xuống 
            collider.isTrigger = true;
            // Sinh ra coin 
            DropCoin();
            // Hủy quái vật
            Destroy(gameObject, 0.9f);
        }
    }

    void DropCoin()
    {
        // Sinh ra một số lượng coin ngẫu nhiên từ minCoinCount đến maxCoinCount
        int coinCount = Random.Range(minCoinCount, maxCoinCount + 1);
        for (int i = 0; i < coinCount; i++)
        {
            Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f), Quaternion.identity);
        }
        // Hủy quái vật sau khi rớt xuống
        Destroy(gameObject, 2f);
    }

    void UpdateCoinText()
    {
        // Cập nhật số coin lên TextMeshPro
        coinText.text = coinCount.ToString();   
    }
}
