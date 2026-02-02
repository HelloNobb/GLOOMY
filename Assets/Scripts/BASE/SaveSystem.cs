// 저장시스템 (static 클래스)
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveToFile<T>(string filename, T data) //T: 제네릭(GloomyData, GardenData등)
    {
        //데이터 저장할 경로지정, ToJson으로 json형태로 포맷팅된 문자열 생성
        string path = Path.Combine(Application.persistentDataPath, filename+".json");
        string json = JsonUtility.ToJson(data, true); //data->json문자열 변환 / true: 예쁘게 들여쓰기
        //file create&save
        File.WriteAllText(path, json);  //path위치에 json내용을 파일로 씀 (이미 파일 있으면 덮어씀/없으면 새로씀)
    }

    public static T LoadFromFile<T>(string filename)
    {
        //저장된 json파일의 경로 생성(저장할때랑 똑같이)
        string path = Path.Combine(Application.persistentDataPath, filename+".json");
        //해당 경로에 파일이 존재한다면,
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json); //json문자열->원래데이터객체로 되돌림
        } else {
            Debug.Log("해당 파일 존재 안함");
            return default; //파일없음/문제생김 -> null 또는 해당 타입 기본값 반환
        }
    }

    public static void DeleteFile(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename+".json");

        if(File.Exists(path)){
            File.Delete(path);
            Debug.Log($"[시스템]파일 삭제됨: {path}");
        } else {
            Debug.Log($"[시스템] 삭제할 파일 없음: {path}");
        }
    }
}