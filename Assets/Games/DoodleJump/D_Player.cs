using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class D_Player : SmartObject
{
    public float jumpForce = 10;
    Rigidbody2D rb;
    bool alreadyJump = false;

    //public Slider slider;

    public int point = 0;


    public float[] inputs;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;
        
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
        if (!isActive) return;

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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isActive = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        inputs = new float[15];
        inputs[0] = this.transform.position.x;
        inputs[1] = this.transform.position.y;
        inputs[2] = this.rb.velocity.y;

        RaycastHit2D hitUpCenter = Physics2D.Raycast(transform.position, Vector2.up, 10, LayerMask.GetMask("Default"));
        if (hitUpCenter)
        {
            Debug.DrawRay(transform.position, Vector2.up * Vector3.Distance(transform.position, hitUpCenter.point), Color.red, 0.2f);
            inputs[3] = Vector3.Distance(transform.position, hitUpCenter.point);
            inputs[4] = hitUpCenter.point.x;
            inputs[5] = hitUpCenter.point.y;
        }

        RaycastHit2D hitUpRight = Physics2D.Raycast(transform.position, new Vector2(1, 1), 10, LayerMask.GetMask("Default"));
        if (hitUpRight)
        {
            Debug.DrawRay(transform.position, new Vector2(1, 1) * Vector2.Distance(transform.position, hitUpRight.point), Color.red, 0.2f);
            inputs[6] = Vector3.Distance(transform.position, hitUpRight.point);
            inputs[7] = hitUpRight.point.x;
            inputs[8] = hitUpRight.point.y;
        }

        RaycastHit2D hitUpLeft = Physics2D.Raycast(transform.position, new Vector2(-1, 1), 10, LayerMask.GetMask("Default"));
        if (hitUpLeft)
        {
            Debug.DrawRay(transform.position, (new Vector2(-1, 1) * Vector3.Distance(transform.position, hitUpLeft.point)), Color.red, 0.2f);
            inputs[9] = Vector3.Distance(transform.position, hitUpLeft.point);
            inputs[10] = hitUpLeft.point.x;
            inputs[11] = hitUpLeft.point.y;
        }

        RaycastHit2D hitCenterDown = Physics2D.Raycast(transform.position, new Vector2(0, -1), 10, LayerMask.GetMask("Default"));
        if (hitCenterDown)
        {
            Debug.DrawRay(transform.position, (new Vector2(0, -1) * Vector3.Distance(transform.position, hitCenterDown.point)), Color.red, 0.2f);
            inputs[12] = Vector3.Distance(transform.position, hitCenterDown.point);
            inputs[13] = hitCenterDown.point.x;
            inputs[14] = hitCenterDown.point.y;
        }

        float output = brain.FeedForward(inputs)[0];
        output = Mathf.Min(output, 1);
        Vector3 newPos = Vector3.Lerp(transform.position, new Vector3(Utils.ReMap(output, 0, 1, -2.3f, 2.3f), transform.position.y, 0), Time.deltaTime * 10f);
        
        this.transform.position = newPos;
        
        if (Camera.main.transform.position.y < transform.position.y)
        {
            point += 1;
            fitness += 10;
        }
    }

}
