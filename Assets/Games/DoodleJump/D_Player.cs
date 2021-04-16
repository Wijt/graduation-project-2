using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class D_Player : MonoBehaviour
{
    public float jumpForce = 10;
    Rigidbody2D rb;
    bool alreadyJump = false;

    public Slider slider;

    public int point = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Platform"))
        {
            if (!alreadyJump)
            {
                alreadyJump = true;
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce);
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Platform"))
        {
            alreadyJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Death"))
        {
            Death();
        }
    }

    public void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, new Vector3(ReMap(slider.value, 0, 1, -2.3f, 2.3f), transform.position.y, 0), Time.deltaTime * 10f);
        this.transform.position = newPos;
        if (Camera.main.transform.position.y<transform.position.y)
        {
            point += 1;
        }
    }

    public float ReMap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
