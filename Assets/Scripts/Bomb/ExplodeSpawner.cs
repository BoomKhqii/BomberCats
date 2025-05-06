using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeSpawner : MonoBehaviour
{
    public Transform bombLocation;
    public int locationX;
    public int locationZ;
    private float bombLocationY = 0.9160001f;
    public float range = 3;

    public ParticleSystem explosion;

    public LayerMask unbreakable;
    public LayerMask fire;
    public LayerMask player;
    private bool[] explosionDirection = { true, true, true, true }; // N, S, W, E


    //public float skill = 0;

    public void SetSpawningPlayer(GameObject player, float upgrade)
    {
        range += upgrade;
    }

    void Start()
    {
        StartCoroutine(ExplosionSequence(range, locationX, locationZ, bombLocationY));
    }

    IEnumerator ExplosionSequence(float range, int x, int z, float y)
    {
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,  // North
            Vector3.back,     // South
            Vector3.left,     // West
            Vector3.right     // East
        };

        for (int i = 0; i < range; i++)
        {
            for (int j = 0; j < directions.Length; j++)
            {
                if (explosionDirection[j]) // Check if direction is active
                {
                    Vector3 offset = directions[j] * i;
                    Vector3 explosionOrigin = new Vector3(x, y, z) + offset;

                    if (IsExplosionThere(explosionOrigin, new Vector3(0.5f, 0.5f, 0.5f), "Explosion") == false)
                    {
                        Instantiate(explosion, explosionOrigin, Quaternion.identity);
                    }
                    raycastExplosion(explosionOrigin, directions[j], j);
                }
            }

            yield return new WaitForSeconds(0.1f); // delay *between* each wave
        }
    }
    bool raycastExplosion(Vector3 origin, Vector3 direction, int dirIndex)
    {
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, 1f))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check for breakable crate
            if (hitObject.CompareTag("Breakable"))
            {
                CrateLogic crate = hitObject.gameObject.GetComponent<CrateLogic>();

                // Snap to grid if needed
                Vector3 firePosition = new Vector3(
                    Mathf.Round(hitObject.transform.position.x),
                    origin.y,
                    Mathf.Round(hitObject.transform.position.z)
                );

                // Spawn fire BEFORE destroying the crate
                Instantiate(explosion, firePosition, Quaternion.identity);
                crate.CrateDrop();

                return explosionDirection[dirIndex] = false; // Stop fire after breaking
            }

            // If it's unbreakable, stop fire
            if (((1 << hitObject.layer) & unbreakable) != 0)
            {
                return explosionDirection[dirIndex] = false;
            }
        }

        return explosionDirection[dirIndex];
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
