using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemySlime_Controller : MonoBehaviour
{
    [SerializeField] float start, end, speed;
    Rigidbody2D rig;
    int isRight = 1;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody2D>();
    }
    void Run()
    {
        var x_enemy = transform.position.x;
        if(x_enemy < start)
        {
            isRight = 1;
        }
        if(x_enemy > end)
        {
            isRight = -1;
        }
        transform.Translate(new Vector3(isRight*speed*Time.deltaTime, 0, 0));

        var y_enemy = transform.position.y;
        var x_player = player.transform.position.x; 
        var y_player = player.transform.position.y;        
        if((x_player > start && x_player <end) && (y_player < y_enemy + 1 && y_player > y_enemy - 1))
        {
            if(x_player < x_enemy)
            {
                isRight = -1;
            }
            if(y_player > y_enemy)
            {
                isRight = 1;
            }
        }
    }
    void Flip()
    {
        transform.localScale = new Vector2(isRight, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        Run();  
        Flip();
    }
}
