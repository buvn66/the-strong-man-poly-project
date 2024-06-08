using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class quai2Scrip : MonoBehaviour
{
    public float moveSpeed = 2f; // Tốc độ di chuyển của quái
    public float chaseSpeed = 4f; // Tốc độ di chuyển khi đuổi theo nhân vật
    private Transform player; // Đối tượng nhân vật (Player)
    public float start, end; // điểm bắt đầu, điểm kết thúc 
    private bool isLeft = true;
    public float followDistance = 3f; // Khoảng cách để nhân vật lại gần con quái
    public GameObject coinPrefab; // Prefab của coin
    public int minCoinCount = 1; // Số lượng coin tối thiểu được sinh ra
    public int maxCoinCount = 3; // Số lượng coin tối đa được sinh ra
    private int coinCount = 0; // Số coin
    public TMP_Text coinText; // TextMeshPro để hiển thị số coin
    private bool isDead = false; // Trạng thái khi quái vật bị trúng đạn

    private Animator animator; // Animator component

    private int hitCount = 0; // Biến đếm số lần trúng đạn
    private int maxHits = 3; // Số lần trúng đạn tối đa trước khi chết
    private bool isHit = false; // Trạng thái khi quái vật bị hit
    void Start()
    {
        // Tìm đối tượng nhân vật (Player)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Cập nhật số coin lên TMPro
        UpdateCoinText();
        // Lấy component Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Kiểm tra xem quái vật có bị trúng đạn không
        if (isDead)
        {
            // Chuyển sang trạng thái death
            animator.SetTrigger("Death");
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
                // Chuyển sang trạng thái đuổi theo và chạy nhanh hơn
                animator.SetBool("isMoving", true);
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.Translate(directionToPlayer * chaseSpeed * Time.deltaTime);

                // Kiểm tra xem có đủ gần để tấn công không
                if (distanceToPlayer <= 1f) // Ví dụ khoảng cách tấn công là 1 đơn vị
                {
                    animator.SetTrigger("attack");
                }

                return;
            }
            else
            {
                // Chuyển sang trạng thái di chuyển bình thường
                animator.SetBool("isMoving", false);
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
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);

            // Đảo ngược hướng nhìn của nhân vật khi di chuyển sang trái
            if (scaleBoss.x > 0)
            {
                scaleBoss.x *= -1;
            }
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);

            // Đảo ngược hướng nhìn của nhân vật khi di chuyển sang phải
            if (scaleBoss.x < 0)
            {
                scaleBoss.x *= -1;
            }
        }

        transform.localScale = scaleBoss;
        animator.SetBool("isMoving", true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bulet")
        {
            // Tăng số lần trúng đạn
            hitCount++;

            if (hitCount >= maxHits)
            {
                // Chuyển sang trạng thái bị trúng đạn
                isDead = true;
                animator.SetTrigger("hit");

                BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>(); // Đạn bắn quái sẽ rơi xuống
                collider.isTrigger = true;

                // Sinh ra coin 
                DropCoin();
                // Hủy quái vật
                Destroy(gameObject, 2f);
            }
            else
            {
                // Chuyển sang trạng thái bị hit nhưng chưa chết
                animator.SetTrigger("hit");
            }
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
       
    }

    void UpdateCoinText()
    {
        // Cập nhật số coin lên TextMeshPro
        coinText.text = coinCount.ToString();
    }
}

