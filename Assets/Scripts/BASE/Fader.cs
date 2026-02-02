using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Fader
{
    public static IEnumerator FadeSprite(SpriteRenderer renderer, float targetAlpha, float duration){
        
        Color c = renderer.color;
        float start = c.a;
        float t = 0;

        while (t < 1f){
            t += Time.deltaTime / duration;
            float a = Mathf.Lerp(start, targetAlpha, t);
            renderer.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        renderer.color = new Color(c.r, c.g, c.b, targetAlpha);
    }
}
