using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBombController : MonoBehaviour
{
    public Transform bombLocation;
    float bombLocationY = 20;

    public ParticleSystem explosion;

    public LayerMask fire;

    void Start()
    {
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
        yield return new WaitForSeconds(3f);
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
