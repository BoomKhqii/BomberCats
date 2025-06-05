using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUltimateLogic : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private string lvl1Col = "#C6ECFF", lvl2Col = "#602776", lvl3Col = "#7F0006";

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
        Active();
    }

    public void Faded() { spriteRenderer.color = new Color(1f, 1f, 1f, 0.078f); }
    public void Active() { spriteRenderer.color = new Color(0.7764705882352941f, 0.9254901960784314f, 1f, 1f); }

    public void UIColoursStandardLevelingSystem(int lvl)
    {
        Color tempColour;
        switch (lvl)
        {
            case 1:
                if (ColorUtility.TryParseHtmlString(lvl1Col, out tempColour))
                    spriteRenderer.color = tempColour;
                break;
            case 2:
                if (ColorUtility.TryParseHtmlString(lvl2Col, out tempColour))
                    spriteRenderer.color = tempColour;
                break;
            case 3:
                if (ColorUtility.TryParseHtmlString(lvl3Col, out tempColour))
                    spriteRenderer.color = tempColour;
                break;
        }
    }
}
