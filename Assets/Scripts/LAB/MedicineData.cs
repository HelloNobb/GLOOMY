using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MedicineData", menuName = "Lab/Medicine")]
public class MedicineData : ScriptableObject
{
    public string id;
    public string displayName;
    public int price;
    public string description;
    
    public Sprite mediImg;
}