using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//special recipe load, give effect after tea time
public class TEAManager : MonoBehaviour
{
    public static TEAManager instance;
    //사용자가 선택한 재료
    public FlowerData selectedFlower;
    public BaseData selectedBase;
    public AddData selectedAdd;

	void Awake() {
		if (instance==null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
	}
	void Start()
	{
		BaseUIManager.instance?.SetActiveSceneBttns(false);
	}
    public void ReceiveSelectedDatas(FlowerData f, BaseData b, AddData a){
        selectedFlower = f;
        selectedBase = b;
        selectedAdd = a;
    }

    public void FinishTeaTime(){
        Destroy(gameObject);
    }
}
