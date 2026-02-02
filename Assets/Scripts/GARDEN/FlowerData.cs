//각 꽃의 성장시간, 설명, 이미지 등 실제 데이터 보관
using UnityEngine;

[CreateAssetMenu(fileName = "FlowerData", menuName = "Garden/Flower")] //데이터컨테이너를 에셋화
public class FlowerData : ScriptableObject  //꽃 정보 담을 수 있는 그릇
{
    public string flowerId;
    public string flowerName;
    public Sprite flowerIcon;
    public Sprite flowerImage_Adult;
    public Sprite flowerImage_Child;
    public Sprite flowerImage_Baby;
    public Sprite flowerImage_Seed;
    public float growthTimeSeconds;
    public string description;
    public string tasteInfo;
    public Sprite machineFlowerImg;
    public Sprite withWaterImg;
    public Sprite withMilkImg;
}