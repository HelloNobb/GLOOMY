using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIBLinker : MonoBehaviour
{
    private float speed = 5f;
    private float minAlpha = 0.2f;
    private float maxAlpha=  1f;

    private float time;
    private Graphic uiGraphic; //img, txt 등
    private TMP_Text tmpTxt; //tmp_Text

	void Start()
	{
		uiGraphic = GetComponent<Graphic>();
        tmpTxt = GetComponent<TMP_Text>();
	}
	void Update()
	{
		time+= Time.deltaTime * speed;
        float t = (Mathf.Sin(time)+1f)/2f;
        float alphaValue = Mathf.Lerp(minAlpha,maxAlpha,t);

        if (uiGraphic != null){
            Color c = uiGraphic.color;
            c.a = alphaValue;
            uiGraphic.color = c;
        }
        else if (tmpTxt != null){
            Color c = tmpTxt.color;
            c.a = alphaValue;
            tmpTxt.color = c;
        }
	}
}
