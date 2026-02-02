using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/*
    약 별 아이디
    -tear / blue / happy / health / battery / sleep

    ** secret Panel (비밀유지각서) 부분 삭제처리해야함 (json,코드내)

    1) onclick  - 연구원, 약 5종 각각
    2) 씬 로드 - 문 [닫힘상태 > 열리기] -> 연구원 인삿말 로드
    3) 연구원: neutral상태일때 깜빡임 애니메이션 재생하기
*/

public class LabManager : MonoBehaviour
{
    //public GameObject infoPanels;
    /// //////////////
    public static LabManager instance;
    public static bool isLabChatting = false;
    
    [Header("문")]
    public GameObject labDoors;
    public Animator labDoorsAnimator;

    [Header("연구원")]
    public GameObject researcher;

    [Header("Medicine")]
    public GameObject[] infoPanels;
    public MedicineData[] medicineDatas;
    public MedicineData currentMedicine; //클릭한 약 오브젝트의 데이터

    [Header("Tear")]
    public GameObject tearMachineObj;
    public GameObject tearPanel;

    [Header("deco")]
    public GameObject blackPanel;
    //other values
    private float lastTappedTime = 0.0f;
    private float doubleTapTerm = 0.3f;
    private Coroutine openDoorCo;
    //눈물 환율
    private int goldPerTears=0;
    private bool isDoorOpening=false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        blackPanel.SetActive(false);
        foreach(GameObject info in infoPanels){
            info.SetActive(false);
        }
        currentMedicine = null;

        tearPanel.SetActive(false);
        tearMachineObj.SetActive(false);

        researcher.SetActive(true);
        labDoors.SetActive(true);
        

        //SetMedicineUI();

