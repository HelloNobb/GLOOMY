// 팝업 열기/닫기 버튼 기능 , 타깃판넬 연결
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POPUIManager : MonoBehaviour
{
    [System.Serializable]
    public class PopupItem
    {
        public Button openButton;
        public Button closeButton;
        public GameObject targetPanel;
    }

    public List<PopupItem> popupItems = new List<PopupItem>();
    public GameObject blackPanel;
    private void Start()
    {
        blackPanel.SetActive(false);
        // 팝업시리즈들 다 불러와서 하나하나 설정 (1.일단판넬비활/ 2.여는버튼에기능추가 /3. 닫는버튼기능추가)
        foreach (PopupItem item in popupItems){
            
            item.targetPanel.SetActive(false);

            item.openButton.onClick.AddListener(()=>{
                    item.targetPanel.SetActive(true);
                    blackPanel.SetActive(true);
                }
            );

            item.closeButton.onClick.AddListener(()=>{
                    item.targetPanel.SetActive(false);
                    blackPanel.SetActive(false);
                }
            );
        }
    }
}