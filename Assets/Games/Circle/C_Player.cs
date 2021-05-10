using SharpNeat.Phenomes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_Player : UnitController
{
    public float jumpForce = 0;
    public float speed = 1;

    Rigidbody2D rb;


    //NEAT STUFF
    IBlackBox brain;
    public bool isRunning = true;
    public int point = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;
        
        ISignalArray inputs = brain.InputSignalArray;
        inputs[0] = this.transform.position.y;
        inputs[1] = this.rb.velocity.y;

        Vector3 originTop = transform.position + new Vector3(0, 0.9f, 0);
        Vector3 originBottom = transform.position + new Vector3(0, -1.14f, 0);

        RaycastHit2D hitCenter = Physics2D.Raycast(originTop, Vector2.down, 10, LayerMask.GetMask("Default"));
        Debug.DrawRay(originTop, Vector3.down * Vector3.Distance(originTop, hitCenter.point), Color.red, 0.2f);
        inputs[2] = Vector3.Distance(originTop, hitCenter.point);

        RaycastHit2D hitBottom = Physics2D.Raycast(originBottom, new Vector2(1, 1), 10, LayerMask.GetMask("Default"));

        Debug.DrawRay(originBottom, new Vector2(1, 1) * Vector2.Distance(originBottom, hitBottom.point), Color.red, 0.2f);
        inputs[3] = Vector3.Distance(originBottom, hitBottom.point);


        RaycastHit2D hitTop = Physics2D.Raycast(originTop, new Vector2(1, -1), 10, LayerMask.GetMask("Default"));

        Debug.DrawRay(originTop, (new Vector2(1, -1) * Vector3.Distance(originTop, hitTop.point)), Color.red, 0.2f);
        inputs[4] = Vector3.Distance(originTop, hitTop.point);


        brain.Activate();

        ISignalArray outputs = brain.OutputSignalArray;
        if (outputs[0] > 0.5)
            Jump();
        rb.AddRelativeForce(Vector2.right * speed - rb.velocity);
        //transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce);

        point++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Road"))
        {
            Death();
        }
    }

    public void Death()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isRunning = false;
    }

    public override void Activate(IBlackBox box)
    {
        this.brain = box;
        isRunning = true;
    }

    public override void Stop()
    {
        isRunning = false;
    }

    public override float GetFitness()
    {
        return point;
    }
}
