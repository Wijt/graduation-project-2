using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Camera : MonoBehaviour
{
    public Transform target;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - target.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < target.position.y)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, new Vector3(transform.position.x, target.position.y + offset.y, transform.position.z), Time.deltaTime * 1f);
            this.transform.position = newPos;

        }

    }
}
