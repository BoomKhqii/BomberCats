using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class BigBombController : MonoBehaviour
{
    public Transform bombLocation;
    public ParticleSystem explosion;
    public LayerMask fire;

    public GeneralPlayerController player;
    private Rigidbody rb;
    private float time = 3f;

    public void Upgrade(float level)
    {
        rb = GetComponent<Rigidbody>();

        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {
            rb.drag = 0.5f; // Drag to slow down the bomb
            time = 2.5f;
        }
        else                // 3
        {
            rb.drag = 0f;
            time = 2f;
        }

        StartCoroutine(waiter());
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position, 1f, fire))
        {
            StopCoroutine(waiter());
            Explode();
            Destroy(gameObject);
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(time);
        yield return new WaitForSeconds(0.2f);
        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        Instantiate(explosion, new Vector3(bombLocation.position.x, 0, bombLocation.position.z), Quaternion.identity);
    }

    bool IsExplosionThere(Vector3 center, Vector3 halfExtents, string tag)
    {
        Collider[] hits = Physics.OverlapBox(center, halfExtents);
        foreach (Collider col in hits)
        {
            if (col.CompareTag(tag))
                return true;
        }
        return false;
    }
}
