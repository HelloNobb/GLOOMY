using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonSetting : MonoBehaviour
{
    public static SingletonSetting instance;
    void Awake()
    {   
        //Singleton Pattern
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}