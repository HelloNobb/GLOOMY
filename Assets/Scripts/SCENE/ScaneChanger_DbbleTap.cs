using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScaneChanger_DbbleTap : MonoBehaviour
{
    float lastTapTime = 0.0f;
    float doubleTapTerm = 0.3f;
    public GameObject door;
    public Animator openDoor;
    private Coroutine d;

    public void OnButtonTapped() {

        float currentTime = Time.time; //게임 시작된 이후 경과된 시간
        
        if (currentTime - lastTapTime < doubleTapTerm) {
            if (d != null) StopCoroutine(d);
            d = StartCoroutine(OpenDoorAndGo());
        }
        lastTapTime = currentTime;
    }

    IEnumerator OpenDoorAndGo(){
        yield return new WaitForSeconds(0.5f);
        openDoor.SetTrigger("OpenDoor");
        yield return new WaitForSeconds(1.3f);
        door.SetActive(false);

        GameManager.Instance.ChangeSceneOfCurrentState();
    }
    
}