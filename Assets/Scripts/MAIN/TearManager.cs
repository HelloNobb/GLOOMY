using System;
using System.Collections;
using UnityEngine;

public class TearManager : MonoBehaviour
{
    public static TearManager instance;
    // tear objs
    [Header("GD_Tears")]
    public GameObject tearLeftD;
    public GameObject tearRightD;
    
    [Header("GL_Tears")]
    public GameObject tearLeftL;
    public GameObject tearRightL;
    //current
    private GameObject tearLeft;
    private GameObject tearRight;
    private Animator leftAnimator;
    private Animator rightAnimator;
    private Coroutine cry;
    // G Control
    [Header("G")]
    public GameObject gD;
    public GameObject gL;

    [Range(0,100)]
    private float blueLevel = GloomyManager.instance.currentGloomy.blue;
    private bool isTearDropping = false;
    public bool isTearDrugActive{
        get {
            var g = GloomyManager.instance.currentGloomy;
            if (!g.isTearDrugActive) return false;

            TimeSpan span = DateTime.Now - g.tearDrugStartTime;

            if (span.TotalSeconds > 60){
                g.isTearDrugActive = false;
                GloomyManager.instance.SaveCurrentGloomy();
                return false;
            }
            return true;
        }
    }
    public bool isGSick = false;

	void Awake(){
		if (instance == null) instance = this;
        else Destroy(gameObject);

        //애니메이터 컴포넌트 불러오기 (일단 기본으로 초기화)
        leftAnimator = tearLeftD.GetComponent<Animator>();
        rightAnimator = tearRightD.GetComponent<Animator>();
	}
	void Start() //애니메이터 컴포 2개 불러오기, 루프코루틴 실행
    {
        if (GloomyManager.instance == null) return;
        GloomyData g = GloomyManager.instance.currentGloomy;
        if (gD.activeSelf){
            tearLeft = tearLeftD;
            tearRight = tearRightD;
            leftAnimator = tearLeft.GetComponent<Animator>();
            rightAnimator = tearRight.GetComponent<Animator>();

            tearLeft.SetActive(true);
            tearRight.SetActive(true);
        } 
        else if (gL.activeSelf){
            tearLeft = tearLeftL;
            tearRight = tearRightL;
            leftAnimator = tearLeft.GetComponent<Animator>();
            rightAnimator = tearRight.GetComponent<Animator>();

            tearLeftL.SetActive(true);
            tearRightL.SetActive(true);
        }
    }
    public void CryG(){
        if (cry != null) StopCoroutine(cry);
        cry = StartCoroutine(TearDropLoop());
    }
    public void StopCryG(){
        tearLeft.SetActive(false);
        tearRight.SetActive(false);
        if (cry != null) StopCoroutine(cry);
    }

    // Tear Settings ==============================================
    IEnumerator TearDropLoop(){ //눈물 흘리기 루프
        float interval = GetIntervalOfTears(blueLevel); //눈물 흘리는 간격
        while (true) {
            if (!isTearDropping){
                
                yield return new WaitForSeconds(GetIntervalOfTears(blueLevel));
                StartCoroutine(PlayTearAnimation());
            }
            yield return null; //실행시키면 지연없이 바로 루프 시작
        }   
    }
    
    public float GetIntervalOfTears(float level)  //눈물 간격 시간 반환 (기준: 우울도)
    {
        float interval;
        // 우울도에 따라 간격 계산
        if (level <= 25.0f) interval = 15f;
        else if (level <=50.0f) interval = 10f;
        else if (level <= 75.0f) interval = 6f;
        else if (level <= 99.0f) interval = 3f;
        else{
            Debug.Log("우울도 설정 잘못됨.");
            interval = 100f;
        }
        //눈물약 투여여부에 따라
        return isTearDrugActive ? interval*0.5f : interval;
    }

    IEnumerator PlayTearAnimation(){
        isTearDropping = true;
        //왼쪽눈물 / 오른쪽눈물 정하기 (5:5확률)
        bool playLeft = UnityEngine.Random.value < 0.5f; //[0.0 , 0.5):left / [0.5 , 1.0):right
        GameObject targetTear = playLeft ? tearLeft : tearRight;
        Animator targetAni = playLeft ? leftAnimator : rightAnimator;
        //해당 오브젝트, 애니메이션 활성화
        targetTear.SetActive(true);
        targetAni.SetTrigger("DropTear");
        //애니메이션 길이만큼 기다렸다 실행반복
        yield return new WaitForSeconds(1.4f);

        isTearDropping = false;
        targetTear.SetActive(false);
    }

    public void CollectTear(bool isLeft) {
        GameObject target = isLeft ? tearLeft : tearRight;
        if (!target.activeInHierarchy) return; //없는 오브젝트면 무시
        
        target.SetActive(false);
        //collecteadTearCount++;
        PlayerManager.instance.AddTears(1);
        Debug.Log("눈물 수집 완료");
        isTearDropping = false;
    }

    
}