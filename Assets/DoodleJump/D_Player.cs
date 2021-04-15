using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_Player : MonoBehaviour
{
    public float jumpForce = 10;
    Rigidbody2D rb;
    bool alreadyJump = false;

    public Slider slider;


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

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(ReMap(slider.value, 0, 1, -2.3f, 2.3f), transform.position.y, 0);
    }

    public float ReMap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}