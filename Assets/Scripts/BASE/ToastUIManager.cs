using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic; //큐

public class ToastUIManager : MonoBehaviour
{
	public static ToastUIManager instance;

	public CanvasGroup toastCGoup;
	public GameObject toastPanel;
	public TMP_Text toastTxt;
	Coroutine showingToast;
	private float duration = 2f;
	private Queue<string> toastQueue = new Queue<string>();
	private bool isShowing = false;

	void Awake(){
		if (instance == null) instance = this;
		else Destroy(gameObject);

		//DontDestroyOnLoad(gameObject);
		toastCGoup.alpha = 0f;
	}
	public void ShowToast(string txt){
		toastQueue.Enqueue(txt);
		if (!isShowing){
			StartCoroutine(ProcessQueue());
		}
	}
	IEnumerator ProcessQueue(){
		isShowing = true;

		while (toastQueue.Count > 0){
			string txt = toastQueue.Dequeue();
			toastTxt.text = txt;

			//fade-in
			yield return StartCoroutine(Fade(0f, 1f, 0.3f));
			//wait
			yield return new WaitForSeconds(duration);
			//fade-out
			yield return StartCoroutine(Fade(1f,0f,0.3f));
		}
		isShowing = false;
	}
	IEnumerator Fade(float from, float to, float time){
		float elapsed = 0f;

		while (elapsed < time){
			elapsed += Time.deltaTime;
			toastCGoup.alpha = Mathf.Lerp(from,to,elapsed/time);
			yield return null;
		}
		toastCGoup.alpha = to;
	}
}