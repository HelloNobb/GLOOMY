using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JustChatController : MonoBehaviour{
	public static JustChatController instance;
	[Header("UIs")]
	public GameObject chatPanel;
	public TMP_Text speaker;
	public TMP_Text txt;
	public Dictionary<string, Sprite> speakerFace; //string: json face:~~ 이거
	public TextAsset[] jsonFiles;

	void Awake(){
		if (instance == null) instance=this;
		else Destroy(gameObject);
	}
}