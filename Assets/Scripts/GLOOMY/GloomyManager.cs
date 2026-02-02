using System;
using UnityEngine;

public class GloomyManager : MonoBehaviour
{
    public static GloomyManager instance;
    public GloomyData currentGloomy; //지금글루미객체
    //불러다 쓰는법: GloomyManager.instance.currentGloomy.(스탯명);
    public enum BlueState { //수치 유동적으로 정할 수 있도록 enum
        BlueLow, //0~10%
        BlueMidLow, //10~50%
        BlueMidHigh,//50~90%
        BlueHigh //90~100%
    }
    public enum IntiState{
        IntiLow, // 0~10%
        IntiMidLow, //10~50%
        IntiMidHigh, // 50~90%
        IntiHigh //90~100%
    }
    public enum GState{
        Normal, Abnormal
        //sick-> chat,shot 제외 금지 / shot>health약 투여시||아픈시간부터3일후 풀림
        //upset-> chat,shot 제외 금지 / 말걸기 10회시||행복약투여시 풀림
        //dirty-> bath,chat 제외 금지 / 씻기면 풀림
    }
    public BlueState blueState;
    public IntiState intiState;
    public GState gState;
    
    //이벤트 상황 (normal)
    public bool IsSick() => currentGloomy != null && !currentGloomy.health;
    public bool IsDirty() => currentGloomy != null && currentGloomy.hygiene < 25;
    public bool IsUpset() => currentGloomy != null && (DateTime.Now - currentGloomy.lastChatTime).Days >= 3 && currentGloomy.metDate.Date != DateTime.Now.Date;
    //public bool IsDischarged() => currentGloomy.battery <= 0;
    //다른 스크립트에서 상태확인용
    public GState GetGState() => gState;
    public BlueState GetBlueState() => blueState;
    public IntiState GetIntiState() => intiState;

