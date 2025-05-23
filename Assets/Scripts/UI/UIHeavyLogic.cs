using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeavyLogic : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start() { spriteRenderer = GetComponent<SpriteRenderer>(); }

    public IEnumerator FadeIn(float duration)
    {
        Faded(); // faded first
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color, targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Full opacity

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }
        spriteRenderer.color = targetColor;
    }

    public void Faded() { spriteRenderer.color = new Color(1f, 1f, 1f, 0.078f); }
}
