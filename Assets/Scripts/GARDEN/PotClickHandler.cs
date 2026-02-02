using UnityEngine;
using UnityEngine.EventSystems;

public class PotClickHandler : MonoBehaviour, IPointerClickHandler
{
    public int potIndex;
    public void OnPointerClick(PointerEventData eventData){
        
        if(potIndex < 0 || potIndex >= GardenManager.instance.potObj.Length){
            Debug.Log("잘못된 팟인덱스");
            return;
        } 

        if (GardenManager.instance.flowerObj[potIndex].activeSelf){
            GardenManager.instance.OnFilledPotClicked(potIndex);
        } else{
            GardenManager.instance.OnEmptyPotClicked(potIndex);
        }
    }
}