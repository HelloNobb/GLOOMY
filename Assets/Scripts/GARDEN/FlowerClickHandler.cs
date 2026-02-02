using UnityEngine;
using UnityEngine.EventSystems;

public class FlowerClickHandler : MonoBehaviour, IPointerClickHandler
{
    public int fIndex;
    public void OnPointerClick(PointerEventData eventData){
        
        if(fIndex < 0 || fIndex >= GardenManager.instance.potObj.Length){
            Debug.Log("잘못된 꽃인덱스");
            return;
        } 

        if (GardenManager.instance.flowerObj[fIndex].activeSelf){
            GardenManager.instance.OnGrownFlowerDoubleClicked(fIndex);
        }
    }
}