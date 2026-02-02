using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// 메인룸 ui, Gsprite 셋팅만 관여
public class MainGManager : MonoBehaviour
{
    public static MainGManager instance;

	void Awake(){
		if (instance == null) instance = this;
        else Destroy(gameObject);
	}
	void Start()
    {
        BaseUIManager.instance.sceneBttns.SetActive(true);
        //load gState
        GloomyData G = GloomyManager.instance.currentGloomy;
        if (GloomyManager.instance.currentGloomy == null){
            Debug.LogError("글루미가 할당 안됨. 다시 시작화면으로 돌아감");
            SceneChanger.instance.ChangeScene("Scene_INTRO1");
        }
        GloomyManager.instance.CheckGStates();
        //load UI
        ApplyGState();
        if (BaseUIManager.instance != null){
            BaseUIManager.instance.SetActiveBaseUI(true);
            BaseUIManager.instance.SetActiveSceneBttns(true);
        }
        //글루미 상태에 따라 toast
        if (ToastUIManager.instance != null && GloomyManager.instance.gState != GloomyManager.GState.Normal){
            if (GloomyManager.instance.IsSick()){
                ToastUIManager.instance.ShowToast("글루미가 아픕니다.");
            } 
            if (GloomyManager.instance.IsDirty()){
                ToastUIManager.instance.ShowToast("글루미가 더럽습니다.");
            }
            if (GloomyManager.instance.IsUpset()){
                ToastUIManager.instance.ShowToast("글루미의 기분이 나쁩니다.");
            }
        }
        
    }
    
    public void ApplyGState(){
        //null exception
        if (GloomyManager.instance == null || GloomyManager.instance.currentGloomy == null){
            Debug.Log("[MM] 글루미정보 로드 실패");
            return;
        }
        GSpriteController.instance?.SetGSprite();

        //눈물 루틴 시작하고 리턴 ==============
        if (GloomyManager.instance.gState == GloomyManager.GState.Normal){
            //TearManager.instance.CryG();
        } else{ //Abnormal
            if (GloomyManager.instance.IsSick()){ //if sick
                //TearManager.instance.StopCryG();
                //care block
            } else{ // if not sick
                //아픔X + 더러움상태
                // if(GloomyManager.instance.IsDirty()){
                //     TearManager.instance.CryG();
                // } else { // 아픔X + 더러움X 상태 + Abnormal  ((추후 삐짐, 다른증상 넣기
                //     TearManager.instance.CryG();
                // }
            }
        }
    }
}
