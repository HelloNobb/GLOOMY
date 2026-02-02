using UnityEngine;
using TMPro;

public class FInfoPanel : MonoBehaviour
{
	public FlowerData fData;
	public TMP_Text fName;
	public TMP_Text fAmount;

	public void SetFInfoPanel(){
		if (fData == null) {
			Debug.Log("꽃 데이터 못받아 인포X");
			return;
		}
		fName.text = fData.flowerName;
		//fAmount.text = "보유 수량"; --추후 구현
		
		int amount;
		if(InventoryManager.instance != null){
            amount = InventoryManager.instance.inventoryData.flowerInv.GetFAmountById(fData.flowerId);
        } else{
            amount = 1;
        }
		//재고 여부에 따라 개수 문구 결정
		// if (amount <= 0){
		// 	fAmount.text = "재고 없음";
		// } else{
		// 	fAmount.text = amount.ToString();
		// }
		fAmount.text = amount.ToString();
			
	}
}