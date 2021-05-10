using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_PipeSpawner : MonoBehaviour
{
    public List<GameObject> pipes = new List<GameObject>();
    public GameObject pipe;
    public Vector3 spawnPos;
    private float spaceBetweenPipes = 3;

    // Start is called before the first frame update
    void Start()
    {
        spaceBetweenPipes = F_SettingController.setting.spaceBetweenPipes;
        SpawnMultiplePipe(3);
    }

    public void SpawnMultiplePipe(int pipeCount)
    {
        if (pipes.Count > 0) {
            foreach (GameObject p in pipes)
            {
                Destroy(p);
            }
            pipes.Clear();
        }
        for (int i = 0; i < pipeCount; i++)
            SpawnPipe();
    }

    public void SpawnPipe()
    {
        if (pipes.Count > 0){ 
            pipes.Add(Instantiate(pipe, new Vector3(pipes[pipes.Count - 1].transform.position.x + spaceBetweenPipes, Random.Range(-1f, 1f), spawnPos.z), Quaternion.identity, this.transform));
            return;
        }
        pipes.Add(Instantiate(pipe, new Vector3(spawnPos.x, Random.Range(-2f, 2f), spawnPos.z), Quaternion.identity, this.transform));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name.Contains("Pipe") && collision.gameObject.GetComponent<F_Pipe>() != null)
        {
            SpawnPipe();
            pipes.Remove(collision.gameObject);
            Destroy(collision.gameObject, 1);
        }
    }
}
