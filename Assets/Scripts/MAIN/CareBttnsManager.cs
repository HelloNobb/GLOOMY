using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.AI;
using System;

public class CareBttnsManager : MonoBehaviour
{
    public static CareBttnsManager instance;
    public GameObject pillPanel;
    public GameObject walkPanel;
    public GameObject bathPanel;
    public GameObject black;
    //산책 불가
    public GameObject cantPanel;
    public TMP_Text cantReason;

	void Awake(){
		if (instance == null) instance = this;
        else Destroy(gameObject);
	}
	void Start()
    {
        pillPanel.SetActive(false);
        walkPanel.SetActive(false);
        bathPanel.SetActive(false);
        black.SetActive(false);
    }
    // Bath ====================================
    public void OnBathClick(){
        if (GloomyManager.instance == null){
            cantReason.text = "글루미가 부재합니다.";
            cantPanel.SetActive(true);
            return;
        }
        //if upset
        if (GloomyManager.instance.IsSick()){
            cantReason.text = "글루미가 아픕니다.";
            cantPanel.SetActive(true);
        } else {
            //위생상태에 따라
            if (GloomyManager.instance.GetGHygiene() >= 100){
                cantReason.text = "글루미는 이미 깨끗합니다.";
                cantPanel.SetActive(true);
            } else{
                bathPanel.SetActive(true);
            }
        }
    }
    public void OnBathConfirmClick(){
        SceneChanger.instance?.ChangeScene("Scene_Bath");
    }
    // Tea ====================================
    public void OnTeaClick(){
        if (GloomyManager.instance == null){
            cantReason.text = "글루미가 부재합니다.";
            cantPanel.SetActive(true);
            return;
        }
        if (SceneChanger.instance == null) return;
        
        SceneChanger.instance.ChangeScene("Scene_Kitchen1");
    }
    // Walk ===================================
    public void OnWalkClick(){ 
        if (GloomyManager.instance == null){
            cantReason.text = "글루미가 부재합니다.";
            cantPanel.SetActive(true);
            return;
        }
        if (GloomyManager.instance.currentGloomy.battery >= 1){
            walkPanel.SetActive(true);
        } else{
            cantReason.text = "배터리가 부족합니다.";
            cantPanel.SetActive(true);
        }
    }
    public void OnWalkConfirmClick(){
        GloomyManager.instance?.SetGBattery(-1);
        SceneChanger.instance?.ChangeScene("Scene_Walk");
    }
    // Shot ===================================
    public void OnShotClick(){
        if (GloomyManager.instance == null){
            cantReason.text = "글루미가 부재합니다.";
            cantPanel.SetActive(true);
            return;
        }
        pillPanel.SetActive(true);
    }
    public void OnChatClick(){
        //ChatManager.instance.OnStartChat();
        if (GloomyManager.instance == null){
            cantReason.text = "글루미가 부재합니다.";
            cantPanel.SetActive(true);
            return;
        }
        if (ChatManager.instance == null) {
            Debug.Log("ChatManager is absent!");
            return;
        }
        Debug.Log("chatManager호출?");
        ChatManager.instance.OnChatBttnClicked();
    }
    public void OnCloseParentPanel(Button cancelBttn) {
        GameObject parentPanel = cancelBttn.transform.parent.gameObject;
        parentPanel.SetActive(false); 
    }
}
