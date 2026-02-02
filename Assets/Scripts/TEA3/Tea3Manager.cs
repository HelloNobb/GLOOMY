using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tea3Manager : MonoBehaviour
{
    public static Tea3Manager instance;
    // cup fields ================================
    public Animator anim_dragCup;
    public GameObject cup; 
    public GameObject fb;
    public GameObject a;
    //애니메이션재생후 위치 지키기 위해, spriteRenderer.enabled로 비활 조정하기
    public GameObject dragSign;
    public GameObject cupDragger;

    Coroutine waitAndInfoDrag;
    Coroutine dragAndFreezeCup;
    //컵원래위치
    Vector3 initialPos = new Vector3(-0.641f, -3.535f, 0.01615f);
    Vector3 initialScale = new Vector3(0.26189f, 0.26189f, 0.26189f);
    // g fields ================================
    public GameObject G; //글루미
    public GameObject chatPanel;
    public TMP_Text chatTxt;
    public Sprite G_Drink;
    public Sprite G_Default;
    // data
    public FlowerData fData;
    public BaseData bData;
    public AddData aData;

	void OnEnable(){
		ApplyCupState(initialPos, initialScale);
	}
	void Awake(){
		if (instance == null) instance = this;
        else Destroy(gameObject);
	}

	void Start(){
		
        cup.SetActive(true);
        dragSign.SetActive(false);
        cupDragger.SetActive(false);
        waitAndInfoDrag = StartCoroutine(WaitAndInfoDrag());

        G.SetActive(true);
        chatPanel.SetActive(false);
        G.GetComponent<SpriteRenderer>().sprite = G_Default;
        //차 데이터 불러오기
        SetTeaDatas();


	}
    public void SetTeaDatas(){
        if (TEAManager.instance == null) {
            FinishTeaTime();
            return;
        }
        TEAManager TM = TEAManager.instance;
        fData = TM.selectedFlower;
        aData = TM.selectedAdd;
        bData = TM.selectedBase;

        if (bData.baseName == "우유"){
            fb.GetComponent<SpriteRenderer>().sprite = fData.withMilkImg;
            a.GetComponent<SpriteRenderer>().sprite = aData.milkAddImg;
        } else if (bData.baseName == "물"){
            fb.GetComponent<SpriteRenderer>().sprite = fData.withWaterImg;
            a.GetComponent<SpriteRenderer>().sprite = aData.waterAddImg;
        }
        
    }

    IEnumerator WaitAndInfoDrag(){
        yield return new WaitForSeconds(2f);
        dragSign.SetActive(true);
        cupDragger.SetActive(true);
    }

    public void AfterDragCup(){
        dragAndFreezeCup = StartCoroutine(PlayAndFreezeCupState());
    }
    IEnumerator PlayAndFreezeCupState(){
        anim_dragCup.SetTrigger("SlideCup");
        float animLength = anim_dragCup.runtimeAnimatorController.animationClips[0].length;
        yield return new WaitForSeconds(animLength);
        //yield return new WaitForSeconds(1.5f);
        anim_dragCup.enabled = false;
        dragSign.SetActive(false);
        //현재상태의 스케일,위치로 컵 고정시키기(현재씬에있는동안)
        Vector3 finPos = cup.transform.position;
        Vector3 finScale = cup.transform.localScale;
        ApplyCupState(finPos, finScale);
        StartCoroutine(DrinkAndFinish());
    }
    IEnumerator DrinkAndFinish(){
        yield return new WaitForSeconds(2f);
        cup.SetActive(false);
        G.GetComponent<SpriteRenderer>().sprite = G_Drink;
        yield return new WaitForSeconds(2f);
        cup.SetActive(true);
        G.GetComponent<SpriteRenderer>().sprite = G_Default;
        yield return new WaitForSeconds(1f);
        chatTxt.text = "맛있어요. 잘 마실게요. 고마워요!";
        chatPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        FinishTeaTime();
    }
    public void FinishTeaTime(){
        if (SceneChanger.instance == null) return;
        SceneChanger.instance.ChangeScene("Scene_Main");
    }

    public void ApplyCupState(Vector3 pos, Vector3 scale){
        cup.transform.position = pos;
        cup.transform.localScale = scale;
    }
}