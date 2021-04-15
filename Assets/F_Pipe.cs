using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Pipe : MonoBehaviour
{
    public float gap = 1;
    public float speed = 1;

    public GameObject UpPipe;
    public GameObject DownPipe;

    // Start is called before the first frame update
    void Awake()
    {
        UpPipe.transform.localPosition = new Vector3(0, gap/2, 0);
        DownPipe.transform.localPosition = new Vector3(0, -gap/2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
    }
}
