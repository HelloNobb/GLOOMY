using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 스프라이트 전용 (uiX)
public class Blinker : MonoBehaviour
{
    private float speed = 2f;
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
