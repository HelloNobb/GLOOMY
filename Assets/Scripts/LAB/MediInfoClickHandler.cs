using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MediInfoClickHandler : MonoBehaviour
{
	public MedicineData mediData;
	public TMP_Text mediName;
	public TMP_Text price;
	private Image img;
	private float fadeDuration = 0.3f;
	private float showTime = 2f;

	void Awake(){
		img = GetComponent<Image>();

		mediName.text = mediData.displayName;
		if (mediData.price == -1){
			price.text = "???";
		} else{
			price.text = mediData.price.ToString() + " G";
		}
		
	}
	public string GetInfoId(){
		return mediData.id;
	}
	// info panel active settings ====================
	public void SetInfoActive(){
		gameObject.SetActive(true);
		StartCoroutine(TenderActive());
	}
	public void SetInfoUnActive(){
		StartCoroutine(TenderUnActive());
	}

	private IEnumerator TenderActive(){
		yield return StartCoroutine(UIFader.FadeImg(img, 1f, fadeDuration));
		yield return new WaitForSeconds(showTime);
		yield return StartCoroutine(TenderUnActive());
	}
	private IEnumerator TenderUnActive(){
		yield return StartCoroutine(UIFader.FadeImg(img, 0f, fadeDuration));
		gameObject.SetActive(false);
	}
}