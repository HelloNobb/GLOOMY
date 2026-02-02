using System;
using UnityEngine;

public class LightBlinker : MonoBehaviour
{
    private float speed = 8f;
    private SpriteRenderer sr;
    private float time;

    void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}
	void Update()
	{
		time += Time.deltaTime * speed;
        float alphaTmp = (MathF.Sin(time) + 1f) / 2f; //0~1사이 부드럽게 반복 (MathF.Sin(time): -1~+1)
        float alphaValue = Mathf.Lerp(0.3f, 1f, alphaTmp); //0.3~1사이 부드럽게 반복
        Color c = sr.color;
        c.a = alphaValue;
        sr.color = c;
	}
}
