
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BGLooper : MonoBehaviour
{
    private float speed=0.5f; //초속
    //배경 초기위치
    private Vector3 startPos;
    private Vector3 donePos;
    private Vector3 respawnPos;

    public Transform BG1;
    public Transform BG2;

    private float imgWidth;
    private float img2Width;
    public bool isBG1;

	void Start(){
        SpriteRenderer sr1 = BG1.GetComponent<SpriteRenderer>();
        SpriteRenderer sr2 = BG2.GetComponent<SpriteRenderer>();
        imgWidth = sr1.bounds.size.x;//이미지1 너비
        img2Width = sr2.bounds.size.x;//이미지2 너비
        if (isBG1){
            startPos = new Vector3(imgWidth/2,0,0);
            donePos = new Vector3(-imgWidth+1f,0,0);
            respawnPos = new Vector3(BG2.position.x + img2Width/2 + imgWidth/2,0,0);
        } else {
            startPos = new Vector3(imgWidth + img2Width/2 -0.2f, 0, 0);
            donePos = new Vector3(-img2Width+1f,0,0);
            respawnPos = new Vector3(BG1.position.x + imgWidth/2 + img2Width/2,0,0);
        }
        transform.position = startPos;
	}
	void Update() {
        //왼쪽 방향으로 이동
		transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (isBG1){
            respawnPos = new Vector3(BG2.position.x + img2Width/2 + imgWidth/2,0,0);
        }else {
            respawnPos = new Vector3(BG1.position.x + imgWidth/2 + img2Width/2,0,0);
        }

        if (transform.position.x <= donePos.x){
            transform.position = respawnPos;
        }
	}
}
