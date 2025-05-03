using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaTrapLogic : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Material mat;
    private Color originalColor;
    private float timer = 0f;

    public MeshRenderer invis;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        mat = renderer.material; // Use .material to get an instance
        originalColor = mat.color;
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0f, timer / fadeDuration);
        mat.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= fadeDuration)
            invis.enabled = false;
    }
}
