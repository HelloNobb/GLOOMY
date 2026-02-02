using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PlayerData playerData;

    public string playerPosition="";//직급
    public int[] goldPerTears= {1,2,5,10,20};
    public int playerGoldRate=0;

    public enum PlayerLv{
        lv1, lv2, lv3, lv4, lv5
    }
    public PlayerLv lv = PlayerLv.lv1;
    public void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //유저 데이터 로드 (이름, 돈, 눈물, 누적눈물)
            //LoadPlayerData();
        } else {
            Destroy(gameObject);
        }
    }

    public void LoadPlayerData()
    {
        playerData = SaveSystem.LoadFromFile<PlayerData>("PLAYER_DATA");
        
        if (playerData == null){
            playerData = new PlayerData();
            playerData.playerName = "정보없음";
            playerData.collectedCash = 0;
            playerData.collectedTears = 0;
            playerData.accumCollectedTears = 0;
            playerData.hiredDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        }
        SetPlayerLevel();
    }

    public void SavePlayerData()
    {
        SaveSystem.SaveToFile<PlayerData>("PLAYER_DATA", playerData);
    }
    public void SetPlayerName(string name)
    {
        playerData.playerName = name;
        SavePlayerData();
        //UI display update code
        BaseUIManager.instance?.UpdatePlayerNameDisplay(name);
    }

    public void SetPlayerLevel(){
        string beforePos = playerPosition;
        //눈물에 따라 직급 setting
        if (playerData.accumCollectedTears >= 10000){
            playerPosition = "글루미 마스터";
            playerGoldRate = goldPerTears[4];
            lv = PlayerLv.lv5;
        } else if (playerData.accumCollectedTears >= 5000){
            playerPosition = "베테랑 관리인";
            playerGoldRate = goldPerTears[3];
            lv = PlayerLv.lv4;
        } else if (playerData.accumCollectedTears >= 2000){
            playerPosition = "쓸만한 관리인";
            playerGoldRate = goldPerTears[2];
            lv = PlayerLv.lv3;
        } else if (playerData.accumCollectedTears >= 500){
            playerPosition = "그저그런 관리인";
            playerGoldRate = goldPerTears[1];
            lv = PlayerLv.lv2;
        } else {
            playerPosition = "초보 관리인";
            playerGoldRate = goldPerTears[0];
            lv = PlayerLv.lv1; 
        }
        //level 바뀌면 할 행동
        if (playerPosition != beforePos && beforePos != ""){
            Debug.Log("player Level up!");
        }
        //UI업데이트
        BaseUIManager.instance?.UpdatePlayerLvDisplay(playerPosition);
    }
    public PlayerLv GetPlayerLv(){
        SetPlayerLevel();
        return lv;
    }
    public string GetPlayerName(){
        return playerData.playerName;
    }
    public string GetPlayerHiredDate(){
        return playerData.hiredDate;
    }
    public string GetPlayerPosition(){
        SetPlayerLevel();
        return playerPosition;
    }
    public int GetPlayerGoldRate(){
        SetPlayerLevel();
        return playerGoldRate;
    }
    public int GetPlayerAccumTears(){
        return playerData.accumCollectedTears;
    }
    public int GetPlayerOwnedTears(){
        return playerData.collectedTears;
    }
    public int GetPlayerOwnedGold(){
        return playerData.collectedCash;
    }

    public void AddGold(int amount)
    {
        if (playerData.collectedCash + amount < 0){
            Debug.Log("골드 부족으로 거절됨");
            return;
        }
        playerData.collectedCash += amount;
        SavePlayerData();
        //UI display update code
        BaseUIManager.instance?.UpdateCashDisplay(playerData.collectedCash);
    }
    public void AddTears(int amount)
    {
        if (playerData.collectedTears + amount < 0){
            Debug.Log("이상한 값 추가됨");
            return;
        }
        playerData.collectedTears += amount;
        //playerData.accumCollectedTears += amount; 
        SavePlayerData();
        //SetPlayerLevel();
        //UI display update code
        BaseUIManager.instance?.UpdateTearsDisplay(playerData.collectedTears, playerData.accumCollectedTears);
    }
    public void AddSubimitTears(int amount){
        playerData.accumCollectedTears += amount;
        SavePlayerData();
        SetPlayerLevel();
        BaseUIManager.instance?.UpdateTearsDisplay(playerData.collectedTears, playerData.accumCollectedTears);
    }
}