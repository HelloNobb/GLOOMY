using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabTearPanel : MonoBehaviour
{
	public TMP_Text displayedTearValue;
	public TMP_Text displayedGoldValue;
	public Button upBttn;
	public Button downBttn;
	public Button SubmitBttn;

	private PlayerData player;
	private int ownedTears;
	private int goldPerTears;
	private int tearDisplay;
	private int goldDisplay;
	void Start()
	{
		gameObject.SetActive(false);
		player = PlayerManager.instance.playerData;

		ownedTears = player.accumCollectedTears;
		goldPerTears = PlayerManager.instance.GetPlayerGoldRate();

		LoadData();
	}
	void OnEnable() //오브젝트 활성화될때마다 호출됨
	{
		player = PlayerManager.instance.playerData;
		LabManager.instance.blackPanel.SetActive(true);
		LoadData();
	}
	void OnDisable()
	{
		LabManager.instance.blackPanel.SetActive(false);
	}

	public void LoadData(){
		Debug.Log("tearPanel 데이터 로드됨");
		PlayerData player = PlayerManager.instance?.playerData;
		if (player == null){
			Debug.Log("유저데이터 못받아와서 초기화함.");
			return;
		}
		//ui value reset
		goldPerTears = PlayerManager.instance.GetPlayerGoldRate();
		ownedTears = PlayerManager.instance.GetPlayerOwnedTears();
		tearDisplay = 0;
		goldDisplay = 0;
		SetTearUI();
		SubmitBttn.interactable = false;
	}
	public void SetTearUI(){
		gameObject.SetActive(true);
		
		SetUpDownBttns();
		SetGoldValue();
		displayedTearValue.text = tearDisplay.ToString();
		displayedGoldValue.text = goldDisplay.ToString();

	}
	public void SetUpDownBttns(){
		upBttn.interactable = true;
		downBttn.interactable = true;

		if (ownedTears < tearDisplay + 100){
			upBttn.interactable = false;
		}
		if (tearDisplay <= 0){
			tearDisplay = 0;
			goldDisplay = 0;
			downBttn.interactable = false;
			SubmitBttn.interactable = false;
		}
	}
	public void SetGoldValue(){
		goldPerTears = PlayerManager.instance.GetPlayerGoldRate();
		goldDisplay = (tearDisplay / 100) * goldPerTears;
	}
	public void OnUpBttnClicked(){
		if (ownedTears < tearDisplay + 100){
			Debug.Log("눈물 부족");
			SetUpDownBttns();
			return;
		}
		tearDisplay += 100;
		SetTearUI();
		SubmitBttn.interactable = true;
	}
	public void OnDownBttnClicked(){
		if (tearDisplay <= 0){
			Debug.Log("이미 0");
			SetUpDownBttns();
			return;
		}
		tearDisplay -= 100;
		SetTearUI();
	}
	public void OnSubmitClicked(){
		if (tearDisplay == 0){
			return;
		}
		LabManager.instance.TearSubmitted(tearDisplay, goldDisplay);
		gameObject.SetActive(false);
		LabChatController.instance.DisplayDialogue("tear_submitted");
	}
	public void OnCancelClicked(){
		gameObject.SetActive(false);
	}
	public bool CanAfford(int tear){
		return false;
	}
	
}