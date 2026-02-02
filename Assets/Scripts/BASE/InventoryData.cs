using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public FlowerInventory flowerInv = new FlowerInventory();
    public MedicineInventory medicineInv = new MedicineInventory();
    public ItemInventory itemInv = new ItemInventory();
}
[System.Serializable]
public class FlowerInventory
{
    public int lav, rose, cham, jas, pansy, mari, berga, lotus, dand, cherry;

    public int GetFAmountById(string id){
        switch(id){
            case "lav": return lav;
            case "rose": return rose;
            case "cham": return cham;
            case "jas": return jas;
            case "pansy": return pansy;
            case "mari": return mari;
            case "berga": return berga;
            case "lotus": return lotus;
            case "dand": return dand;
            case "cherry": return cherry;
            default:
                Debug.Log("잘못된 꽃id 요청됨: "+id);
                return 0;
        }
    }
}

[System.Serializable]
public class MedicineInventory
{
    public int blue, tear, happy, health, battery, sleep;

    public int GetMAmountById(string id){
        switch (id){
            case "blue": return blue;
            case "tear": return tear;
            case "happy": return happy;
            case "health": return health;
            case "battery": return battery;
            case "sleep": return sleep;
            default:
                Debug.Log("잘못된 약id호출됨: "+id);
                return -1;
        }
    }
}

[System.Serializable]
public class ItemInventory
{
    public bool teadyBear;
}