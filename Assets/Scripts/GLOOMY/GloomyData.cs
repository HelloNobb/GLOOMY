using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable] //직렬화
public class GloomyData
{
    //글루미 영구 데이터
    public int gloomyID; //"GLOOMY #OOO"의 3자리 코드
    public string metDateStr; //분양 온 날
    public DateTime metDate {
        get {
            //저장된 시각이 로컬이든 utc이든 자동으로 시간대 인식하고 보정됨
            return DateTime.Parse(metDateStr, null, System.Globalization.DateTimeStyles.RoundtripKind);
        } set {
            metDateStr = value.ToString("o");
        }
    }
    //글루미의 상태 데이터
    public int battery; //0~3
    public bool health; //t-normal / f-sick
    public float blue; //%.0f
    public float intimacy; //%.0f
    public int hygiene; //%
    public int sociability; //%
    public bool mood;
    public string bestFriend;
    //public Dictionary<string, int> interests = new();
    //public string favFriend; //제일 친한 친구
    
    //비공개 데이터
    public DateTime lastBathTime;
    public DateTime lastHygieneUpdateTime;
    public DateTime lastChatTime;
    public int injectionCount;
    public DateTime tearDrugStartTime;//
    public bool isTearDrugActive;//
    public int healCount;
    public int clearedDeepTalk;
}