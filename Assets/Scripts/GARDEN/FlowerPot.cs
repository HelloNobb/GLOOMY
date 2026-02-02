//받은 정보에 따라 UI,꽃 모습 변경 담당
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FlowerPot : MonoBehaviour
{
    public FlowerData currentFlower;
    public float growTimer;//sec
    public bool isPlanted;
    public DateTime plantTime;
    
    public GameObject flowerObj;
    public SpriteRenderer flowerRenderer;
    [Header("flowerInfoPanel")]
    public GameObject fPanel;
    public GameObject fBackLight;
    public TMP_Text fName;
    public TMP_Text timer;

    private Coroutine autoHideCo;
    private Collider2D flowerCollider; //다 자란 경우에만 활성화
    

    void Start()
    {
        fBackLight.SetActive(false);
        fPanel.SetActive(false);
        flowerCollider = flowerObj.GetComponent<Collider2D>();
        flowerCollider.enabled = false;
    }
    void Update()
    {
        if (!isPlanted || currentFlower == null) return;

        growTimer += Time.deltaTime;
        UpdateSprite();
        UpdateTimeDisplay();

        if (growTimer >= currentFlower.growthTimeSeconds){ //다 자라면
            //growTimer = currentFlower.growthTimeSeconds;
            if (!flowerCollider.enabled)
                flowerCollider.enabled = true;
                fBackLight.SetActive(true);
            return;
        }

        if (flowerCollider.enabled)
            flowerCollider.enabled = false;
            
    }
    //꽃 심으면,
    public void PlantFlower(FlowerData flower, DateTime plantedTime){
        isPlanted = true;
        currentFlower = flower;
        plantTime = plantedTime;

        growTimer = (float)(DateTime.Now - plantedTime).TotalSeconds;
        UpdateSprite();
        flowerObj.SetActive(true);
    }
    public void EmptyFlower(){
        growTimer = 0f;
        isPlanted = false;
        currentFlower = null;

        flowerObj.SetActive(false);
    }
    //꽃이미지 타이머에 따라 변경
    void UpdateSprite(){
        if (currentFlower == null) return;
        fBackLight.SetActive(false);
        float growthTotalTime = currentFlower.growthTimeSeconds;
        if (growTimer >= growthTotalTime){
            flowerRenderer.sprite = currentFlower.flowerImage_Adult;
            fBackLight.SetActive(true);
        } else if (growTimer >= growthTotalTime/2){
            flowerRenderer.sprite = currentFlower.flowerImage_Child;
        } else if (growTimer >= growthTotalTime/5){
            flowerRenderer.sprite = currentFlower.flowerImage_Baby;
        } else
            flowerRenderer.sprite = currentFlower.flowerImage_Seed;
    }
    //public FlowerData GetCurrentFlower() => currentFlower;

    public float GetRemainingGrowthTime(){
        if (!isPlanted || currentFlower == null) return 0f;
        return (currentFlower.growthTimeSeconds - growTimer);
    }

    public void ShowStatus(){
        fName.text = currentFlower.flowerName;
        //fInfo.text = currentFlower.description;
        //타이머표시
        UpdateTimeDisplay();
        autoHideCo = StartCoroutine(AutoHideAfterDelay(2f));
    }
    void UpdateTimeDisplay(){
        float leftSec = GetRemainingGrowthTime();
        string displayTime;

        if (leftSec >= 3600f){
            int hour = Mathf.FloorToInt(leftSec / 3600f);
            int min = Mathf.FloorToInt(leftSec % 3600f / 60f);
            displayTime = $"{hour}시간 {min}분";
        } else if (leftSec >= 60f){
            int min = Mathf.FloorToInt(leftSec/60f);
            int sec = Mathf.FloorToInt(leftSec % 60f);
            displayTime = $"{min}분 {sec}초";
        } else if (leftSec >= 0f) {
            int sec = Mathf.FloorToInt(leftSec);
            displayTime = $"{sec}초";
        } else {
            displayTime = "성장 완료";
        }
        timer.text = displayTime;
    }
    public void HideStatus(){
        fPanel.SetActive(false);

        if (autoHideCo != null){
            StopCoroutine(autoHideCo);
            autoHideCo = null;
        }
    }
    /// 성장 끝난 꽃
    public bool IsDoneGrowing(){
        if (growTimer > currentFlower.growthTimeSeconds){
            return true;
        }
        return false;
    }
    public void Harvest(){
        Debug.Log("꽃 수확함");
        //특수효과 및 인벤토리 추가
        InventoryManager.instance?.SetOwnedFlower(currentFlower.flowerId, 1);
        fBackLight.SetActive(false);
        EmptyFlower();
        HideStatus();
    }
    public void Discard(){
        Debug.Log("꽃 버림");
        if (fBackLight.activeSelf) 
            fBackLight.SetActive(false);
        EmptyFlower();
        HideStatus();
    }

    private IEnumerator AutoHideAfterDelay(float sec){
        yield return new WaitForSeconds(sec);
        HideStatus();
    }
}