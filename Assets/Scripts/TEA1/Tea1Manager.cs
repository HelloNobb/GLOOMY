using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// submit버튼 누르면 선택된 3가지재료정보 TEAManager에 넘기기
public class Tea1Manager : MonoBehaviour
{
    public static Tea1Manager instance;
    //selected flower Data
    public FlowerData selectedFData;
    public BaseData selectedBData;
    public AddData selectedAData;
    //selected blank
    public Image flowerBlank;
    public Image baseBlank;
    public Image addBlank;
    public TMP_Text noFlowerTxt;
    //info Panels;
    public FInfoPanel[] fInfoPanels;
    public BInfoPanel[] bInfoPanels;
    public AInfoPanel[] aInfoPanels;
    //pop-up Panel
    public GameObject warningPanel;
    public TMP_Text warningTxt;


	void Awake() {
		if (instance == null) instance = this;
        else Destroy(gameObject);
	}

	void Start() {
		flowerBlank.gameObject.SetActive(false);
        baseBlank.gameObject.SetActive(false);
        addBlank.gameObject.SetActive(false);

        noFlowerTxt.gameObject.SetActive(false);
        warningPanel.SetActive(false);
        UnActiveAllInfoPanels();
	}

    public void UnActiveAllInfoPanels(){
        foreach (FInfoPanel fInfo in fInfoPanels)
            fInfo.gameObject.SetActive(false);
        foreach (BInfoPanel bInfo in bInfoPanels)
            bInfo.gameObject.SetActive(false);
        foreach (AInfoPanel aInfo in aInfoPanels)
            aInfo.gameObject.SetActive(false);
    }
    
    // flower settings ===========================================
    public void SetF(FlowerData fData){

        noFlowerTxt.gameObject.SetActive(false);
        flowerBlank.gameObject.SetActive(false);
        selectedFData = fData;
        
        if (flowerBlank != null && selectedFData.flowerIcon != null){
            int amount;
            if (InventoryManager.instance != null){
                amount = InventoryManager.instance.inventoryData.flowerInv.GetFAmountById(fData.flowerId);
            } else{
                Debug.Log("Inv Data loading failed..");
                amount = 0;
            }
            // fill blank with icon
            if (amount > 0){
                
                flowerBlank.sprite = selectedFData.flowerIcon;
                flowerBlank.gameObject.SetActive(true);
            } else{
                selectedFData = null;
                noFlowerTxt.gameObject.SetActive(true);
            }
            
            // active only selected flower panel
            foreach (FInfoPanel fInfoPanel in fInfoPanels){
                fInfoPanel.gameObject.SetActive(false);

                if (fInfoPanel.fData == fData){
                    FInfoPanel infoPanelScript = fInfoPanel.GetComponent<FInfoPanel>();
                    infoPanelScript.SetFInfoPanel();
                    fInfoPanel.gameObject.SetActive(true);
                }
            }
        }
    }
    public FlowerData GetSelectedF(){
        return selectedFData;
    }
    // base settings ===========================================
    public void SetBase(BaseData bData){
        selectedBData = bData;

        if (baseBlank != null && selectedBData.baseImg != null){
            // fill blank with icon
            baseBlank.sprite = selectedBData.baseImg;
            baseBlank.gameObject.SetActive(true);
            // active only selected base panel
            foreach (BInfoPanel bInfoPanel in bInfoPanels){
                bInfoPanel.gameObject.SetActive(false);

                if (bInfoPanel.baseData == bData){
                    bInfoPanel.gameObject.SetActive(true);
                }
            }
        }
    }
    public BaseData GetSelectedB(){
        return selectedBData;
    }
    // add settings ===========================================
    public void SetAdd(AddData aData){
        selectedAData = aData;

        if (addBlank != null && selectedAData.addImg != null){
            // fill blank with icon
            addBlank.sprite = selectedAData.addImg;
            addBlank.gameObject.SetActive(true);
            // active only selected add panel
            foreach (AInfoPanel aInfoPanel in aInfoPanels){
                aInfoPanel.gameObject.SetActive(false);

                if (aInfoPanel.addData == aData){
                    aInfoPanel.gameObject.SetActive(true);
                }
            }
        }
    }
    public AddData GetSelectedA(){
        return selectedAData;
    }
    // submit settings ===========================================
    public void OnSubmitClicked(){
        //재료선택 3개 다 됐는지
        if (selectedFData == null || selectedBData == null || selectedAData == null){
            warningTxt.text = "재료를 모두 선택하지 않았습니다.";
            warningPanel.SetActive(true);
            return;
        } 
        TEAManager.instance?.ReceiveSelectedDatas(selectedFData, selectedBData, selectedAData);
        SceneChanger.instance?.ChangeScene("Scene_Kitchen2");
        
    }
}