        if (PlayerManager.instance != null){
            goldPerTears = PlayerManager.instance.GetPlayerGoldRate();
        } else{
            Debug.Log("PlayerManager:null이라 골드환율0 처리됨!");
        }
        
		
    }
    // Intro Control ========================================
    public void OnDoorDoubleTapped()
    {
        float currentTime = Time.time;
        if (currentTime - lastTappedTime < doubleTapTerm)
        {
            Debug.Log("Door DoubleTapped!");
            openDoorCo = StartCoroutine(WaitAndOpenDoors());

        }
        lastTappedTime = currentTime;
    }
    IEnumerator WaitAndOpenDoors()
    {
        isDoorOpening = true;
        yield return new WaitForSeconds(1f);
        labDoorsAnimator.SetTrigger("OpenDoors");

        yield return new WaitForSeconds(1.5f);
        //door opening completed
        labDoors.SetActive(false);
        isDoorOpening = false;
        lastTappedTime = 0.0f;
        GreetPlayer(); //***
    }
    
    // Researcher Control ===================================
    public void GreetPlayer(){ //한문장 대사출력
        LabChatController.instance?.DisplayGreeting();
		Debug.Log("[LabM] greetplayer 호출됨!");
    }
    public void OnResearcherClicked() 
    {
        Debug.Log("Researcher Clicked!");
        //can't chat situation-> return
        if (isDoorOpening){
            Debug.Log("문 열리는중이라 클릭처리무시됨!");
            return;
        }
        if (LabChatController.instance.chatPanel.activeSelf){
            Debug.Log("chatPanel켜있어서 클릭처리무시됨!");
            return;
        }
        if (IsAnyOtherPanelActive()){
            Debug.Log("판넬중 하나라도 켜있어서 클릭처리무시됨!");
            return;
        }
        //Display Ask Dialogue
        LabChatController.instance.DisplayDialogue("researcher_clicked");
    }
    //Medicines Control ===========================
    public void OnMedicineClicked(string mediID){ //medi obj의 스크립트에서 호출
        if (IsAnyOtherPanelActive() || LabChatController.instance.chatPanel.activeSelf || isDoorOpening){
            Debug.Log("약버튼 클릭 무시됨(다른거활성화중)");
            return;
        }
        // 약 데이터 문제 없는지 확인 =========================
        currentMedicine = GetMedicineData(mediID);
        if (currentMedicine ==null){
            Debug.Log("약 id 잘못 입력됨!");
            return;
        }
        // 해당 medi info panel 활성화 ======================
        foreach (GameObject m in infoPanels){
            m.SetActive(false);
            string infoId = m.GetComponent<MediInfoClickHandler>().GetInfoId();
            if (infoId == mediID){
                m.GetComponent<MediInfoClickHandler>().SetInfoActive();
            }
        }
        // double tapped => 구매 대사 display==================
        float currentTime = Time.time;
        if (currentTime - lastTappedTime < doubleTapTerm){
            if (LabChatController.instance.chatPanel.activeSelf){
                Debug.Log("이미 대화중이라 더블탭 무시됨");
                return;
            }
            LabChatController.instance.DisplayDialogue($"{mediID}");
        }
        lastTappedTime = currentTime;
    }
    public MedicineData GetMedicineData(string mediID){
        foreach (var m in medicineDatas){
            if (m.id == mediID) return m;
        }
        Debug.Log("해당약 id가 목록에 없음");
        return null;
    }

    //Command Control =============================
    public void ExecuteCommand(string command)
    {
        //**** 실행 다 한 후, 현재노드의 next확인해서 null아니면 다음 노드 dialogue실행하기
        var parts = command.Split(':');
        switch(parts[0]){
            case "showPanel": 
                ShowPanel(parts[1]); 
                Debug.Log("showPanel 명령입력됨!");
                break;
            case "gameSet": 
                SetGame(parts[1]);
                Debug.Log("gameSet 명령입력됨!");
                break;
            case "giveItem":
                GiveItem(parts[1]); 
                break;
            default:
                Debug.Log("알수없는command 요청됨");
                break;
        }
        
    }
    public void ShowPanel(string panelName)
    {
        UnActiveAllPanels();
        blackPanel.SetActive(true);
        LabChatController.instance.chatPanel.SetActive(false);
        switch (panelName)
        {
            case "tearPanel": tearPanel.SetActive(true); break;
            //case "medicinePanel": medicinePanel.SetActive(true); break;
            //case "secretPanel": secretPanel.SetActive(true); break;
            default: Debug.Log("설정 안된 판넬이 json에서 요구됨!"); break;
        }
    }
    public void SetGame(string gameSetting)
    {
        //exchangeGloomy, resetGame
        switch (gameSetting)
        {
            case "makeSleepGloomy":
                //GameManager.Instance.currentState = GameManager.GameState.GloomySleepRequired;
                //GameManager.Instance.ChangeSceneOfCurrentState();
                GameManager.Instance.ChangeGameState(GameManager.GameState.GloomySleepRequired);
                break;
            case "retire":
                GameManager.Instance.ChangeGameState(GameManager.GameState.PlayerRetireRequired);
                break;
			case "chat":
				LabChatController.instance?.DisplayChat();
				break;
            default:
                Debug.Log("알 수 없는 게임셋팅 요구됨!");
                break;
        }
    }
    public void GiveItem(string itemName)
    {
        //inventory setting, gold--;
        if (itemName == "medicine"){
            //currentMedicine에 해당하는 약 인벤토리에 추가***
            int price = currentMedicine.price;
            PlayerManager.instance.AddGold(-price);
        }
    }
    // Panels Settings ==========================
    public void TearSubmitted(int tearAmount, int goldAmount){
        if (PlayerManager.instance == null){
            Debug.Log("playerManager:null > 눈물제출처리못함");
            return;
        }

        PlayerManager.instance.AddGold(goldAmount);
        PlayerManager.instance.AddTears(-tearAmount);
        PlayerManager.instance.AddSubimitTears(tearAmount);
        //accumTearAmount 업데이트 > playerLv 업데이트 > goldPerTears업데이트
        PlayerManager.instance.SetPlayerLevel();
        BaseUIManager.instance.UpdatePlayerUIs();
    }
    public bool IsAnyOtherPanelActive(){
        GameObject[] panelsForCheck = {
            //medicinePanel: 상시 켜있어야함
            tearPanel
        };
        foreach (var panel in panelsForCheck){
            if (panel.activeSelf)
                return true;
        }
        return false;
    }
    public void UnActiveAllPanels(){
        GameObject[] panels = {
            tearPanel
        };
        foreach (var panel in panels){
            panel.SetActive(false);
        }
        foreach (GameObject m in infoPanels){
            m.SetActive(false);
        }
        blackPanel.SetActive(false);
    }

    // medi data
    public MedicineData GetCurrentMedi(){
        return currentMedicine;
    }
}