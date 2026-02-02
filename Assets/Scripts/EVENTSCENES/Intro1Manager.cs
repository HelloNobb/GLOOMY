using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.Collections;
using UnityEditor;

[System.Serializable]
public class DialogueData{
    public List<string> lines;
}
public class Intro1Manager : MonoBehaviour
{
    [Header("연구원 대화 및 유저이름받기")]
    public GameObject researcher;
    public GameObject chatPanel;
    public TMP_Text dialogueTxt;
    public GameObject inputNamePanel;
    public TMP_InputField inputNameField;
    public GameObject yesNoPanel;
    public Button yesBttn;
    public Button noBttn;

    [Header("글루미팩")]
    public Button GPack;
    public Sprite GPackOpenedSprite;
    public GameObject gloomySleeping;
    public GameObject shadow;
    public GameObject blight;

    [Header("extra UI")]
    public Button doneBttn;
    public TMP_Text txt_InfoForClick;

    private int clickCount = 0;
    private const int clicksToOpen = 10;

    private List<string> dialogues;
    private int dialogueIndex = 0;
    private string playerName = "";
    private bool waitingForNext = false;

    void Start() //시작시,
    {
        //연구원 등장
        researcher.SetActive(true);
        shadow.SetActive(true);

        chatPanel.SetActive(false);
        inputNamePanel.SetActive(false);
        yesNoPanel.SetActive(false);

        doneBttn.gameObject.SetActive(false);
        txt_InfoForClick.gameObject.SetActive(false);
        gloomySleeping.gameObject.SetActive(false);

        LoadIntroDialogue();
        StartCoroutine(ShowBoxAfterDelay());
        //StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
    }
    IEnumerator ShowBoxAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        chatPanel.SetActive(true);
        StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
    }
    void LoadIntroDialogue()
    {
        //json파일 불러오기(Resources/JSON_intro/Intro1Dialogues.json)
        TextAsset file = Resources.Load<TextAsset>("JSON_intro/Intro1Dialogues");
        
        if (file != null){
            //가져온 json파일의 전체 문자열 가져와(file.text)-> DialogueData타입으로 파싱
            DialogueData data = JsonUtility.FromJson<DialogueData>(file.text);
            //dialogues에 파싱한 대사리스트를 저장해두기 (나중에 한줄씩 출력/타이핑효과/랜덤뽑기 기능넣기)
            dialogues = data.lines;
            Debug.Log("대사 개수: "+dialogues.Count);
        } else{
            dialogues = new List<string>{"대사를 불러오지 못했습니다."};
            Debug.Log("intro json파일 못찾음");
        }
    }
    IEnumerator TypeDialogue(string line) //해당 문자열 타이핑효과내서 나타내기
    {
        waitingForNext = false;
        dialogueTxt.text = "";

        if (playerName != "")
            line = line.Replace("{playerName}", playerName);

        foreach (char c in line) {
            dialogueTxt.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        waitingForNext = true;
    }

    public void OnBoxClicked() //chatBox클릭시(대사끝났을떄)
    {
        if(!waitingForNext) return;
        //현재 대사
        string currentLine = dialogues[dialogueIndex];
        //command Line (handling->return)
        if (currentLine.StartsWith("@")){
            HandleCommand(currentLine);
            return;
        }
        //ordinary Line
        dialogueIndex++;
        currentLine = dialogues[dialogueIndex];
        if (dialogueIndex < dialogues.Count && !dialogues[dialogueIndex].StartsWith("@")){
            StartCoroutine(TypeDialogue(currentLine));
        }
    } 
    void HandleCommand(string command)
    {
        switch (command)
        {
            case "@input_name":
                chatPanel.SetActive(false);
                inputNamePanel.SetActive(true);
                break;
            case "@yes_no":
                yesNoPanel.SetActive(true);
                chatPanel.GetComponent<Button>().interactable = false;
                break;
            case "@open_pack":
                chatPanel.SetActive(false);
                researcher.SetActive(false);
                txt_InfoForClick.gameObject.SetActive(true);
                GPack.gameObject.SetActive(true);
                break;
            case "@end":
                chatPanel.SetActive(false);
                researcher.SetActive(false);
                doneBttn.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("jsn파일 내 알수없는 명령어: "+command);
                break;
        }
    }

    public void OnNameEntered() //이름입력완료버튼에 연결
    {
        playerName = inputNameField.text;
        PlayerManager.instance.SetPlayerName(playerName);

        inputNamePanel.SetActive(false);
        chatPanel.SetActive(true);
        chatPanel.GetComponent<Button>().interactable = true;

        dialogueIndex++;
        if (dialogueIndex < dialogues.Count)
            StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
    }
    public void OnYesClicked()
    {
        yesNoPanel.SetActive(false);
        chatPanel.GetComponent<Button>().interactable = true;
        dialogueIndex++;
        if (dialogueIndex < dialogues.Count)
            StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
    }
    public void OnNoClicked()
    {
        //chatPanel.GetComponent<Button>().interactable = false;
        yesNoPanel.SetActive(false);
        chatPanel.SetActive(false);
        chatPanel.GetComponent<Button>().interactable = false;
        inputNamePanel.SetActive(true);
        dialogueIndex-= 2; // 이름 > yes/no > 그 앞 대사까지 되돌리기
    }
    public void OnIntro1Complete() //DoneBttn에 온클릭이벤트로 달려있음
    {
        //인트로1 완료 기록
        PlayerPrefs.SetInt("INTRO1_COMPLETED", 1);
        PlayerPrefs.Save();
        //글루미 배정
        GloomyManager.instance.AssignNewGloomy();
        //메인씬으로 이동
        SceneChanger.instance.ChangeScene("Scene_Main");
    }
    public void OnGloomyPackClicked()
    {
        Debug.Log("글루미팩 클릭됨");
        clickCount++;

        if (clickCount >= clicksToOpen){
            //글루미팩 > 열린이미지로 변경
            txt_InfoForClick.gameObject.SetActive(false);
            GPack.gameObject.SetActive(false);
            blight.SetActive(true);
            dialogueIndex++;
            StartCoroutine(ShowGloomyAfterDelay());
        } 
    }
    IEnumerator ShowGloomyAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        blight.SetActive(false);
        gloomySleeping.SetActive(true);
        shadow.SetActive(false);
        //doneBttn.gameObject.SetActive(true);
        StartCoroutine(ShowResearcherAfterDelay());
        StartCoroutine(TypeDialogue(dialogues[dialogueIndex]));
    }
    IEnumerator ShowResearcherAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        researcher.transform.position = new Vector3(-1.5f, researcher.transform.position.y, researcher.transform.position.z);
        researcher.SetActive(true);
        chatPanel.SetActive(true);
    }

    
}