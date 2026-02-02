// 게임 진행 상태 관리 (돈, 눈물, 스탯, 글루미 상태, 아이템 등)
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState{
        Intro1Required, //현글루미X + 인트로1 완료 기록 없음
        Intro2Required, //현글루미X + 인트로1 완료 기록 있음
        MainGame, //기존게임로드
        GloomyMeltRequired, //글루미 우울도100 > [녹는씬(연구원찾아오는것까지) > Intro2씬]
        GloomyFixRequired, //글루미 우울도0 > [폐기씬(연구원찾아오는것까지) > Intro2씬]
        GloomyleaveRequired, //글루미 친밀도100 > [떠남씬(연구원찾아오는것까지) > Intro2씬]
        GloomySleepRequired,//연구소 > 글루미 폐기요청 > 수면제 주사씬>fade-out
        GloomyExchangeRequired, //수면제주사씬 이후 (fade-in > )
        PlayerRetireRequired, //연구소 > 은퇴결정시
        NewMainGame //인트로1 이후 첫로드

    }
    public GameState currentState; //현재 게임상태
                                   //public GloomyData g = GloomyManager.instance.currentGloomy;

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject); //현재 게임오브젝트 자기자신 참조
        } else{
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //InitializeGameData();
        SetStartState();
        PlayerManager.instance.playerData.collectedTears = 531;

        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
    }
	void OnApplicationQuit()
	{
		if (GloomyManager.instance.currentGloomy != null){
            GloomyManager.instance.SaveCurrentGloomy();
        }
	}
	// Set Datas at First ======================================
	public void SetStartState()
    {
        //InitializeGameData();
        Debug.Log("Intro1완료여부: " + PlayerPrefs.GetInt("INTRO1_COMPLETED"));

        GloomyManager.instance.LoadGloomy();
        GloomyData gloomy = GloomyManager.instance.currentGloomy;

        PlayerManager.instance.LoadPlayerData();
        PlayerData player = PlayerManager.instance.playerData;

        if (gloomy == null) {

            if (PlayerPrefs.GetInt("INTRO1_COMPLETED") == 1) {
                Debug.Log("상태: Intro2필요");
                currentState = GameState.Intro2Required; //인트로매니저에서 글루미할당하기
            }
            else {
                Debug.Log("상태: Intro1필요");
                currentState = GameState.Intro1Required;
            }
        } 
        else {
            currentState = GameState.MainGame;
        }
    }
    // GameState Changed > 호출해서 상황에 맞는 씬으로 이동 ======================================
    public void ChangeGameState(GameState gState){
        currentState = gState;
        ChangeSceneOfCurrentState();
    }
    public void ChangeSceneOfCurrentState() //시작화면>대문 두드리면 씬 어디로?
    {
        switch (currentState)
        {
            case GameState.Intro1Required: //맨처음
                SceneChanger.instance.ChangeScene("Scene_INTRO1");
                break;
            case GameState.Intro2Required: //교환신청 > 수면제복용 > 시작화면(글루미삭제상태) 이후
                SceneChanger.instance.ChangeScene("Scene_INTRO2");
                break;
            case GameState.MainGame:
                SceneChanger.instance.ChangeScene("Scene_Main");
                break;
            case GameState.GloomyFixRequired: //글루미 우울도 0
                SceneChanger.instance.ChangeScene("Scene_FIX");
                break;
            case GameState.GloomyleaveRequired:
                SceneChanger.instance.ChangeScene("Scene_LEAVE");
                break;
            case GameState.GloomyMeltRequired: //글루미 우울도 100
                SceneChanger.instance.ChangeScene("Scene_MELT");
                break;
            case GameState.GloomySleepRequired: //연구소 > 교환신청
                SceneChanger.instance.ChangeScene("Scene_SLEEP");
                break;
            case GameState.GloomyExchangeRequired: //수면제복용이후
                DeleteGloomyData(true);
                SceneChanger.instance.ChangeScene("Scene_Start");
                break;
            case GameState.PlayerRetireRequired: //은퇴신청이후(게임전체초기화)
                InitializeGameData();
                SceneChanger.instance.ChangeScene("Scene_Start");
                break;
            default:
                InitializeGameData();
                SceneChanger.instance.ChangeScene("Scene_INTRO1");
                break;
        }
    }
    
    public void InitializeGameData()
    {
        PlayerPrefs.DeleteAll();
        //delete playerData
        SaveSystem.DeleteFile("PLAYER_DATA");
        //delete gData
        if (GloomyManager.instance.currentGloomy != null){
            DeleteGloomyData(true);
        }
        //delete invData
        InventoryUtils.ResetInventoryData();
    }
    public void DeleteGloomyData(bool isPermanentRemove)
    {
        GloomyManager.instance.RemoveCurrentGloomy(isPermanentRemove);
    }
    
}
// ====inventory reset setting =============================
public static class InventoryUtils
{
    public static void ResetInventoryData()
    {
        InventoryData emptyData = new InventoryData(); // 빈 데이터 생성
        SaveSystem.SaveToFile("INVENTORY_DATA", emptyData); // 저장
        Debug.Log("인벤토리 데이터 초기화 완료");
    }
}