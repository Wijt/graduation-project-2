using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class F_Player : MonoBehaviour
{
    public int point=0;
    private float jumpForce;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpForce = F_SettingController.setting.birdJumpForce;
    }

    public void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponents<F_Pipe>() != null)
        {
            point++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Pipe") || collision.gameObject.name.Contains("Death"))
        {
            Death();
        }
    }

    public void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);       
    }
}
