using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using random = UnityEngine.Random;
namespace Nave_Project
{
public class CharaMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 2.0f;
    Vector2 randomposition;
    Vector2 ReverseP;
    void Start()//初期位置設定
    {
        Camera cam = Camera.main;
        if (random.value < 0.25f)
        {
            randomposition = new Vector3(random.Range(-0.5f, -0.1f), random.Range(-0.5f, -0.1f), 0.5f);
        }
        else if (random.value < 0.5f)
        {
            randomposition = new Vector3(random.Range(-0.5f, -0.1f), random.Range(1.1f, 1.5f), 0.5f);
        }
        else if (random.value < 0.75f)
        {
            randomposition = new Vector3(random.Range(1.1f, 1.5f), random.Range(1.1f, 1.5f), 0.5f);
        }
        else
        {
            randomposition = new Vector3(random.Range(1.1f, 1.5f), random.Range(-0.5f, -0.1f), 0.5f);
        }
        transform.position = cam.ViewportToWorldPoint(randomposition);

        ReverseP = cam.ViewportToWorldPoint(new Vector3(1 - randomposition.x, 1 - randomposition.y, 0.5f));//中心に向かうベクトル計算
        Debug.Log(ReverseP);
        Debug.Log(randomposition);
        StartCoroutine("Moving");//移動開始
    }
    IEnumerator Moving()
    {
        while (true)
        {
            transform.Translate(ReverseP * speed * Time.deltaTime);
            yield return null;
        }
    }

}}
