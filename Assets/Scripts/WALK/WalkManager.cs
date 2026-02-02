using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkManager : MonoBehaviour
{
    [Header("UIs")]
    public Slider walkSlider;
    [Header("Objs")]
    public Transform bg;
    public Transform G;
    public GameObject sliderInfo;
    public Animator doorAnimator;
    public Animator GWalk;

    private float speed = 3f;
    private float maxOffset = 1f; //글루미 x: -1~1
    
    private float maxRight = 29.5f;
    private float maxLeft = -29.5f;
    private Coroutine resetCoroutine;
    private Coroutine doneCoroutine;

	void Start()
	{
		walkSlider.value = 0f;
        //sceneBttns unactive
        BaseUIManager.instance?.SetActiveBaseUI(false);
        sliderInfo.SetActive(true);
        GWalk = G.GetComponent<Animator>();
        GWalk.SetBool("isWalking", false);
	}
	void Update()
	{
        float direction = walkSlider.value; //-1~+1
        //0에 가까우면 무시
        if (Mathf.Abs(direction) < 0.01f) return; //Mathf.Abs:절대값함수
        
        //방향에 따라 G 모습 변경
        if (direction > 0){
            G.GetComponent<SpriteRenderer>().flipX = false;
            GWalk.SetBool("isWalking", true);
        } else if (direction < 0){
            G.GetComponent<SpriteRenderer>().flipX = true;
            GWalk.SetBool("isWalking", true);
        } else{ //direction = 0
            GWalk.SetBool("isWalking", false);
        }
        //움직일 양
        float moveAmount = direction * speed * Time.deltaTime;
        //캐릭터or배경 move
        if (Mathf.Abs(G.localPosition.x + moveAmount) <= maxOffset){
            G.localPosition += new Vector3(moveAmount,0f,0f);
        } else{
            float newX = bg.localPosition.x - moveAmount;

            if (newX > maxRight) newX = maxRight;
            if (newX < maxLeft) newX = maxLeft;

            bg.localPosition = new Vector3(newX,0f,0f);
        }
	}
    IEnumerator SmoothResetSlider(){
        float duration = 0.3f;
        float elapsed = 0f;
        float start = walkSlider.value;

        while (elapsed < duration){
            elapsed += Time.deltaTime;
            
            float t = elapsed / duration; //현재 경과 비율 계산 (0.0~1.0)
            walkSlider.value = Mathf.Lerp(start,0f,t); //0으로 스르륵 음직이기(ui)
            yield return null; //다음프레임까지 대기 (=>한프레임에한번씩호출해서 부드럽게)
        }
        walkSlider.value = 0f;
    }
    // events =========================================
    public void OnReleaseSlider(){
        if(resetCoroutine != null)
            StopCoroutine(resetCoroutine);
        
        resetCoroutine = StartCoroutine(SmoothResetSlider());
    }
    public void OnClickSlider(){
        sliderInfo.SetActive(false);
    }
    public void OnClickDoor(){
        if (doneCoroutine != null)
            StopCoroutine(doneCoroutine);
        
        doneCoroutine = StartCoroutine(OpenAndEnter());
    }
    IEnumerator OpenAndEnter(){
        doorAnimator.SetTrigger("OpenDoor");
        yield return new WaitForSeconds(2f);
        GloomyManager.instance?.WalkDoneGloomy();
        SceneChanger.instance?.ChangeScene("Scene_Main");
    }
    public void OnClickDone(){
        GloomyManager.instance?.WalkDoneGloomy();
        SceneChanger.instance?.ChangeScene("Scene_Main");
    }
}
