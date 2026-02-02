using UnityEngine;

[CreateAssetMenu(fileName = "NormalItemData", menuName = "Shop/normalItem")] 
public class NormalItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemInfo;
}