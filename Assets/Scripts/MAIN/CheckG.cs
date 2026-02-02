using TMPro;
using UnityEngine;

public class CheckG : MonoBehaviour
{
	public TMP_Text hygieneValue;

	public void Start(){
		if(GloomyManager.instance == null || GloomyManager.instance.currentGloomy == null){
			return;
		}
		hygieneValue.text = GloomyManager.instance.GetGHygiene() + "%";
	}
}