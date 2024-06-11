using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class quaman : MonoBehaviour
{
    public string manmoi;
    public void loading()
    {
        SceneManager.LoadScene(manmoi);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            loading();
        }

    }
}