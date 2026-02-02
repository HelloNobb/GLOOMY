using TMPro;
using UnityEngine;

public class BaseBttn : MonoBehaviour
{
	public BaseData bData;
	//public 
	public void OnClickBase(){
		if (bData == null){
			Debug.Log("base data is null..");
			return;
		}
		Tea1Manager.instance?.SetBase(bData);
	}
}