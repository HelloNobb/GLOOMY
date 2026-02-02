using System.Collections;
using UnityEngine;
using TMPro;
using System;


public class ChatManager : MonoBehaviour
{
    // 씬 입장시 - Greet 문구 출력(face설정 포함)
    // 챗 버튼 클릭 시 - chat 하기
    public static ChatManager instance;
    [Header("chat UIs")]
    public GameObject chatPanel;
    public TMP_Text chatTxt;
    public GameObject chatSign;

    public ChatLoader chatLoader;
    private ChatNode currentNode;
    private float typingSpeed = 0.03f;
    private bool isTyping = false;
    private int txtIndex = 0;
    private Coroutine type;

    void Awake(){
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
	void Start(){
        //check gloomyState
        if (GloomyManager.instance == null) {
            Debug.Log("G Data is null");
            return;
        }
		//인삿말 출력 (효과없음)
        DisplayGreetNode();
	}

    private void SetFace(string face){
        switch(face){
            case "Default":
                break;
            case "Happy":
                break;
            case "Mad":
                break;
            default:
                break;
        }
    }
    
    //로드할 파일 설정 ===============================
    private string GetFileName(){
        if (GloomyManager.instance.IsSick()){
            return "Gloomy_Event_Sick";
        } 
        else if (GloomyManager.instance.IsDirty()){
            return UnityEngine.Random.value < 0.5f ? "Gloomy_Event_Dirty" : "Gloomy_Daily";
        } 
        else {
            //random 30%확률로 우울도/친밀도/일상 대사 나옴
            float r = UnityEngine.Random.value; //0~1
            if (r > 0.66f){ //blue
                switch (GloomyManager.instance.blueState){
                    case GloomyManager.BlueState.BlueLow:
                        return "Gloomy_Blue01";
                    case GloomyManager.BlueState.BlueMidLow:
                        return "Gloomy_Blue02";
                    case GloomyManager.BlueState.BlueMidHigh:
                        return "Gloomy_Blue03";
                    case GloomyManager.BlueState.BlueHigh:
                        return "Gloomy_Blue04";
                    default:
                        Debug.Log("[chatM] 글루미 우울도 불러오기 실패");
                        return "Gloomy_Daily";
                }
            } else if (r > 0.33f){ //inti
                switch (GloomyManager.instance.intiState){
                    case GloomyManager.IntiState.IntiLow:
                        return "Gloomy_Inti01";
                    case GloomyManager.IntiState.IntiMidLow:
                        return "Gloomy_Inti02";
                    case GloomyManager.IntiState.IntiMidHigh:
                        return "Gloomy_Inti03";
                    case GloomyManager.IntiState.IntiHigh:
                        return "Gloomy_Inti04";
                    default:
                        Debug.Log("[chatM] 글루미 친밀도 불러오기 실패");
                        return "Gloomy_Daily";
                }
            } else{ //daily
                return "Gloomy_Daily";
            }
        }
    }
    private string GetGreetFileName(){
        string fName;
        DateTime lastChatTime = GloomyManager.instance.GetGLastChatTime();
        DateTime currentTime = DateTime.Now;
        TimeSpan timeElapsed = currentTime-lastChatTime;
        
        if (timeElapsed.TotalHours < 24){
            fName = "Gloomy_Hello_24h";
        } else if (timeElapsed.TotalHours < 24*3){
            fName = "Gloomy_Hello_3d";
        } else if (timeElapsed.TotalHours < 24*7){
            fName = "Gloomy_Hello_7d";
        } else {
            fName = "Gloomy_Hello_Old";
        }
        return fName;
    }
    //Display Node Txts & set face ===============================
    public void DisplayGreetNode(){
        if (type != null){
            StopCoroutine(type);
            type=null;
        }
        txtIndex = 0;
        TextAsset greetJsonFile = Resources.Load<TextAsset>($"JSON_Chat/{GetGreetFileName()}");
        if (greetJsonFile == null){
            Debug.Log("[ChatM] greet json file is null");
            return;
        }
        currentNode = chatLoader.GetRandomChatNode(greetJsonFile);

        type = StartCoroutine(TypeLine(currentNode.text[txtIndex]));
    }
    //display greet/chat node txt=========
    public void DisplayChatNode(){ //정확히는 노드 첫문장만 출력
        if (type != null){
            StopCoroutine(type);
            type=null;
        }
        txtIndex = 0;
        //node set
        TextAsset chatJsonFile = Resources.Load<TextAsset>($"JSON_Chat/{GetFileName()}");
        if (chatJsonFile == null) {
            Debug.Log("[chatM] jsonfile null");
            return;
        }
        currentNode = chatLoader.GetRandomChatNode(chatJsonFile);

        type = StartCoroutine(TypeLine(currentNode.text[txtIndex]));
	}
    //=========
    public void OnChatBttnClicked(){ //CareBttnsManager에서 호출
        if (GloomyManager.instance == null){
            Debug.Log("GM is null");
            return;
        }
        DisplayChatNode();
        Debug.Log("chatmanager>clicked호출됨!");
    }
    public void OnPanelClicked(){
        if (isTyping || currentNode == null) {
            Debug.Log("아직 대사 출력중");
            return;
        }
        txtIndex++;
        // 노드 대사 끝났는지, 안끝났는지에 따라
        if (txtIndex < currentNode.text.Length){
            StopCoroutine(type);
            type = StartCoroutine(TypeLine(currentNode.text[txtIndex]));
        } else {
            chatPanel.SetActive(false);
            SetGSprite("Default");
            GloomyManager.instance?.ChatGloomy();
        }
    }
    IEnumerator TypeLine(string line){
        isTyping = true;
        chatTxt.text = "";
        chatPanel.SetActive(true);
        //sprite(face) setting
        SetGSprite(currentNode.face);
        //line replace
        if (PlayerManager.instance != null){
            line = line.Replace("{playerName}", PlayerManager.instance.GetPlayerName());
        } else{
            line = line.Replace("{playerName}", "관리인");
        }
        //type
        foreach(char c in line){
            chatTxt.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(0.3f);
        isTyping = false;
    }

    public void SetGSprite(string face){
        if (GSpriteController.instance == null){
            Debug.Log("[chatM] gspriteManager is null");
            return;
        }

        switch(face){
            case "Default":
                GSpriteController.instance.Normal();
                break;
            case "Mad":
                GSpriteController.instance.Mad();
                break;
            case "Happy":
                GSpriteController.instance.Smile();
                break;
            case "Sad":
                GSpriteController.instance.Sad();
                break;
            default:
                GSpriteController.instance.Normal();
                break;
        }
    }

}