    //싱글톤 설정
    public void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else 
            Destroy(gameObject);
    }
    //글루미 로드 (키O -> 현글루미불러오기 / 키X -> 현글루미 null (인트로로이동))
    public void LoadGloomy()
    {
        Debug.Log("GloomyManager: LoadGloomy() 호출됨");
        //현재 글루미키O -> 지금글루미 로드 / 키X -> 지금글루미:null
        if (PlayerPrefs.HasKey("CURRENT_GLOOMY_KEY")){
            int id = PlayerPrefs.GetInt("CURRENT_GLOOMY_KEY");
            currentGloomy = SaveSystem.LoadFromFile<GloomyData>($"GLOOMY_{id}");
        } else{
            currentGloomy = null;
        }
        if (currentGloomy.lastHygieneUpdateTime < currentGloomy.metDate){
            currentGloomy.lastHygieneUpdateTime = DateTime.Now;
        }
        UpdateGHygieneByTime();
        CheckGStates();
    }

    public void AssignNewGloomy()
    {
        currentGloomy = new GloomyData();
        currentGloomy.gloomyID = UnityEngine.Random.Range(100, 999);
        PlayerPrefs.SetInt("CURRENT_GLOOMY_KEY", currentGloomy.gloomyID);

        currentGloomy.metDateStr = System.DateTime.Now.ToString("o");//ISO 8601형식
        currentGloomy.battery = 3;
        currentGloomy.health = true;
        currentGloomy.blue = 90.00f;
        currentGloomy.intimacy = 0.00f;
        currentGloomy.hygiene = 60;
        currentGloomy.sociability = 0;
        currentGloomy.mood = true;
        currentGloomy.bestFriend = "없음";

        currentGloomy.lastBathTime = DateTime.Now;
        currentGloomy.lastHygieneUpdateTime = DateTime.Now;
        currentGloomy.lastChatTime = DateTime.Now;

        currentGloomy.injectionCount = 0;
        currentGloomy.healCount = 0;
        currentGloomy.clearedDeepTalk = 0;
        currentGloomy.isTearDrugActive = false;

        SaveCurrentGloomy();
    }
    public void SaveCurrentGloomy()
    {
        if (currentGloomy != null){
            SaveSystem.SaveToFile($"GLOOMY_{currentGloomy.gloomyID}", currentGloomy);
        }
    }

    public void DeleteCurrentGloomy()
    {
        if (currentGloomy != null){
            currentGloomy = null;
            PlayerPrefs.DeleteKey("CURRENT_GLOOMY_KEY");
        }
    }
    public void RemoveCurrentGloomy(bool isGivenUp) //폐기시
    {
        if (currentGloomy == null){
            Debug.Log("현 글루미가 이미 null이라 삭제불가");
            return;
        }
        //교환처리 :: 남길 데이터 없이 싹 삭제
        if (isGivenUp){
            SaveSystem.DeleteFile($"GLOOMY_{currentGloomy.gloomyID}");
        }
        PlayerPrefs.DeleteKey("CURRENT_GLOOMY_KEY");
        currentGloomy = null;
    }
    // check&Set States ===================================================
    public void CheckGStates(){
        if (currentGloomy == null) return;
        
        UpdateGHygieneByTime(); //new
        // health setting ======================
        int drugCount = GetGInjectionCount();
        int healCount = GetGHealCount();
        if (drugCount % 3 == 0 && drugCount > 0){
            SetGHealth(false);
            if (drugCount/3 == healCount){
                SetGHealth(true);
            } else {
                SetGHealth(false);
            }
        } else{
            SetGHealth(true);
        }
        // GStates Check ====================
        SetBlueState();
        SetIntimacyState();
        SetGState();//normal/abnormal(sick/dirty/upset)
    }
    
    public void SetBlueState(){ //우울도 구간 체크
        if (currentGloomy.blue >= 90.00f){ //90이상 100이하
            blueState = BlueState.BlueHigh;
            
            if (currentGloomy.blue >= 100f){
                GameManager.Instance.ChangeGameState(GameManager.GameState.GloomyMeltRequired);
                return;
            }
        } else if (currentGloomy.blue >= 50.00f){
            blueState = BlueState.BlueMidHigh;
        } else if (currentGloomy.blue >= 10.00f){
            blueState = BlueState.BlueMidLow;
        } else{  //10미만 0이상
            blueState = BlueState.BlueLow;
            
            if (currentGloomy.blue <= 0f){
                GameManager.Instance.ChangeGameState(GameManager.GameState.GloomyFixRequired);
                return;
            }
        }
    }
    public void SetIntimacyState(){ //친밀도 구간 체크
        if (currentGloomy.intimacy > 90.00f){
            intiState = IntiState.IntiHigh;
        } else if (currentGloomy.intimacy > 50.00f){
            intiState = IntiState.IntiMidHigh;
        } else if (currentGloomy.intimacy > 10.00f){
            intiState = IntiState.IntiMidLow;
        } else {
            intiState = IntiState.IntiLow;
        }
    }
    public void SetGState(){
        if (IsSick() || IsDirty()){
            gState = GState.Abnormal;
        } else {
            gState = GState.Normal;
        }
    }

    // G Activities ==============================================
    public void ChatGloomy() //말걸때마다 호출
    {
        SetGBlue(-0.01f);
        SetGInti(+0.01f);
        SetGLastChatTime();
    }
    public void BathGloomy() //목욕 완료시 호출
    {
        SetGHygiene(+50);
        currentGloomy.lastBathTime = DateTime.Now;
        currentGloomy.lastHygieneUpdateTime = DateTime.Now;
        UpdateGHygieneByTime();
        //situation = "bathDone";
    }
    public void UpdateGHygieneByTime(){ //위생: lastBathTime기준 매시간 4%씩 감소
        if (currentGloomy == null) return;

        DateTime lastBathTime = currentGloomy.lastBathTime;
        DateTime now = DateTime.Now;
        
        //calculate timespan
        TimeSpan timePassed = now - lastBathTime;
        int hoursPassed = Mathf.FloorToInt((float)timePassed.TotalHours);//내림으로계산
        if (hoursPassed <= 0) return;
        //-2 per hour
        int hygieneLoss = hoursPassed*2;
        int newHygiene = currentGloomy.hygiene - hygieneLoss;

        currentGloomy.hygiene = Mathf.Clamp(newHygiene, 0, 100);
        SaveCurrentGloomy();
    }
    public void CancelBathGloomy(){
        //situation = "bathCanceled";
    }
    public void ShotGloomy(string drugId)
    {
        if (currentGloomy == null) return;

        switch(drugId){
            case "blue":
                SetGBlue(+5);
                break;
            case "happy":
                SetGBlue(-5);
                break;
            case "tear":
                currentGloomy.isTearDrugActive = true;
                currentGloomy.tearDrugStartTime = DateTime.Now;
                SaveCurrentGloomy();
                break;
            case "battery":
                SetGBattery(+3);
                break;
            case "health":
                SetGHealth(true);
                SetGHealCount();
                break;
            default:
                break;
        }
    }
    
    public void WalkStartGloomy()
    {
        if (currentGloomy.battery <= 0){
            Debug.Log("글루미 배터리 부족");
            return;
        }
        SetGBattery(-1);
    }
    public void WalkDoneGloomy(){
        SetGBlue(-1);
    }
    public void WalkCanceledGloomy(){
    }
    public void TeaDoneGloomy(){
    }
    public void TeaCanceledGloomy(){
    }

    // Set G stats settings ==============================================
    public void SetGBlue(float setValue){
        if (currentGloomy == null) return;

        currentGloomy.blue += setValue;
        //100이상 / 0이하 setting
        if (currentGloomy.blue >= 100f) currentGloomy.blue = 100f;
        if (currentGloomy.blue <= 0f) currentGloomy.blue = 0f;

        BaseUIManager.instance?.UpdateGloomyBlue(currentGloomy.blue);
        SaveCurrentGloomy();
    }
    public float GetGBlue(){
        return currentGloomy.blue;
    }
    public void SetGInti(float setValue){
        if (currentGloomy == null) return;
        currentGloomy.intimacy += setValue;

        if (currentGloomy.intimacy >= 100f) currentGloomy.intimacy = 100f;
        if (currentGloomy.intimacy <= 0f) currentGloomy.intimacy = 0f;

        BaseUIManager.instance?.UpdateGloomyInti(currentGloomy.intimacy);
        SaveCurrentGloomy();
    }
    public float GetGInti(){
        return currentGloomy.intimacy;
    }
    public void SetGHealth(bool isHealthy){
        if (currentGloomy == null) return;
        currentGloomy.health = isHealthy;
        SaveCurrentGloomy();
    }
    public bool GetGHealth(){
        
        return currentGloomy.health;
    }
    public void SetGBattery(int setValue){
        if (currentGloomy == null) return;

        currentGloomy.battery += setValue;
        if (currentGloomy.battery >= 3) currentGloomy.battery = 3;
        if (currentGloomy.battery <= 0) currentGloomy.battery = 0;
        SaveCurrentGloomy();
    }
    public int GetGBattery(){
        return currentGloomy.battery;
    }

    public void SetGHygiene(int value){
        int newValue = currentGloomy.hygiene + value;
        currentGloomy.hygiene = Mathf.Clamp(newValue, 0, 100);
        SaveCurrentGloomy();
        //CheckGStates();
    }
    public int GetGHygiene(){
        return currentGloomy.hygiene;
    }
    public void SetGSociality(int setValue){
        if (currentGloomy == null) return;

        currentGloomy.sociability += setValue;

        if (currentGloomy.sociability >= 100) currentGloomy.sociability = 100;
        if (currentGloomy.sociability <= 0) currentGloomy.sociability = 0;
        SaveCurrentGloomy();
    }
    public int GetGSociality(){
        return currentGloomy.sociability;
    }

    public void SetGMood(bool isNormal){
        if (currentGloomy == null) return;

        currentGloomy.mood = isNormal;
        SaveCurrentGloomy();
    }
    public bool GetGMood(){
        return currentGloomy.mood;
    }
    public void SetGBestie(string fName){
        if (currentGloomy == null) return;

        if (currentGloomy.bestFriend == null){
            currentGloomy.bestFriend = "없음";
        } else {
            currentGloomy.bestFriend = fName;
        }
        SaveCurrentGloomy();
    }
    public string GetGBestie(){
        return currentGloomy.bestFriend ?? "없음";
    }
    // Other G settings ==========================================
    public void SetGLastBathTime(){
        if (currentGloomy == null) return;

        currentGloomy.lastBathTime = DateTime.Now;
        SaveCurrentGloomy();
    }
    public void SetGLastChatTime(){
        if (currentGloomy == null) return;

        currentGloomy.lastChatTime = DateTime.Now;
        SaveCurrentGloomy();
    }
    public DateTime GetGLastChatTime(){
        return currentGloomy.lastChatTime;
    }
    public void SetGInjectionCount(){
        if (currentGloomy == null) return;

        currentGloomy.injectionCount++;
        SaveCurrentGloomy();
    }
    public int GetGInjectionCount(){
        return currentGloomy.injectionCount;
    }

    public void SetGClearedDeepTalkCount(){
        if (currentGloomy == null) return;

        if (currentGloomy.clearedDeepTalk >= 10){
            Debug.Log("딥토크 이미 10개 완료");
        } else{
            currentGloomy.clearedDeepTalk += 1;
            SaveCurrentGloomy();
        }
    }
    public void SetGHealCount(){
        if (currentGloomy == null) return;

        currentGloomy.healCount++;
        SaveCurrentGloomy();
    }
    public int GetGHealCount(){
        return currentGloomy.healCount;
    }
}   

