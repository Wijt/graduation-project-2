using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Player : MonoBehaviour
{
    public float jumpForce = 10;
    Rigidbody2D rb;
    bool alreadyJump = false;

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
    }
}
