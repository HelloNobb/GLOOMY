using TMPro;
using UnityEngine;

public class PopInfoUI : MonoBehaviour
{
	public static PopInfoUI instance;
	//P
	public TMP_Text pName;
	public TMP_Text pDate;
	public TMP_Text accumedTears;
	public TMP_Text pLv;
	//G
	public TMP_Text gBlue;
	public TMP_Text gInti;
	public TMP_Text gBattery;
	public TMP_Text gMood;
	public TMP_Text gHygiene;
	public TMP_Text gHealth;
	public TMP_Text gSociality;
	public TMP_Text bestie;

	void Awake(){
		if (instance == null) instance = this;
		else Destroy(gameObject);
	}
	void OnEnable()
	{
		SetValues();
	}

	public void SetValues(){
		pName.text = PlayerManager.instance?.GetPlayerName();
		pDate.text = PlayerManager.instance?.GetPlayerHiredDate();
		accumedTears.text = PlayerManager.instance?.GetPlayerAccumTears().ToString();
		pLv.text = PlayerManager.instance?.GetPlayerPosition();
		//G
		gBlue.text = GloomyManager.instance?.GetGBlue().ToString("F2") + "%";
		gInti.text = GloomyManager.instance?.GetGInti().ToString("F2") + "%";
		gBattery.text = GloomyManager.instance?.GetGBattery().ToString();
		if (GloomyManager.instance != null){
			if (GloomyManager.instance.GetGMood()){
				gMood.text = "보통";
			} else{
				gMood.text = "우울함";
			}
		}else{
			gMood.text = "정보없음";
		}
		if (GloomyManager.instance != null){
			if (GloomyManager.instance.GetGHygiene() >= 70){
				gHygiene.text = "깨끗함";
			} else if (GloomyManager.instance.GetGHygiene() >= 30){
				gHygiene.text = "보통";
			} else{
				gHygiene.text = "더러움";
			}
		} else{
			gHygiene.text = "정보없음";
		}
		if (GloomyManager.instance != null){
			gHealth.text = GloomyManager.instance.GetGHealth() ? "건강함" : "아픔";
		} else{
			gHealth.text = "정보없음";
		}
		gSociality.text = GloomyManager.instance?.GetGSociality() + "%";
		bestie.text = GloomyManager.instance?.GetGBestie();
	}
}