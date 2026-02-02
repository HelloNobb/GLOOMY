using UnityEngine;
using TMPro;

public class AInfoPanel : MonoBehaviour
{
	public AddData addData;
	public TMP_Text addName;
	public void SetBInfoPanel(){
		if (addData == null){
			Debug.Log("add data 못받아서 로드못함");
			return;
		}
		addName.text = addData.addName;
	}
}