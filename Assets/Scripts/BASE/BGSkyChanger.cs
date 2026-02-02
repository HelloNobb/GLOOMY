using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGSkyChanger : MonoBehaviour
{
    private SpriteRenderer sr;
    public GameObject clouds;
    public GameObject stars;
    public Sprite dayImg;
    public Sprite nightImg;
    public Sprite dawnImg;
    private int lastHour = -1;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
        clouds.SetActive(false);
        stars.SetActive(false);
	}
	void Update()
	{
		int currentHour = DateTime.Now.Hour;
        if (currentHour != lastHour){
            SetSkyByTime();
            lastHour = currentHour;
        }
	}

	public void SetSkyByTime(){
        // 시작화면이면 무조건 밤으로 =================
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Scene_Start"){
            sr.sprite = nightImg;
            stars.SetActive(true);
            clouds.SetActive(false);
            return;
        }
        // 시간대 따라 낮/밤/새벽 =================
        int hour = DateTime.Now.Hour;
        
        if (hour >= 7 && hour < 17) {//오전7 ~ 오후5 : 낮
            sr.sprite = dayImg;
            stars.SetActive(false);
            clouds.SetActive(true);
        } else if ((hour >= 17 && hour < 19) || (hour>=5 && hour <7)){ //오후5~오후7/오전5~7 : 과도기
            sr.sprite = dawnImg;
            stars.SetActive(false);
            clouds.SetActive(true);
        } else if (hour >= 19 || hour < 5) {//오후7~오전5 : 밤
            sr.sprite = nightImg;
            clouds.SetActive(false);
            stars.SetActive(true);
        } else{
            Debug.Log("알수없는 시간대");
            sr.sprite = dawnImg;
            stars.SetActive(false);
            clouds.SetActive(true);
        }
    }
}
