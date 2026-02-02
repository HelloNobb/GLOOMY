using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlowerCard : MonoBehaviour
{
    public Image flowerImg;
    public TMP_Text flowerName;
    public TMP_Text flowerScent;
    public TMP_Text flowerGrowTime;
    public Button selectBttn;
    public Button closeBttn;

    private System.Action onConfirm;

    private float growthSec;

    public void Show(FlowerData flower, System.Action confirmCallback){

        flowerImg.sprite = flower.flowerIcon;
        flowerName.text = flower.flowerName;
        flowerScent.text = flower.description;
        
        growthSec = flower.growthTimeSeconds;
        if (growthSec > 1800){
            flowerGrowTime.text = $"{growthSec / 1800}시간";
        } else if (growthSec >= 60){
            flowerGrowTime.text = $"{growthSec / 60}분";
        } else
            flowerGrowTime.text = $"{growthSec}초";

        onConfirm = confirmCallback;
        gameObject.SetActive(true);
    }

    void Start()
    {
        selectBttn.onClick.AddListener(()=>{
            onConfirm?.Invoke(); //action call-back
            gameObject.SetActive(false);
        });
        closeBttn.onClick.AddListener(()=>{
            gameObject.SetActive(false);
        });
    }
}