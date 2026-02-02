using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class BaseUIManager : MonoBehaviour //베이스 UI <-> 실제 데이터값 불러와 연결 및 업데이트
{
    public static BaseUIManager instance;

    public GameObject baseUIPanel;
    //gloomy info
    public TMP_Text gloomyID;
    public TMP_Text dayCount;
    //player info
    public TMP_Text playerName;
    public TMP_Text hiredDate;
    public TMP_Text accumTear; //누적 모은 눈물수 = 실적
    public TMP_Text playerPosition;
    //모은 돈
    public TMP_Text coinValue;
    public TMP_Text tearValue;
    //글루미
    public TMP_Text blueValue;
    public TMP_Text intiValue;
    public Slider blueSlider;
    public Slider intiSlider;
    public int dayPassed;
    //씬버튼들
    public GameObject sceneBttns;

    void Awake()
    {
        if (instance == null){
            instance = this;
        }   
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetUIStats();
    }

    public void SetUIStats() //최초 1회만 호출, 각각 업데이트되는건 함수로 호출
    {
        if (GloomyManager.instance.currentGloomy == null){
            Debug.Log("글루미 객체없음: UI업데이트 스킵");
            return;
        }

        GloomyData G = GloomyManager.instance.currentGloomy;
        dayPassed = (System.DateTime.Now - G.metDate).Days + 1;

        gloomyID.text = "GLOOMY #"+ G.gloomyID.ToString();
        dayCount.text = "DAY "+ dayPassed.ToString();

        blueValue.text = G.blue.ToString("F2")+"%";
        blueSlider.value = G.blue;

        intiValue.text = G.intimacy.ToString("F2")+"%";
        intiSlider.value = G.intimacy;
        
        //플레이어 UI 초기화
        UpdatePlayerUIs();
    }
    //플레이어 이름, 캐시, 눈물 Update
    public void UpdatePlayerNameDisplay(string name){
        playerName.text = name;
    }

    public void UpdateCashDisplay(int cash){
        coinValue.text = cash.ToString();
    }
    public void UpdateTearsDisplay(int tears, int accumTears) {
        tearValue.text = tears.ToString();
        accumTear.text = accumTears.ToString();
        //tearValue.text = accumTears.ToString();
    }
    public void UpdatePlayerLvDisplay(string position) {
        playerPosition.text = position;
    }
    public void UpdatePlayerUIs(){
        PlayerData p = PlayerManager.instance?.playerData;
        if (p == null) {
            Debug.Log("플레이어 데이터 없음 >> UI update failed");
            return;
        }
        playerName.text = p.playerName;
        accumTear.text = p.accumCollectedTears.ToString();
        hiredDate.text = p.hiredDate;
        PlayerManager.instance.SetPlayerLevel();

        coinValue.text = p.collectedCash.ToString();
        tearValue.text = p.collectedTears.ToString();//고용날짜 초기화 코드
    }
    //글루미 스탯 Update
    public void UpdateGloomyBlue(float newBlue){
        blueValue.text = newBlue.ToString("F2")+"%";
    }
    public void UpdateGloomyInti(float newInti){
        intiValue.text = newInti.ToString("F2")+"%";
    }
    public void SetActiveSceneBttns(bool set){
        sceneBttns.SetActive(set);
    }
    public void SetActiveBaseUI(bool set){
        baseUIPanel.SetActive(set);
    }
}
