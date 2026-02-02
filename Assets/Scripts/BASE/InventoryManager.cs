//인벤토리 :: 약,꽃,기타아이템들
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryData inventoryData;
    [Header("InventoryBttnSettings")]
    public GameObject newImg;//인벤토리 추가되면 인벤토리버튼옆 빨간이미지 활성화
    public Button inventoryBttn;
    public Button cancelBttn;

    [Header("Scroll Views")]
    public GameObject itemScrollView;
    public GameObject flowerScrollView;
    public GameObject medicineScrollView;
    [Header("UI Prefabs")]
    public GameObject flowerSlotPrefab;
    public GameObject medicineSlotPrefab;
    [Header("Parent Transforms")] //스크롤뷰 안의 content오브젝트
    public Transform flowerContentParent;
    public Transform medicineContentParent;

    [Header("Catergory Bttns")]
    public Button itemBttn;
    public Button flowerBttn;
    public Button medicineBttn;
    [Header("Inventory BG")]
    public Image panelBackground; //판넬이미지
    //버튼이미지들
    private Sprite itemDefaultImg;
    private Sprite itemSelectedImg;
    private Sprite flowerDefaultImg;
    private Sprite flowerSelectedImg;
    private Sprite medicineDefaultImg;
    private Sprite medicineSelectedImg;
    //판넬에 넣을 이미지들
    private Sprite defaultBG;
    private Sprite itemBG;
    //스크롤 렉스
    public ScrollRect itemScrollRect;
    public ScrollRect flowerScrollRect;
    public ScrollRect medicineScrollRect;
    //scriptableObjs
    [Header("Real Datas")]
    public FlowerData[] flowerDatas;
    public MedicineData[] medicineDatas;

    void Awake(){
        if (instance == null) instance = this;
        else Destroy(gameObject);

        LoadInventory();
    }
    void Start()
    {
        //카테고리 버튼이미지 3개 (기본,선택 버전) 로드
        itemDefaultImg = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/inventory_item");
        itemSelectedImg = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/inventory_item_pink");
        flowerDefaultImg = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/inventory_flower");
        flowerSelectedImg = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/inventory_flower_pink");
        medicineDefaultImg = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/inventory_medicine");
        medicineSelectedImg = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/inventory_medicine_pink");
        //배경이미지 로드
        defaultBG = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/Inventory_panel_default");
        itemBG = Resources.Load<Sprite>("Art/Sprites/UI/Inventory/Inventory_panel_itm");
        //데이터 준비
        OnClickCategory("flower");
        
    }
	void OnApplicationQuit(){
		SaveInventory();
	}
	// abilities =========================================
    public void OnClickInvBttn(){
        if (newImg.activeSelf) 
            newImg.SetActive(false);
    }
	public void OnClickCategory(string categoryName)
    {
        //버튼 색깔- 모두 default로 초기화
        itemBttn.image.sprite = itemDefaultImg;
        flowerBttn.image.sprite = flowerDefaultImg;
        medicineBttn.image.sprite = medicineDefaultImg;
        //스크롤뷰- 모두 비활성화로 초기화
        itemScrollView.SetActive(false);
        flowerScrollView.SetActive(false);
        medicineScrollView.SetActive(false);
        //데이터 업데이트
        
        //선택한 카테고리명에 따라 열기
        switch (categoryName)
        {
            case "item":
                itemScrollView.SetActive(true);
                itemBttn.image.sprite = itemSelectedImg;
                panelBackground.sprite = itemBG;
                itemScrollRect.verticalNormalizedPosition = 1f; //스크롤바 맨위로
                RefreshItemUI();
                break;
            case "flower":
                flowerScrollView.SetActive(true);
                flowerBttn.image.sprite = flowerSelectedImg;
                panelBackground.sprite = defaultBG;
                flowerScrollRect.verticalNormalizedPosition = 1f; //스크롤바 맨위로
                RefreshFlowerUI();
                break;
            case "medicine":
                medicineScrollView.SetActive(true);
                medicineBttn.image.sprite = medicineSelectedImg;
                panelBackground.sprite = defaultBG;
                medicineScrollRect.verticalNormalizedPosition = 1f; //스크롤바 맨위로
                RefreshMedicineUI();
                break;
        }

    }
    // Inventory Data Settings ====================================
    public void SaveInventory()
    {
        SaveSystem.SaveToFile<InventoryData>("INVENTORY_DATA", inventoryData);
    }
    public void LoadInventory()
    {
        InventoryData loadedData = SaveSystem.LoadFromFile<InventoryData>("INVENTORY_DATA");
        
        if (loadedData == null){
            Debug.Log("[Inventory] 저장된 데이터 없음. 초기화 진행!");
            ResetInventory();
        } else {
            Debug.Log("[Inventory] 저장된 데이터 불러옴!");
            inventoryData = loadedData;
        }
        //refresh UI
        RefreshFlowerUI();
        RefreshItemUI();
        RefreshMedicineUI();
    }
    public void ResetInventory(){
        inventoryData = new InventoryData();
        SaveInventory();
        //refresh UI
        if (newImg.activeSelf) 
            newImg.SetActive(false);
        RefreshFlowerUI();
        RefreshItemUI();
        RefreshMedicineUI();
    }
    // Data Update ====================================
    public void SetOwnedMedicine(string id, int amount){
        switch (id){
            case "tear":
                inventoryData.medicineInv.tear += amount;
                break;
            case "blue":
                inventoryData.medicineInv.blue += amount;
                break;
            case "happy":
                inventoryData.medicineInv.happy += amount;
                break;
            case "health":
                inventoryData.medicineInv.health += amount;
                break;
            case "battery":
                inventoryData.medicineInv.battery += amount;
                break;
            case "sleep":
                inventoryData.medicineInv.sleep += amount;
                break;
            default:
                Debug.Log("잘못된 이름의 약 호출됨!");
                break;
        }
        if (amount > 0){
            newImg.SetActive(true);
        }
        SaveInventory();
        RefreshMedicineUI();
    }
    public void SetOwnedFlower(string id, int amount){
        switch(id){
            case "lav":
                inventoryData.flowerInv.lav += amount;
                break;
            case "jas":
                inventoryData.flowerInv.jas += amount;
                break;
            case "cham":
                inventoryData.flowerInv.cham += amount;
                break;
            case "lotus":
                inventoryData.flowerInv.lotus += amount;
                break;
            case "cherry":
                inventoryData.flowerInv.cherry += amount;
                break;
            case "rose":
                inventoryData.flowerInv.rose += amount;
                break;
            case "dand":
                inventoryData.flowerInv.dand += amount;
                break;
            case "mari":
                inventoryData.flowerInv.mari += amount;
                break;
            case "pansy":
                inventoryData.flowerInv.pansy += amount;
                break;
            case "berga":
                inventoryData.flowerInv.berga += amount;
                break;
            default:
                Debug.Log("잘못된 꽃이름 호출됨!");
                break;
        }
        if (amount > 0){
            newImg.SetActive(true);
        }
        SaveInventory();
        RefreshFlowerUI();
    }
    public void SetOwnedItem(string id, bool isOwned){
        switch (id){
            case "teddyBear":
                inventoryData.itemInv.teadyBear = isOwned;
                break;
            default:
                Debug.Log("잘못된 아이템 이름 호출됨!");
                break;
        }
        if (isOwned){
            newImg.SetActive(true);
        }
        SaveInventory();
        RefreshItemUI();
    }

    // UI Update ====================================
    public void RefreshFlowerUI(){
        Debug.Log("꽃 UI resfreshed!");
        //기존 슬롯 제거
        foreach (Transform child in flowerContentParent){
            Destroy(child.gameObject);
        }
        //create new slot
        foreach (FlowerData f in flowerDatas){
            int count = inventoryData.flowerInv.GetFAmountById(f.flowerId);
            if (count <= 0) continue; //0개면 생략

            GameObject slot = Instantiate(flowerSlotPrefab, flowerContentParent);
            FlowerSlotUI ui = slot.GetComponent<FlowerSlotUI>();
            ui.SetUI(f, count);
        }
    }
    public void RefreshMedicineUI(){
        Debug.Log("약 UI resfreshed!");
        //기존 슬롯 제거
        foreach (Transform child in medicineContentParent){
            Destroy(child.gameObject);
        }
        //create new slot
        foreach (MedicineData m in medicineDatas){
            int count = inventoryData.medicineInv.GetMAmountById(m.id);
            if (count <= 0) continue;

            GameObject slot = Instantiate(medicineSlotPrefab, medicineContentParent);
            DrugSlotUI dUI = slot.GetComponent<DrugSlotUI>();
            dUI.SetUI(m, count);
        }
    }
    public void RefreshItemUI(){
        Debug.Log("아이템 UI resfreshed!");
    }
}