using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Player : MonoBehaviour
{
    public float jumpForce;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce);
    }
}
