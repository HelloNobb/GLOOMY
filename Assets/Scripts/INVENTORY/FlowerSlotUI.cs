using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowerSlotUI : MonoBehaviour
{
	public Image fIcon;
	public TMP_Text fName;
	public TMP_Text fInfo;
	public TMP_Text fOwned;

	public void SetUI(FlowerData fData, int amount){
	
		fIcon.sprite = fData.flowerIcon;
		fName.text = fData.flowerName;
		fInfo.text = fData.description;
		fOwned.text = "x" + amount.ToString();
	}
}