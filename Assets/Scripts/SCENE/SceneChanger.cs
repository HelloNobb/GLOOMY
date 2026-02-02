//button객체의 onclick() 속성에 이 스크립트 넣고 불러올 씬 이름만 입력하기

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; //씬 관리 위한 네임스페이스

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    public GameObject loadingUI;
    public TMP_Text loadingTxt;
    public Coroutine change;

    private bool isSceneChanging = false;

    public void Awake()
    {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (isSceneChanging) return;

        if (!string.IsNullOrEmpty(sceneName)){
            if (change != null) StopCoroutine(change);
            change = StartCoroutine(LoadSceneAndHandleUI(sceneName));
            
        }
        else{
            Debug.LogWarning("씬 이름이 설정안돼서 전환안됨");
        }
    }

    private IEnumerator LoadSceneAndHandleUI(string sceneName) 
    {
        isSceneChanging = true;
        
        loadingTxt.text = GetLoadingMessage(sceneName);
        loadingUI.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        //로딩 끝날때까지 대기(x)->progress가 0.9이상 될때까지 대기
        while (asyncLoad.progress < 0.9f){
            //진행 상태 보여주는 기능넣기 (프레임마다반복됨)
            yield return null;
        }
        //로딩창 켠 상태로 n초 대기
        if (sceneName == "Scene_Main" || sceneName == "Scene_Garden" || sceneName == "Scene_Lab")
        {
            yield return new WaitForSeconds(1f);
        }
        else if (sceneName == "Scene_Start") {
            Debug.Log("시작화면으로 돌아가는중...");
            yield return new WaitForSeconds(2f);
            
        }
        else{
            yield return new WaitForSeconds(1f);
        }
        //씬 진입 허용
        asyncLoad.allowSceneActivation = true;
        //씬로드후
        yield return new WaitForSeconds(0.5f);
        //BaseUIManager.instance.SetUIStats();
        loadingUI.SetActive(false);
        Debug.Log("씬전환 및 BaseUI처리 완료됨: "+sceneName);
        isSceneChanging = false;
    }

    private Dictionary<string, string> sceneMessages = new Dictionary<string, string>{

        {"Scene_Main", "Loading..."},
        {"Scene_Lab", "연구소에서 신분 확인하는 중..."},
        {"Scene_Garden", "정원 들어가는 중..."},

        {"Scene_Bath", "글루미에게 목욕하자고 설득중..."},
        {"Scene_Kitchen1", "부엌으로 이동중..."},
        {"Scene_Kitchen2", "재료 준비중..."},
        {"Scene_Ttime", "글루미 기다리는 중..."},
        {"Scene_Walk", "글루미 외출옷으로 갈아입는 중..."},

        {"Scene_INTRO1", "연구원 기다리는 중..."},
        {"Scene_INTRO2", "새로운 글루미 배정을 기다리는 중..."},
        {"Scene_Start", "Loading..."}
    };

    private string GetLoadingMessage(string sceneName) 
    {
        if (sceneMessages.TryGetValue(sceneName, out string mssg)) {
            return mssg;
        }
        return "로딩중...";
    }
}
