using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //máu của Boss 
    private float _health = 100;

    [SerializeField]
    private Slider _healthSlider;
  
    private void Start()
    {
        _healthSlider.value = _health;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            //hủy viên đạn 
            Destroy(other.gameObject);
            //mổi lần trung bullet -10 máu của boss  
            _health -= 1;
            _healthSlider.value = _health;
            if (_health <= 0)
            {
               

                Destroy(gameObject);
            }
        }
    }
}
