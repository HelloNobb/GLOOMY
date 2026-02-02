using UnityEngine;
using UnityEngine.EventSystems;

public class CupDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform dragRect;//드래그할 ui이미지
    
    public float triggerY = -330f;
    private Vector2 startLocalPos;
    private Vector2 originalPos;
    
    void Start(){
        //dragRect.gameObject.SetActive(true);
        dragRect.anchoredPosition = new Vector2(dragRect.anchoredPosition.x, -700f);
    }
	public void OnBeginDrag(PointerEventData eventData){
        //초기위치 저장
        originalPos = dragRect.anchoredPosition;
        // 화면 상의 마우스/손가락 위치 --> ui내부에서의 좌표로 바꾸는 함수
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        dragRect.parent as RectTransform, //부모ui객체의 rectranform기준 좌표구하기
        eventData.position, //현재 마우스/터치 위치
        eventData.pressEventCamera, // ui이벤트 감지한 카메라 -일반적으로 canvas가 ScreenSpce-camera일때 필요 (overlay면 무시)
        out startLocalPos //변환된 좌표를 startLocalPos란 변수에 담겠단 의미
        );
    }
    public void OnDrag(PointerEventData eventData){//드래그하는동안 계속 호출됨
        //
        Vector2 currentLocalPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragRect.parent as RectTransform, //부모오브제 기준으로
            eventData.position, //지금 마우스/터치가 있는곳을
            eventData.pressEventCamera, //카메라 기준으로 변환해서
            out currentLocalPos //그 결과를 이 변수에 담아줘
        );

        Vector2 delta = currentLocalPos - startLocalPos;
        dragRect.anchoredPosition += new Vector2(0, delta.y); // y축만 이동

        startLocalPos = currentLocalPos;
    }
    public void OnEndDrag(PointerEventData eventData){
        if (dragRect.anchoredPosition.y >= triggerY){
            //드래그 목표치 완료 :: 애니메이션 재생
            dragRect.gameObject.SetActive(false);
            Tea3Manager.instance?.AfterDragCup();
        }
        dragRect.anchoredPosition = originalPos;
    }
}
