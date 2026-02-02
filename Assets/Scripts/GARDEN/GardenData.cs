using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GardenData 
{
    public PotData[] pots = new PotData[3];
}
[System.Serializable]
public class PotData 
{
    public bool isPlanted;
    public string flowerName;
    public string plantedTimeStr;
}