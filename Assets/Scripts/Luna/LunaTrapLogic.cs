using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LunaTrapLogic : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Material mat;
    private Color originalColor;
    private float timer = 0f;

    private float stun = 1.5f;

    public MeshRenderer invis;
    public GameObject lunaObject;
    public LayerMask affectedLayers;
    private Vector3 boxSize = new Vector3 (0.1f, 1, 0.1f);

    void Start()
    {
        GeneralPlayerController skill = lunaObject.GetComponent<GeneralPlayerController>();
        Upgrade(skill.signatureSkill);

        Renderer renderer = GetComponent<Renderer>();
        mat = renderer.material; // Use .material to get an instance
        originalColor = mat.color;
    }

    public void Upgrade(float level)
    {
        if (level < 3)     // 2
            return;
        else if (level < 4)     // 3
        {
            stun = 2f;
        }
        else                    // 4 +
        {
            stun = 2.5f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0f, timer / fadeDuration);
        mat.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= fadeDuration)
            invis.enabled = false;
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize, quaternion.identity, affectedLayers);
        HashSet<GameObject> stunnedObjects = new HashSet<GameObject>(); // uses hash to avoid double calling

        foreach (Collider col in colliders)
        {
            GameObject obj = col.gameObject;

            if (col.gameObject == lunaObject) continue;

            if (!stunnedObjects.Contains(obj))
            {
                stunnedObjects.Add(obj);
                GeneralPlayerController enemy = obj.GetComponent<GeneralPlayerController>();

                if (enemy != null)
                {
                    enemy.PlayerStun(stun);
                    Destroy(gameObject);
                }
            }

            //CrateLogic fire = col.gameObject.GetComponent<CrateLogic>();
            if (col.gameObject.CompareTag("Explosion"))
            {
                Destroy(gameObject);
            }
        }
    }
}
