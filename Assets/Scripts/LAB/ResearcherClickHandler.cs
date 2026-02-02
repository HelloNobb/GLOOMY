using UnityEngine;
using UnityEngine.EventSystems;

public class ResearcherClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData){

        LabManager.instance.OnResearcherClicked();
    }
}