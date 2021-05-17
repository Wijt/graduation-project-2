    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PlatformSpawner : MonoBehaviour
{
    public GameObject platform;

    public int platformCount = 100;
    public float height;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlatforms();
    }

    public void SpawnPlatforms()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < platformCount; i++)
        {
            GameObject p = Instantiate(platform, this.transform);
            p.transform.localPosition = Vector3.right * Random.Range(-2f, +2f) + Vector3.up * i + Vector3.up * Mathf.Min(2f, (height - Random.Range(-1f, 1f)));
        }
    }
}
