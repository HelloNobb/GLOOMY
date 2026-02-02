using UnityEngine;
using UnityEngine.UI;

public class FlowerBttn : MonoBehaviour
{
    public Image iconImg;
    public Button bttn;
    
    private FlowerData flowerData;
    private System.Action onClick;

    public void Setup(FlowerData flower, System.Action callback){

        flowerData = flower;
        onClick = callback;

        if (flower.flowerIcon == null)
            Debug.Log("꽃아이콘 비어있음");
        if (iconImg == null)
            Debug.Log("iconImg 연결안되어있음");

        iconImg.sprite = flower.flowerIcon;
        bttn.onClick.RemoveAllListeners();
        bttn.onClick.AddListener(()=> onClick?.Invoke()); //action call-back: 
    }
}