using UnityEngine;

public class GSpriteController : MonoBehaviour //just mainroom
{
	public static GSpriteController instance;
	// 눕는경우 : 우울도 90 이상 (누웠을땐 감정표현X)
	[Header("default G")]
	public GameObject gD; // G default
	public Animator gAnim;
	[Header("laying G")]
	public GameObject gL; // G Laying
	public Animator gLAnim;

	[Header("tmp emotion")] //잠시동안만 유지되는 표정들(전신))
	public Sprite normal;
	public Sprite smile;
	public Sprite mad;
	public Sprite sad;

	[Header("condition Effect")] //위에 덧붙이는거
	public GameObject dirty; //dirty 붙인 오브젝트
	public GameObject dirtyL;
	public GameObject sick;
	public GameObject sickL; //(누운상태만)
	//spriteRenderer
	private SpriteRenderer gSprite;
	private SpriteRenderer gLSprite;

	void Awake()
	{
		if (instance==null) instance = this;
		else Destroy(gameObject);

		gSprite = gD.GetComponent<SpriteRenderer>();
		gLSprite = gL.GetComponent<SpriteRenderer>();
	}
	public void SetGSprite(){
		//Gdata 없으면 default 모드로 셋팅
		if (GloomyManager.instance == null){
			Debug.Log("[GSprite] gManager null");
			SetDefaultG();
			return;
		}
		//lay or not
		if (GloomyManager.instance.blueState == GloomyManager.BlueState.BlueHigh){
			Debug.Log("[GSprite] blue high");
			SetLayingG();
		} else {
			Debug.Log("[GSprite] blue normal");
			SetDefaultG();
		}
	}
	//앉거나 눕거나 + 아픔/더러움 효과 
	public void SetDefaultG(){ //sittingG
		if (gL.activeSelf) gL.SetActive(false);
		gD.SetActive(true);
		
		gAnim.enabled = true; //깜빡임
		gSprite.sprite = normal;
		//상태효과 설정(아픔/더러움)
		SetEffects();
	}
	public void SetLayingG(){
		if (gD.activeSelf) gD.SetActive(false);
		gL.SetActive(true);

		gLAnim.enabled=true;
		//상태효과
		SetEffects();
	}
	public void SetEffects(){
		sick.SetActive(false);
		sickL.SetActive(false);
		dirty.SetActive(false);
		dirtyL.SetActive(false);
		//sick / dirty
		if (GloomyManager.instance == null){
			Debug.Log("[GSprite] gmanager 없어서 아픔/더러움정보없음");
			return;
		} 
		if (GloomyManager.instance.IsSick()){
			if (gD.activeSelf) sick.SetActive(true);
			else if (gL.activeSelf) sickL.SetActive(true);
		}
		if (GloomyManager.instance.IsDirty()){
			if (gD.activeSelf) dirty.SetActive(true);
			else if (gL.activeSelf) dirtyL.SetActive(true);
		}
	}

	// emotions ========================== 
	// (애초에 아플땐 아픈 대사만, 우울할땐 우울한 대사만 출력해서 상관없음)
	public void Normal(){
		if (gL.activeSelf) return;
		
		gAnim.enabled = true;
		gSprite.sprite = normal;
	}
	public void Smile(){
		if (gL.activeSelf) return;

		gAnim.enabled = false;
		gSprite.sprite = smile;
	}
	public void Mad(){
		if (gL.activeSelf) return;
		
		if (sickL.activeSelf || gL.activeSelf){
			return;
		}
		gAnim.enabled = false;
		gSprite.sprite = mad;
	}
	public void Sad(){
		if (gL.activeSelf) return;
		
		gAnim.enabled = false;
		gSprite.sprite = sad;
	}
	// emotion ==========================
}