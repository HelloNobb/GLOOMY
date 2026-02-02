using UnityEngine;

public class OnClickTear : MonoBehaviour
{
    public bool isLeft;

    void OnMouseDown(){ //mobile- touch
        TearManager.instance?.CollectTear(isLeft);
    }
}