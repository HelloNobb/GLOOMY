using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UIFader{
	public static IEnumerator FadeImg(Image img, float targetAlpha, float duration){
		Color c  = img.color;
		float start = c.a;
		float t = 0;

		while (t < 1f){
			t += Time.deltaTime / duration;
			c.a = Mathf.Lerp(start, targetAlpha, t);
			img.color=c;
			yield return null;
		}
		img.color = new Color(c.r, c.g, c.b, targetAlpha);
		
	}
}