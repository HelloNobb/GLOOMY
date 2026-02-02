using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleRotator : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private RectTransform handleRect;
    private Vector2 center;//핸들의 중심좌표(회전 중심)
    private Vector2 prevDirection; //new
    public float deltaAngle;

    private float targetAngle = 0f;
    private float currentAngle =0f;
    private float followSpeed = 5f; //2~3: 뻑뻑 , 5~6: 살짝밀림 , 10이상: 거의즉시반응

	void Awake()
	{
		handleRect = GetComponent<RectTransform>();
	}

    // 드래그시작 - 터치기준 핸들의 중심위치를 로컬좌표로 계산해 저장
    public void OnBeginDrag(PointerEventData eventData){
        
        Debug.Log("드래그 시작됨");
        //회전 중심 계산 (스크린 > 로컬)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handleRect,
            eventData.position,
            eventData.pressEventCamera,
            out center
        );
        //시작 방향 계산
        Vector2 startPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handleRect,
            eventData.position,
            eventData.pressEventCamera,
            out startPos
        );

        prevDirection = (startPos - center).normalized;
        currentAngle = handleRect.rotation.eulerAngles.z; //현재 회전값 저장

        targetAngle = currentAngle;
        deltaAngle = 0f;
    }
    // 드래그중 - 회전 각도 계산해 저장 (드래그중인 손의 현재위치 -> 핸들기준 로컬좌표 계산)
    public void OnDrag(PointerEventData eventData){

        Vector2 currentPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handleRect,
            eventData.position,
            eventData.pressEventCamera,
            out currentPos
        );
        //드래그 방향 -> 회전각도로 바꾸기
        Vector2 currentDirection = (currentPos - center).normalized; //중심~마우스까지의 방향벡터
        //방향 변화량 계산 (signed: 시계/반시계 판단)
        float delta = Vector2.SignedAngle(prevDirection, currentDirection);
        //반대방향 block(반시계)
        if (delta < 0f){
            deltaAngle = 0f;
            return;
        }

        deltaAngle = delta; //누적회전값
        targetAngle += delta;
        prevDirection = currentDirection;
        //실제 회전 적용
        handleRect.rotation = Quaternion.Euler(0,0,currentAngle);
    }

	void Update()
	{
		//천천히 targetAngle 따라감
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime*followSpeed);
        handleRect.rotation = Quaternion.Euler(0f,0f,currentAngle);
	}
}
