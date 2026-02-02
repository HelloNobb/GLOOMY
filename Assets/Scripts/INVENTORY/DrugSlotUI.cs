using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrugSlotUI : MonoBehaviour
{
	public Image dIcon;
	public TMP_Text dName;
	public TMP_Text dInfo;
	public TMP_Text dOwned;

	public void SetUI(MedicineData mData, int amount){
	
		dIcon.sprite = mData.mediImg;
		dName.text = mData.name;
		dInfo.text = mData.description;
		dOwned.text = "x" + amount.ToString();
	}
}