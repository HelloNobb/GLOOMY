using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
    // 여기서
    : greet문구하나만 가져오기, 선택한문구 타이핑
    - option > next 종류
     : medi_bought, medi_cancel, tear_01, g_01,lab_01,chat

    *** command, next 둘다 있을때 command 먼저 실행 후 next id node 실행
*/
public class LabChatController : MonoBehaviour
{
    public static LabChatController instance;
    [Header("Dialogue data")]
    public TextAsset jsonFile_Greet;
    public TextAsset jsonFile_Lab;
	public TextAsset[] jsonFile_Chats; //5개받기 0,1,2,3,4
    [Header("Chat UIs")]
    public GameObject chatPanel;
    public TMP_Text chatTxt;
    public TMP_Text chatSpeaker;
    [Header("Ops")]
    public GameObject optionsPanel;
    public Transform optionContainer;
    public GameObject optionPrefab;
    public DialogueLoader dLoader; //json file 설정

    //other parameters
    private Dictionary<string, DialogueNode> nodeMap;
    private DialogueNode currentNode;
    
    public Coroutine typeRoutine;////이거하고있었음

    private int textIndex = 0;
    private bool isTyping = false;
    private float typingSpeed = 0.3f;
    private const string R_NAME = "시스";

	void Awake(){
		if (instance == null) instance = this;
        else Destroy(gameObject);
	}
	void Start() {
		chatPanel.SetActive(false);
        optionsPanel.SetActive(false);
	}
    // 현재 노드 -> 이름, 대사, 스프라이트 설정후 display ===================================
    public void SetSpeaker(){ 
        //display Speaker (speaker name & face)
        if (currentNode.speaker!= null){
            chatSpeaker.text = currentNode.speaker;
        } else{
            if (PlayerManager.instance == null){
                chatSpeaker.text = "???";
            } else{
                if (PlayerManager.instance.GetPlayerLv() == PlayerManager.PlayerLv.lv1){
                    chatSpeaker.text = "연구원";
                } else{
                    chatSpeaker.text = R_NAME;
                }
            }
        }
    }
    // 현재 레벨 -> 인삿말 display ===================================
    public void DisplayGreeting(){ //labManager에서 호출s
		string nodeId = GetGreetingNodeId();

		dLoader.LoadDialogue(jsonFile_Greet); //일단셋팅
        currentNode = dLoader.GetNodeById(nodeId);
        string line = dLoader.GetRandomLine(currentNode);
        //type greeting message
        SetSpeaker();
		chatPanel.SetActive(true);
        optionsPanel.SetActive(false);
        if (typeRoutine != null) {
            StopCoroutine(typeRoutine);
            typeRoutine = null;
        }
        typeRoutine = StartCoroutine(TypeDialogue(line));
        //greeting 끝나면 dLoader file 변경
        dLoader.LoadDialogue(jsonFile_Lab);
    }
    // node id -> 대사 display ==================================
    public void DisplayDialogue(string id) { //node단위
        //dLoader.LoadDialogue(jsonFile_Lab); (greeting함수에 넣어둠)
		nodeMap = dLoader.GetNodesDict();
        if (!nodeMap.ContainsKey(id)){
            Debug.Log("대사id ["+id+"]를 찾을 수 없음!");
            return;
        }
        currentNode = nodeMap[id];
        //UI set
        textIndex = 0; //chatBox clicked -> ++
        chatPanel.SetActive(true);
        optionsPanel.SetActive(false);
        //////
        SetSpeaker();
        //display Dialogue
        if (typeRoutine != null) {
            StopCoroutine(typeRoutine);
            typeRoutine = null;
        }
        typeRoutine = StartCoroutine(TypeDialogue(currentNode.text[textIndex]));
    }
	public void DisplayChat(){
		dLoader.LoadDialogue(GetChatFile());
		currentNode = dLoader.GetRandomNode();
		//UI set
        textIndex = 0; //chatBox clicked -> ++
        chatPanel.SetActive(true);
        optionsPanel.SetActive(false);
		SetSpeaker();
        //display Dialogue
        if (typeRoutine != null) {
            StopCoroutine(typeRoutine);
            typeRoutine = null;
        }
        typeRoutine = StartCoroutine(TypeDialogue(currentNode.text[textIndex]));
	}
	IEnumerator TypeDialogue(string line) {
        isTyping = true;
        chatTxt.text = "";
        RSpriteController.instance?.SetRSprite(currentNode.face);

        BaseUIManager.instance?.SetActiveSceneBttns(false);
        LabManager.instance.UnActiveAllPanels();

        PlayerData player = PlayerManager.instance?.playerData;
        if (player != null){
            line = line.Replace("{playerName}", PlayerManager.instance.GetPlayerName());
            line = line.Replace("{playerPosition}",PlayerManager.instance.GetPlayerPosition());
            line = line.Replace("{goldPerTears}", PlayerManager.instance.GetPlayerGoldRate().ToString());
        } else{
            line = line.Replace("{playerName}", "관리인");
            line = line.Replace("{playerPosition}","외부 관리인");
            line = line.Replace("{goldPerTears}", "1");
        }
		MedicineData medi = LabManager.instance?.GetCurrentMedi();
		if (medi != null){
			line = line.Replace("{mediPrice}", medi.price.ToString());
		} else{
			line = line.Replace("{mediPrice}", "1000");
		}
        foreach (char c in line) {
            chatTxt.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        yield return new WaitForSeconds(0.5f);
        //타이핑 끝난 후 _ if (node의 마지막문장)이면
        isTyping = false;
        //이 줄이 text의 마지막 줄 && 옵션 있으면 -> 옵션 표시 (단, greeting노드가 아닐때만)
        if (!currentNode.id.StartsWith("greet_") && textIndex == currentNode.text.Length-1){
            if (currentNode.options != null && currentNode.options.Length  > 0) {
                DisplayOptions(currentNode.id);
            } 
        }
    }
    
    //OPTION Dislay ========================================
    public void DisplayOptions(string id){
        DialogueNode node = nodeMap[id];
        // empty
        foreach (Transform child in optionContainer){
            Destroy(child.gameObject);
        }
        optionsPanel.SetActive(true);
        //options dialogue
        for (int i = 0; i < node.options.Length; i++){
            DialogueOption opt = node.options[i];
            string nextId = opt.next;
            //instantiate opt
            GameObject optObj = Instantiate(optionPrefab, optionContainer);
            TMP_Text optTxt = optObj.GetComponentInChildren<TMP_Text>();
            optTxt.text = opt.text;
            //OptionBttn Click Event
            optObj.GetComponent<Button>().onClick.AddListener(()=>{
                optionsPanel.SetActive(false);
                chatPanel.SetActive(false);

                if(nextId == "medi_bought"){ //약 구매할 골드 있는지에 따라 진행
                    var medi = LabManager.instance.currentMedicine;
                    int mPrice = medi.price;
                    int playerGold = PlayerManager.instance.GetPlayerOwnedGold();

                    if (mPrice > playerGold && mPrice > 0){
                        DisplayDialogue("medi_goldLack");
                    } else {
                        DisplayDialogue("medi_bought");
                    }
                } else if (nextId == null){
                    Debug.Log("json오류: 해당 버튼의 다음 이벤트 할당안됨!");
                } else {
                    DisplayDialogue(nextId);
                }
            });
        }
    }

    // ChatBox Clicked => 다음 대사 있으면 로드 / 없으면 끄거나 command실행 (command,next 같이 있을수없음)
    public void OnChatBoxClicked(){
        if (isTyping || optionsPanel.activeSelf) return; 
        
        textIndex++;

        //greeting dialogue => chat끝내기
        if (currentNode.id.StartsWith("greet_")){
            chatPanel.SetActive(false);
            StopAllCoroutines();
            RSpriteController.instance?.SetRSprite("Default");
            //LabManager.instance.SetResearcherSprite("neutral");
            return;
        }
        //일반 dialogue
        if (currentNode != null && textIndex < currentNode.text.Length){ //if still chat left
            StopAllCoroutines();
            StartCoroutine(TypeDialogue(currentNode.text[textIndex]));
        } 
        else if (currentNode.command != null && currentNode.command.Length>0){ //if command exists
            LabManager.instance.ExecuteCommand(currentNode.command);
        } 
        else if (!string.IsNullOrEmpty(currentNode.next)){ //if next node exists (command X)
            DisplayDialogue(currentNode.next);
        } 
        else { ////opt X, command X, next X
            if (!LabManager.instance.IsAnyOtherPanelActive()){
                    BaseUIManager.instance?.SetActiveSceneBttns(true);
            }
            chatPanel.SetActive(false);
            StopAllCoroutines();
            RSpriteController.instance?.SetRSprite("Default");
            //LabManager.instance.SetResearcherSprite("neutral");
        }
    }
	public string GetGreetingNodeId(){ //player lv-> 인삿말노드 id return
		string nodeId;
		if (PlayerManager.instance != null){
			PlayerManager.PlayerLv pLv = PlayerManager.instance.GetPlayerLv();
			switch(pLv){
				case PlayerManager.PlayerLv.lv1:
					nodeId = "greet_01";
					break;
				case PlayerManager.PlayerLv.lv2:
					nodeId = "greet_02";
					break;
				case PlayerManager.PlayerLv.lv3:
					nodeId = "greet_02";
					break;
				case PlayerManager.PlayerLv.lv4:
					nodeId = "greet_02";
					break;
				case PlayerManager.PlayerLv.lv5:
					nodeId = "greet_02";
					break;
				default:
					nodeId = "greet_00";
					break;
			}
		} else{
			nodeId = "greet_00";
		}
		return nodeId;
	}
	public TextAsset GetChatFile(){ //player lv-> 인삿말노드 id return

		if (PlayerManager.instance != null){
			PlayerManager.PlayerLv pLv = PlayerManager.instance.GetPlayerLv();
			switch(pLv){
				case PlayerManager.PlayerLv.lv1:
					return jsonFile_Chats[0];
				case PlayerManager.PlayerLv.lv2:
					return jsonFile_Chats[1];
				case PlayerManager.PlayerLv.lv3:
					return jsonFile_Chats[2];
				case PlayerManager.PlayerLv.lv4:
					return jsonFile_Chats[3];
				case PlayerManager.PlayerLv.lv5:
					return jsonFile_Chats[4];
				default:
					return jsonFile_Chats[0];
			}
		} else{
			return jsonFile_Chats[0];
		}
	}
}