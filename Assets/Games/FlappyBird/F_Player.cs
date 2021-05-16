using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class F_Player : SmartObject
{
    public int point=0;
    private float jumpForce;
    Rigidbody2D rb;

    public float[] debugArray;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpForce = F_SettingController.setting.birdJumpForce;
        isActive = true;
        fitness = 0;
        point = 0;
        debugArray = new float[6] { 0, 0, 0, 0, 0, 0 };
    }

    private void Update()
    {
        if (!isActive) return;
        
        float[] inputs = new float[6];

        inputs[0] = this.transform.position.y;
        inputs[1] = this.rb.velocity.y;
        inputs[2] = 0;
        inputs[3] = 0;
        inputs[4] = 0;
        inputs[5] = 0;
        RaycastHit2D hitPipe = Physics2D.Raycast(transform.position, Vector2.right, 100, LayerMask.GetMask("Default"));
        Debug.DrawRay(transform.position, Vector3.right * Vector3.Distance(transform.position, hitPipe.point), Color.red, 0.2f);

        if (hitPipe)
        {
            Transform pipe = hitPipe.collider.gameObject.transform;
            if (pipe != null)
            {
                inputs[2] = pipe.position.x - (1.04f / 2);
                inputs[3] = pipe.position.y - F_SettingController.setting.gapBetweenPipe / 2;
                inputs[4] = pipe.position.y + F_SettingController.setting.gapBetweenPipe / 2;
                inputs[5] = Vector2.Distance(this.transform.position, hitPipe.point);
            }
        }

        debugArray = inputs;

        float[] outputs = brain.FeedForward(inputs);

        if (outputs[0] >= 0.5f)
        {
            Jump();
        }

        fitness += 10;

        transform.rotation = Quaternion.Euler(Vector3.forward * Utils.ReMap(rb.velocity.y, +8, -8, 75, -90));
    }
    public void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;
        if (collision.GetComponents<F_Pipe>() != null)
        {
            point++;
            fitness += 1000;
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isActive = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }    
}
