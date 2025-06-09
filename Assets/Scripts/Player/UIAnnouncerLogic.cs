using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnnouncerLogic : MonoBehaviour
{
    public Text mes;

    public void Announce(string message)
    {
        mes.color = new Color(mes.color.r, mes.color.g, mes.color.b, 1f); // Reset to full opacity
        mes.text = message;
        StartCoroutine(Fadeout());
    }

    IEnumerator Fadeout()
    {
        float elapsedTime = 0f;
        Color startColor = mes.color, targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Full opacity

        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            mes.color = Color.Lerp(startColor, targetColor, elapsedTime / 5f);
            yield return null;
        }
    }
}
