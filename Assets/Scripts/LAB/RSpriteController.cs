using System.Collections;
using UnityEngine;

public class RSpriteController : MonoBehaviour
{
	public static RSpriteController instance;

	[Header("Sprites")]
	public Sprite defaultR;
	public Sprite madR;
	public Sprite smileR;
	public GameObject R;

	private Animator rAnim;
	private SpriteRenderer rSprite;

	void Awake(){
		if (instance == null) instance = this;
		else Destroy(instance);

		rSprite = R.GetComponent<SpriteRenderer>();
		rAnim = R.GetComponent<Animator>();
	}
	public void SetRSprite(string face){
		switch(face){
			case "Default":
				DefaultR();
				break;
			case "Mad":
				MadR();
				break;
			case "Smile":
				SmileR();
				break;
			default:
				DefaultR();
				break;
		}
	}
	public void DefaultR(){
		rSprite.sprite = defaultR;
		rAnim.enabled = true;
	}
	public void MadR(){
		rAnim.enabled = false;
		rSprite.sprite = madR;
	}
	public void SmileR(){
		rAnim.enabled = false;
		rSprite.sprite = smileR;
	}
	
}