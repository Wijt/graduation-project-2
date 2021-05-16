using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XOR_Plane : MonoBehaviour
{

    public EvolutionManager evolutionManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DrawPlane()
    {
        Texture2D plane = Resources.Load<Texture2D>("XOR-Plane-64");
        Sprite s = GetComponent<SpriteRenderer>().sprite;
        s = Sprite.Create(plane, new Rect(0, 0, 64, 64), Vector2.one * 0.5f);
        NeuralNetwork fittestBrain = evolutionManager.GetFittestObjectBrain();
        
        for (int x = 0; x < plane.width; x++)
        {
            for (int y = 0; y < plane.height; y++)
            {
                float output = fittestBrain.FeedForward(new float[] { x / plane.width, y / plane.height })[0];
                plane.SetPixel(x, y, new Color(1, 1, 1, output*255));
            }

        }
        plane.Apply();
    }
}
