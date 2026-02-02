using UnityEngine;
using UnityEngine.EventSystems;

public class MedicineClickHandler : MonoBehaviour
{
	public MedicineData mediData;
    public string GetMediId(){
        return mediData.id;
    }
    public void OnMediClicked(){
        LabManager.instance.OnMedicineClicked(mediData.id);
    }
}