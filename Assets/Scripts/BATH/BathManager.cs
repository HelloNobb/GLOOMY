//냉100 유지 10초->강종+감기 , 뜨100 유 10초->강종+친밀도(-1) ::추가해야함
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BathManager : MonoBehaviour
{
    public static BathManager instance;
    [Header("G")]
    public Sprite G_Cold;
    public Sprite G_Warm;
    public GameObject G;
    public Animator G_Anim;
    private AnimatorStateInfo gStateInfo;
    public GameObject heatObj;


    [Header("UI")]
    public Slider heatSlider;
    public TMP_Text heat;
    public GameObject cancelPanel;
    public GameObject chatBox;
    public GameObject heatHandle;
    public TMP_Text infoTxt;

    public HandleRotator handleRotator;
    private Coroutine goRoom;
    private Coroutine showUI;
    private float heatIncreaseRate = 0.00015f; //회전량당 증가량
    private float heatDecreaseRate = 0.1f; //시간당 감소량
    private float holdTimeRequired = 5f;

    private float heatValue = 0f; //현재 온수량(0~1)
    private float heatHoldTimer = 0f; //유지시간카운트
    private bool isBathDone = false;

    enum Temper{
        COLD, WARM
    }
    Temper GTemper;
	void Awake() {
		if (instance == null) instance = this;
        else Destroy(gameObject);
	}
	void Start() {
        G_Anim.enabled = false;
        heat.gameObject.SetActive(false);
        heatSlider.gameObject.SetActive(false);
        heatSlider.value = 0;
        heatHandle.SetActive(false);
        infoTxt.gameObject.SetActive(false);
        heatObj.SetActive(false);
        chatBox.SetActive(false);
        BaseUIManager.instance?.SetActiveBaseUI(false);
        
        if (showUI != null) StopCoroutine(showUI);
        showUI = StartCoroutine(ShowUIs());
    }
    IEnumerator ShowUIs(){
        yield return new WaitForSeconds(2f);
        G_Anim.enabled = true;
        yield return new WaitForSeconds(4f);
        heatSlider.gameObject.SetActive(true);
        heat.gameObject.SetActive(true);
        infoTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        heatHandle.SetActive(true);
    }

	void Update() {

        if (isBathDone) return;

        if (G_Anim.enabled)
            gStateInfo = G_Anim.GetCurrentAnimatorStateInfo(0);
        //핸들 회전 감지
        float input  = handleRotator.deltaAngle;

        if (input > 0f){
            heatValue += input * heatIncreaseRate;
        } else {
            heatValue -= heatDecreaseRate * Time.deltaTime;
        }

        heatValue = Mathf.Clamp01(heatValue);
        heatSlider.value = heatValue;
        heat.text = ((int)(heatValue*100f)).ToString()+"%";

        // 100% 유지 체크
        if (heatValue >= 0.9f){
            heatHoldTimer += Time.deltaTime;

            if (heatHoldTimer >= holdTimeRequired){
                OnBathDone();
                return;
            }
        } else{
            heatHoldTimer = 0f;
        }
        //animation trigger set
        if (!G_Anim.enabled || G_Anim.IsInTransition(0)) return;

        if (heatValue >= 1f){
            if (gStateInfo.IsName("Bath_G_Shiver")){
                G_Anim.SetTrigger("WarmG");
            }
            heatObj.SetActive(true);

        } else if (heatValue >= 0.7f){
            if (gStateInfo.IsName("Bath_G_Shiver")){
                G_Anim.SetTrigger("WarmG");
            }

        }else {
            if (!gStateInfo.IsName("Bath_G_Shiver")){
                G_Anim.SetTrigger("ColdG");
            }
            heatObj.SetActive(false);
        }
	}
    
    void OnBathDone(){
        isBathDone = true;
        Debug.Log("목욕완료");
        
        heatObj.SetActive(true);
        G_Anim.enabled = false;
        G.GetComponent<SpriteRenderer>().sprite = G_Warm;
        //효과적용하고 씬이동
        GloomyManager.instance.BathGloomy();

        if (goRoom != null) StopCoroutine(goRoom);
        goRoom = StartCoroutine(WaitAndGoRoom());
    }
    IEnumerator WaitAndGoRoom(){
        heatSlider.gameObject.SetActive(false);
        heatHandle.SetActive(false);
        infoTxt.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        chatBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneChanger.instance.ChangeScene("Scene_Main");
    }
    public void OnClickCancelBttn(){
        cancelPanel.SetActive(true);
    }
    public void OnClickCancelConfirmBttn(){
        GloomyManager.instance.CancelBathGloomy();
        SceneChanger.instance.ChangeScene("Scene_Main");
    }
    public void OnClickCancelCancelBttn(){
        cancelPanel.SetActive(false);
    }
}