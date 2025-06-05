using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashLogic : MonoBehaviour
{
    public GameObject bite;
    public GameObject lunaObject;

    private float radius = 2f;
    private

    void Start()
    {
        Explode(3, transform.position.x, transform.position.z, 0.9160001f);
    }

    public void Upgrade(float level)
    {
        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {
            radius = 2.5f;
        }
        else                    // 3
        {
            radius = 3f;
        }
    }

    void Explode(float range, float x, float z, float y)
    {
        Vector3 explosionCenter = new Vector3(Mathf.Round(x), y, Mathf.Round(z));
        Collider[] hits = Physics.OverlapSphere(explosionCenter, range);

        HashSet<Vector3> explosionPositions = new HashSet<Vector3>();

        foreach (Collider hit in hits)
        {
            Vector3 rawPos = hit.transform.position;

            // Snap to grid
            Vector3 firePos = new Vector3(
                Mathf.Round(rawPos.x),
                y,
                Mathf.Round(rawPos.z)
            );

            if (!explosionPositions.Contains(firePos))
            {
                explosionPositions.Add(firePos);

                // Use distance from center to apply delay
                float distance = Vector3.Distance(firePos, explosionCenter);
                float delay = distance * 1f; // Change multiplier to adjust wave speed

                StartCoroutine(WaveSplash(firePos, delay));
            }
        }
    }

    IEnumerator WaveSplash(Vector3 firePos, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject spawnedBite = Instantiate(bite, firePos, Quaternion.identity);
        spawnedBite.GetComponent<BiteLogic>().lunaObject = this.lunaObject;
    }
}
