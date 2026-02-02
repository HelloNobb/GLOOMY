//가든 데이터,기능 관리
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GardenManager : MonoBehaviour
{
    public static GardenManager instance;

    [Header("Obj")]
    public GameObject[] potObj;
    public GameObject[] flowerObj; 

    [Header("Flower Data")]
    public FlowerData[] allFlowers;

    [Header("UI")]
    public GameObject flowerSelectPanel;
    public Button flowerSelectCancel;
    public GameObject flowerBttnPrefab;
    public Transform flowerIconGrid; //flower grid group parent
    public FlowerCard flowerCardPanel;
    public GameObject[] fInfoPanels;

    private int selectedPotIndex = -1;
    public static string gardenFileName = "GARDEN_DATA";
    //꽃 더블탭 감지
    private float lastTapTime = 0.0f;
    private float doubleTapTerm = 0.3f;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start(){
        flowerSelectPanel.SetActive(false);
        flowerCardPanel.gameObject.SetActive(false);

        PutFlowerSelection();
        LoadGardenData();
    }
	void OnApplicationQuit(){
		SaveGardenData();
	}
	// Find F Methods ========================================
	FlowerData FindFlowerByName(string name){
        foreach (var flower in allFlowers){
            if (name == flower.flowerName)
                return flower;
        }
        return null;
    }
    int GetFlowerIndexByName(string name){
        for (int i = 0; i < allFlowers.Length; i++){
            if (allFlowers[i].flowerName == name){
                return i;
            }
        }
        return -1;
    }
    // Set Garden Data ========================================
    public void LoadGardenData(){

        GardenData gardenData = SaveSystem.LoadFromFile<GardenData>(gardenFileName);
        //no file/data => reset
        if (gardenData == null || gardenData.pots == null || gardenData.pots.Length != potObj.Length){
            Debug.Log("[Garden]초기 데이터 없음. 빈 상태로 시작");        
            //ResetGardenData();
            return;
        }

        for (int i = 0; i < potObj.Length; i++){
            PotData pot = gardenData.pots[i];
            FlowerPot potScript = potObj[i].GetComponent<FlowerPot>();

            if (pot.isPlanted){ //plant!
                FlowerData f = FindFlowerByName(pot.flowerName);
                DateTime plantedT = DateTime.Parse(pot.plantedTimeStr);
                potScript.PlantFlower(f, plantedT);
                Debug.Log($"{i}th pot planted!");
            } else{
                potScript.EmptyFlower();
                Debug.Log($"{i}th pot Empty!");
            }
        }
    }
    public void SaveGardenData(){
        GardenData gardenData = new GardenData();
        gardenData.pots = new PotData[potObj.Length];

        for (int i = 0; i < potObj.Length; i++){
            FlowerPot potScript = potObj[i].GetComponent<FlowerPot>();
            if (potScript.isPlanted){
                gardenData.pots[i] = new PotData {
                    isPlanted = true,
                    flowerName = potScript.currentFlower.flowerName,
                    plantedTimeStr = potScript.plantTime.ToString()
                };
            } else{
                gardenData.pots[i] = new PotData {
                    isPlanted = false
                };
            }
        }
        SaveSystem.SaveToFile(gardenFileName, gardenData);
    }
    public void ResetGardenData(){

        SaveSystem.DeleteFile(gardenFileName);//파일없어도 괜찮음
        GardenData gardenData = new GardenData();
        gardenData.pots = new PotData[potObj.Length];

        for (int i = 0; i < gardenData.pots.Length; i++){
            gardenData.pots[i] = new PotData { isPlanted = false };
            potObj[i].GetComponent<FlowerPot>().EmptyFlower();
        }
        SaveSystem.SaveToFile(gardenFileName, gardenData);
    }
    public void SavePotData(int potIndex, int fIndex, DateTime plantedTime){
        
        GardenData gardenData = SaveSystem.LoadFromFile<GardenData>(gardenFileName);
        //가든데이터 초기화 필요할 경우 초기화
        if (gardenData == null || gardenData.pots == null || gardenData.pots.Length != potObj.Length){
            gardenData = new GardenData();
            gardenData.pots = new PotData[potObj.Length];
            for (int i = 0; i < potObj.Length; i++)
                gardenData.pots[i] = new PotData{ isPlanted=false };
        }
        //저장할 화분 데이터 받기
        gardenData.pots[potIndex] = new PotData{
            isPlanted = true,
            flowerName = allFlowers[fIndex].flowerName,
            plantedTimeStr = plantedTime.ToString()
        };
        //파일에 저장
        SaveSystem.SaveToFile<GardenData>(gardenFileName, gardenData);
    }
    //빈 화분 클릭시,
    public void OnEmptyPotClicked(int potIndex){
        selectedPotIndex = potIndex;
        flowerSelectPanel.SetActive(true);
        Debug.Log("빈 화분 클릭됨!");
    }
    //안 빈 화분 클릭시,
    public void OnFilledPotClicked(int potIndex){
        selectedPotIndex = potIndex;
        Debug.Log("꽃 심어진 화분 클릭됨!");
        
        FlowerPot potScript = potObj[potIndex].GetComponent<FlowerPot>();//selectedPot's script
        potScript.ShowStatus();
        SetActiveOnlyOnePanel(selectedPotIndex);
    }
    //다 자란 꽃 클릭시,
    public void OnGrownFlowerDoubleClicked(int fIndex){
        selectedPotIndex = fIndex;
        //다 자랐는지 확인
        FlowerPot potScript = potObj[fIndex].GetComponent<FlowerPot>();
        if (!potScript.IsDoneGrowing()){
            Debug.Log("아직 덜 자람");
            return;
        }
        //더블탭 처리
        float currentTime = Time.time;
        if (currentTime - lastTapTime < doubleTapTerm){
            potScript.Harvest();
        }
        lastTapTime = currentTime;
    }
    //(처음) 꽃목록 받아다 자식꽃버튼으로 로드
    void PutFlowerSelection() {

        foreach (Transform child in flowerIconGrid)
            Destroy(child.gameObject);
        
        for (int i = 0; i < allFlowers.Length; i++){
            int flowerIndex = i;
            GameObject bttnObj = Instantiate(flowerBttnPrefab, flowerIconGrid);//그리드자식으로 버튼생성
            FlowerBttn bttn = bttnObj.GetComponent<FlowerBttn>();//버튼의 fbttn컴포넌트 불러와 bttn에 저장
            bttn.Setup(allFlowers[i], ()=> OnFlowerSelected(flowerIndex));
        }
    }
    public void OnSelectCancelBttnClicked(){
        flowerSelectPanel.SetActive(false);
    }
    //꽃버튼 선택 -> 해당 꽃카드 조회
    void OnFlowerSelected(int flowerIndex){
        flowerCardPanel.Show(allFlowers[flowerIndex], ()=> ConfirmPlantFlower(flowerIndex));
    }
    
    //꽃카드의 심기 선택 -> 꽃 심기
    void ConfirmPlantFlower(int flowerIndex){
        //오류 시 리턴
        if(selectedPotIndex < 0 || selectedPotIndex >= potObj.Length) return;
        //해당화분 컴포넌트 가져오기
        FlowerPot potScript = potObj[selectedPotIndex].GetComponent<FlowerPot>();
        DateTime now = DateTime.Now;
        //심기 처리
        potScript.PlantFlower(allFlowers[flowerIndex], now);
        SavePotData(selectedPotIndex, flowerIndex, now);

        flowerSelectPanel.SetActive(false);
    }
    ///////////////fInfoPanel
    public void SetActiveOnlyOnePanel(int activeIndex){
        for (int i = 0; i < fInfoPanels.Length; i++){
            fInfoPanels[i].SetActive(i == activeIndex);
        }
    }
    
}