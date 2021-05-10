using SharpNeat.Phenomes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class F_Player : UnitController
{
    private float jumpForce;
    Rigidbody2D rb;

    //NEAT STUFF
    IBlackBox brain;
    public bool isRunning = true;
    public int point = 0;

    public F_PipeSpawner pipeSpawner;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        pipeSpawner = FindObjectOfType<F_PipeSpawner>();
        jumpForce = F_SettingController.setting.birdJumpForce;
    }

    private void Update()
    {
        if (!isRunning) return;

        ISignalArray inputs = brain.InputSignalArray;
        inputs[0] = this.transform.position.y;
        inputs[1] = this.rb.velocity.y;
        inputs[2] = 0;
        inputs[3] = 0;
        inputs[4] = 0;

        RaycastHit2D hitPipe = Physics2D.Raycast(transform.position, Vector2.right, 100, LayerMask.GetMask("Default"));
        Debug.DrawRay(transform.position, Vector3.right * Vector3.Distance(transform.position, hitPipe.point), Color.red, 0.2f);
        
        if (hitPipe)
        {
            Transform pipe = hitPipe.collider.gameObject.transform;
            if (pipe != null)
            {
                inputs[2] = pipe.position.x - (1.04f / 2f);
                inputs[3] = pipe.position.y - F_SettingController.setting.gapBetweenPipe / 2;
                inputs[4] = pipe.position.y + F_SettingController.setting.gapBetweenPipe / 2;
            }
        }

        

        brain.Activate();

        ISignalArray outputs = brain.OutputSignalArray;

        if (outputs[0] >= 0.5f)
        {
            Jump();
        }

        transform.rotation = Quaternion.Euler(Vector3.forward * ReMap(rb.velocity.y, +8, -8, 75, -90));
    }
    public void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponents<F_Pipe>() != null && isRunning)
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);       
        isRunning = false;
    }

    public float ReMap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
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
