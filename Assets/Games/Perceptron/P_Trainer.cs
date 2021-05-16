using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Trainer : MonoBehaviour
{

    Texture2D plane;

    public Perceptron p;

    public int dotCount = 250;
    public float CalculateY_AI(float x) { return (-p.weights[2] - p.weights[0] * x) / p.weights[1]; }
    public float CalculateY(float x) { return 2 * x; }

    // Start is called before the first frame update
    void Start()
    {

        p = new Perceptron(2, Constants.Sign);

        plane = Resources.Load<Texture2D>("Plane-64");
        Sprite s = GetComponent<SpriteRenderer>().sprite;
        s = Sprite.Create(plane, new Rect(0, 0, 64, 64), Vector2.one * 0.5f);

        Train();                            
    }

    public void Train()
    {
        //create random dots
        Vector2[] dots = new Vector2[dotCount];
        for (int i = 0; i < dotCount; i++)
        {
            dots[i] = new Vector2(Random.Range(0,plane.width), Random.Range(0, plane.height));
        }

        ClearPlane(Color.white);

        for (int i = 0; i < 1000; i++)
        {
            foreach (Vector2 dot in dots)
                p.Train(new float[] { dot.x, dot.y }, new float[] { dot.y < CalculateY(dot.x) ? 1 : -1 });
        }

        foreach (Vector2 dot in dots)
        {
            float answer = p.Fire(new float[] { dot.x, dot.y }); /*   dot.y < CalculateY(dot.x) ? 1 : -1;*/
            plane.SetPixel((int)dot.x, (int)dot.y, answer == 1 ? Color.red : Color.cyan);
        }

        DrawLine(CalculateY, plane, Color.black);
        DrawLine(CalculateY_AI, plane, Color.green);
        plane.Apply();
    }

    public void ClearPlane(Color backgroundColor)
    {
        for (int x = 0; x < plane.width; x++)
        {
            for (int y = 0; y < plane.height; y++)
            {
                plane.SetPixel(x, y, backgroundColor);
            }
        }
    }
    public void DrawLine(System.Func<float, float> drawFunc, Texture2D drawPlane, Color lineColor)
    {
        for (int i = 0; i < 1000; i++)
        {
            if (drawFunc(i)<0)
            {
                continue;
            }
            if (drawFunc(i)>drawPlane.height)
            {
                break;
            }
            plane.SetPixel(i, (int)drawFunc(i), lineColor);
        }
    }
}
