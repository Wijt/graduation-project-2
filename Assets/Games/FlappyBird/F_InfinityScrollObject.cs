using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_InfinityScrollObject : MonoBehaviour
{
    List<Transform> childs = new List<Transform>();
    float childWidth = 0;
    Vector2 targetPos;
    float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        speed = F_SettingController.setting.gameSpeed;
        foreach (Transform child in transform)
        {
            childs.Add(child);
        }
        childWidth = Mathf.Abs(childs[0].position.x - childs[1].position.x);
        targetPos = childs[0].position + Vector3.left * childWidth;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos.x > childs[0].position.x)
        {
            childs[0].position = childs[childs.Count - 1].position + Vector3.right * childWidth;
            Move(childs, 0, childs.Count - 1);
        }
        foreach (Transform child in transform)
        {
            child.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
    public void Move<T>(List<T> list, int oldIndex, int newIndex)
    {
        // exit if positions are equal or outside array
        if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= list.Count) || (0 > newIndex) ||
            (newIndex >= list.Count)) return;
        // local variables
        var i = 0;
        T tmp = list[oldIndex];
        // move element down and shift other elements up
        if (oldIndex < newIndex)
        {
            for (i = oldIndex; i < newIndex; i++)
            {
                list[i] = list[i + 1];
            }
        }
        // move element up and shift other elements down
        else
        {
            for (i = oldIndex; i > newIndex; i--)
            {
                list[i] = list[i - 1];
            }
        }
        // put element from position 1 to destination
        list[newIndex] = tmp;
    }
}
