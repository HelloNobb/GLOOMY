using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    private string currentScene;
    
    public void OnClickSceneBttn(string targetScene){
        if (SceneChanger.instance == null){
            Debug.Log("Scene Changer absent!");
            return;
        }
        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == targetScene){
            Debug.Log("이미 해당 씬에 있음!");
            return;
        }
        //Save Data
        if (currentScene == "Scene_Main"){
            GloomyManager.instance?.SaveCurrentGloomy();
        } else if (currentScene == "Scene_Garden"){
            GardenManager.instance?.SaveGardenData();
        } else if (currentScene == "Scene_Lab"){
            //
        }
        //Change Scene
        SceneChanger.instance.ChangeScene(targetScene);
    }
}