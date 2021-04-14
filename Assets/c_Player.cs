﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class c_Player : MonoBehaviour
{
    public float jumpForce = 0;
    public float speed = 1;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddRelativeForce(Vector2.right * speed - rb.velocity);
        //transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce);
    }
}
