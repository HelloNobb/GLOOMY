using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneBttnCtrl : MonoBehaviour
{
    public static BaseSceneBttnCtrl instance;

    void Awake()
    {
        if (instance == null){
            instance = this;
        } else
            Destroy(gameObject);
    }

    public void SetSceneBttnActive(bool isActive){
        gameObject.SetActive(isActive);
    }
}
