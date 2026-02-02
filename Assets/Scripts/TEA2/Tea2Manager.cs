using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tea2Manager : MonoBehaviour
{
    public static Tea2Manager instance;
    // datas
    public FlowerData fData;
    public BaseData bData;
    public AddData aData;
    // Objs
    public GameObject base1; //
    public GameObject f1; //
    public Button machineBttn;
    public GameObject addObj; //

    public GameObject base2; //
    public GameObject cupFB; //
    public GameObject cupA; //

    public GameObject blanksParent;
    public Button blankF;
    public Button blankB;
    public Button blankA;
    public Image fIcon;
    public Image bIcon;
    public Image aIcon;
    public Button doneBttn;
    //effect
    public GameObject bLight_machineBttn;
    public GameObject bLight_AddObj;
    //animator
    public Animator b1_Anim;
    public Animator b2_Anim;
    public Animator blanksDown_Anim;
    public Animator fb_Anim;
    private Coroutine blanksDown;
    private Coroutine dropTea;
    //values
    public bool isDropped = false;

	void Awake(){
		if (instance == null) instance = this;
        else Destroy(gameObject);
	}

	void Start(){
		isDropped = false;
        base1.SetActive(false);
        f1.SetActive(false);
        addObj.SetActive(false);
        base2.SetActive(false);
        cupFB.SetActive(false);
        cupA.SetActive(false);
        doneBttn.enabled = true;

        blanksParent.SetActive(true);
        fIcon.gameObject.SetActive(true);
        bIcon.gameObject.SetActive(true);
        aIcon.gameObject.SetActive(true);

        bLight_AddObj.SetActive(false);
        bLight_machineBttn.SetActive(false);

        SetIngreds();
	}

    public void SetIngreds(){
        fData = TEAManager.instance?.selectedFlower;
        bData = TEAManager.instance?.selectedBase;
        aData = TEAManager.instance?.selectedAdd;
        //Set Blank Icons
        fIcon.sprite = fData.flowerIcon;
        bIcon.sprite = bData.baseImg;
        aIcon.sprite = aData.addImg;
        //Set Obj Imgs
        base1.GetComponent<SpriteRenderer>().sprite = bData.machineBaseImg;
        base2.GetComponent<SpriteRenderer>().sprite = bData.machineBaseImg2;
        f1.GetComponent<SpriteRenderer>().sprite = fData.machineFlowerImg;
        addObj.GetComponent<SpriteRenderer>().sprite = aData.addImg;
        
        if (bData.baseName == "우유"){
            cupFB.GetComponent<SpriteRenderer>().sprite = fData.withMilkImg;
            cupA.GetComponent<SpriteRenderer>().sprite = aData.milkAddImg;
        } else {  //else if (bData.baseName == "물")
            cupFB.GetComponent<SpriteRenderer>().sprite = fData.withWaterImg;
            cupA.GetComponent<SpriteRenderer>().sprite = aData.waterAddImg;
        }
    }

    
    // Set BlankBttns ==========================
    public void OnClickFBlank(){
        fIcon.gameObject.SetActive(false);
        f1.SetActive(true);
        SetDownBlanks();
    }
    public void OnClickBBlank(){
        bIcon.gameObject.SetActive(false);
        base1.SetActive(true);
        SetDownBlanks();
    }
    public void OnClickABlank(){
        aIcon.gameObject.SetActive(false);
        addObj.SetActive(true);
        SetDownBlanks();
    }
    public void SetDownBlanks(){
        if (!fIcon.gameObject.activeSelf && !bIcon.gameObject.activeSelf && !aIcon.gameObject.activeSelf){
            //애니메이션 진행 후 unactive
            blanksDown = StartCoroutine(WaitAndUnActiveBlanks());
        }
    }
    IEnumerator WaitAndUnActiveBlanks(){
        blanksDown_Anim.SetTrigger("BlankBttns_GoDown");
        yield return new WaitForSeconds(1f);
        blanksParent.SetActive(false);
        bLight_machineBttn.SetActive(true);
    }
    // Set ObjBttns ==========================
    public void OnClickMachineBttn(){
        //재료 넣은 상태인지 확인
        if (fIcon.gameObject.activeSelf || bIcon.gameObject.activeSelf || aIcon.gameObject.activeSelf){
            Debug.Log("아직 재료 준비 안됨.");
            return;
        }
        bLight_machineBttn.SetActive(false);
        dropTea = StartCoroutine(WaitAndDropTea());
    }
    IEnumerator WaitAndDropTea(){
        // b1내림 + b2올림 > b2내림 + bfcup올림 > isDropped=true
        //yield return null;
        b1_Anim.SetTrigger("Base1_GoDown");
        //b2_Anim.SetTrigger("Base2_FillUp");
        
        base2.SetActive(true);
        b2_Anim.SetTrigger("Base2_FillUp");
        yield return null;
        //Base1_GoDown 끝날때까지 기다리고 비활
        //yield return new WaitForSeconds(3f);
        b2_Anim.SetTrigger("Base2_GoDown");
        yield return new WaitForSeconds(3.5f);
        cupFB.SetActive(true);
        fb_Anim.SetTrigger("CupFB_FillUp");
        
        
        
        //Base2_GoDown 끝나면 비활
        yield return new WaitForSeconds(2f);
        isDropped = true;
        bLight_AddObj.SetActive(true);
        //base2.SetActive(false);
        //add click active ,
    }

    IEnumerator WaitForAnimToEnd(Animator anim, string stateName){
        //애니메이션 시작할 때까지 대기
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;
        //재생중일 동안 대기
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f){
            yield return null;
        }
    }

    public void OnClickAddObj(){
        if (!isDropped){
            return;
        }
        bLight_AddObj.SetActive(false);
        cupA.SetActive(true);
        addObj.SetActive(false);
    }
    // Set ObjBttns ==========================
    public void OnClickDoneBttn(){
        if (!cupA.activeSelf || !cupFB.activeSelf){
            Debug.Log("아직 음료 완성 안됨.");
            return;
        }
        SceneChanger.instance?.ChangeScene("Scene_Ttime");
    }
}