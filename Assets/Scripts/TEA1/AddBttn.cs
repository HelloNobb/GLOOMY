using TMPro;
using UnityEngine;

public class AddBttn : MonoBehaviour
{
	public AddData aData;
	
	public void OnAddClicked(){
		if (aData == null){
			Debug.Log("addData is null...");
			return;
		}
		Tea1Manager.instance?.SetAdd(aData);
	}
}