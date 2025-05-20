using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISignatureLogic : MonoBehaviour
{
    public float cooldown;
    public int level;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator FadeIn(float duration)
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Full opacity

        Debug.Log($"Start Color: {startColor}");
        Debug.Log($"Target Color: {targetColor}");


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }
}
