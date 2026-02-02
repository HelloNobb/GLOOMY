using UnityEngine;
using UnityEngine.UI;

public class FBoxBttn : MonoBehaviour
{
	public FlowerData fData;
	public int amount;
	public Image selfImg;
	public Sprite emptyJar;
	public Sprite fullJar;

	void Start(){
		
		SetJar();
	}
	public void SetJar(){
		if (InventoryManager.instance == null) return;
		
		amount = InventoryManager.instance.inventoryData.flowerInv.GetFAmountById(fData.flowerId);
		if (amount > 0){
			selfImg.sprite = fullJar;
		} else{
			selfImg.sprite = emptyJar;
		}
	}
	public void OnClickFBox(){
		if (fData == null){
			Debug.Log("해당 꽃 데이터 없음");
			return;
		} 

		Tea1Manager.instance?.SetF(fData);
		//InfoManager.instance?.ShowOnlyThisPanel(fInfoPanel);
		
	}
}