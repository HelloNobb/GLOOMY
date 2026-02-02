// 현재 시간에 따라 배경이미지 변경 기능
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class MainBGChanger : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite dayBG;
    public Sprite nightBG;
    public Sprite dawnBG;
    private int lastHour=-1;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
        SetBGByTime();
	}
	void Update()
	{
		int currentHour = DateTime.Now.Hour;
        if (currentHour != lastHour){
            SetBGByTime();
            lastHour = currentHour;
        }
	}

	public void SetBGByTime(){
        int hour = DateTime.Now.Hour;
        
        if (hour >= 7 && hour < 17) {//오전7 ~ 오후5 : 낮
            sr.sprite = dayBG;
        } else if ((hour >= 17 && hour < 19) || (hour>=5 && hour <7)){ //오후5~오후7/오전5~7 : 과도기
            sr.sprite = dawnBG;
        } else if (hour >= 19 || hour < 5) {//오후7~오전5 : 밤
            sr.sprite = nightBG;
        } else{
            Debug.Log("알수없는 시간대");
            sr.sprite = dawnBG;
        } 
    }
}
