using System;
using Unity.VisualScripting;
using UnityEngine;

public class shot_check : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject chara;
    Vector2 pos;
    Camera cam;
    void Start()
    {
        chara = GameObject.Find("Chara_Proto");
    }

    // Update is called once per frame
    public void Check()
    {
        pos = chara.transform.position;

        if (Math.Abs(pos.x) <= 0.1 && Math.Abs(pos.y) <= 0.1)
        {
            Debug.Log("Best Shot!");
            return;
        }
        else if (Math.Abs(pos.x) <= 1 && Math.Abs(pos.y) <= 1)
        {
            Debug.Log("Great Shot!");
            return;
        }
        else if (Math.Abs(pos.x) <= 2 && Math.Abs(pos.y) <= 2)
        {
            Debug.Log("Good Shot!");
            return;
        }
        else
        {
            Debug.Log("Bad Shot...");
            return;
        }
    }
}
