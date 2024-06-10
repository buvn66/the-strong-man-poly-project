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
    private bool isAttacking = false; // Trạng thái tấn công của quái vật

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
            return; // Quái vật đã chết, không thực hiện thêm hành động nào
        }

        // Kiểm tra xem nhân vật có tồn tại không
        if (player != null)
        {
            // Tính toán khoảng cách giữa quái và nhân vật
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Kiểm tra xem nhân vật có nằm trong khoảng cách để bắt đầu đuổi không
            if (distanceToPlayer <= followDistance)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.Translate(directionToPlayer * chaseSpeed * Time.deltaTime);

                // Kiểm tra xem có đủ gần để tấn công không
                if (distanceToPlayer <= 1f && !isAttacking) // Ví dụ khoảng cách tấn công là 1 đơn vị
                {
                    StartCoroutine(AttackPlayer());
                }

                return;
            }
        }

        // Di chuyển con quái giữa hai điểm start và end
        MoveBetweenPoints();
    }

    // Coroutine để tấn công nhân vật với delay
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("attack");
        // Thực hiện tấn công nhân vật ở đây nếu cần
        yield return new WaitForSeconds(2f); // Delay giữa các lần tấn công
        isAttacking = false;
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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bulet")
        {
            // Tăng số lần trúng đạn
            hitCount++;
            Debug.Log("Hit count: " + hitCount);

            if (hitCount >= maxHits)
            {
                // Chuyển sang trạng thái bị trúng đạn và chết
                isDead = true;
                animator.SetTrigger("death");
                Debug.Log("Quái vật đã chết");

                // Sinh ra coin 
                DropCoin();

                // Bắt đầu coroutine để ẩn quái vật sau khi animation chết kết thúc
                StartCoroutine(AfterDeath());
            }
            else
            {
                // Chuyển sang trạng thái bị hit nhưng chưa chết
                animator.SetTrigger("hit");
            }
        }
    }
    //IEnumerator:một phương pháp cho phép bạn tạm dừng thực thi một hàm tại một điểm cụ thể và sau đó tiếp tục lại từ điểm đó trong lần cập nhật tiếp theo hoặc sau một khoảng thời gian nhất định.
    IEnumerator AfterDeath()
    {
        // Đợi 3 giây trước khi ẩn quái vật
        yield return new WaitForSeconds(1f);

        // Ẩn quái vật
        gameObject.SetActive(false);
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
