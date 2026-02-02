using UnityEngine;
using TMPro;

public class BInfoPanel : MonoBehaviour
{
	public BaseData baseData;
	public TMP_Text baseName;
	public void SetBInfoPanel(){
		if (baseData == null){
			Debug.Log("베이스 data 못받아서 로드못함");
			return;
		}
		baseName.text = baseData.baseName;
	}
}