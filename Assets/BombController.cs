using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject playerLocation;
    public int playerLocationX;
    public int playerLocationZ;
    float playerLocationY = 0.9160001f;
    public int range = 2;

    public MeshRenderer bomb;
    public SphereCollider body;
    public ParticleSystem explosion;

    public LayerMask unbreakable;
    public LayerMask fire;
    public LayerMask player;
    public bool[] explosionDirection = { true, true, true, true }; // N, S, W, E

    void Start()
    {
        bomb = GetComponent<MeshRenderer>();
        body = GetComponent<SphereCollider>();
        playerLocation = GameObject.FindWithTag("Player");
        StartCoroutine(waiter());
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position, 0.1f, fire))
        {
            StopCoroutine(waiter());
            //enableBombAndBody();
            Explode(range, playerLocationX, playerLocationZ, playerLocationY);
            Destroy(gameObject);
        }
    }

    IEnumerator waiter()
    {
        playerLocationX = Mathf.RoundToInt(playerLocation.gameObject.transform.position.x);
        playerLocationZ = Mathf.RoundToInt(playerLocation.gameObject.transform.position.z);
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(0.2f);
        // enableBombAndBody();
        Explode(range, playerLocationX, playerLocationZ, playerLocationY);
        Destroy(gameObject);
    }

    void Explode(int range, int x, int z, float y)
    {
        Vector3 explosionOrigin;
        Vector3[] directions = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        for (int i = 0; i < range; i++)
        {

            // North
            explosionOrigin = new Vector3(x,y,z+i);
            if (explosionDirection[0])
            {
                Instantiate(explosion, explosionOrigin, Quaternion.identity);
                raycastExplosion(explosionOrigin, Vector3.forward, 0);
            }

            // South
            explosionOrigin = new Vector3(x, y, z-i);
            if (explosionDirection[1])
            {
                Instantiate(explosion, explosionOrigin, Quaternion.identity);
                raycastExplosion(explosionOrigin, Vector3.back, 1);
            }

            // West
            explosionOrigin = new Vector3(x-i, y, z);
            if (explosionDirection[2])
            {
                Instantiate(explosion, explosionOrigin, Quaternion.identity);
                raycastExplosion(explosionOrigin, Vector3.left, 2);
            }

            // East
            explosionOrigin = new Vector3(x+i, y, z);
            if (explosionDirection[3])
            {
                Instantiate(explosion, explosionOrigin, Quaternion.identity);
                raycastExplosion(explosionOrigin, Vector3.right, 3);
            }
            
        }
    }

    bool raycastExplosion(Vector3 origin, Vector3 direction, int dirIndex)
    {
        // Dynamic
        if (Physics.Raycast(origin, direction, 1f, unbreakable))
            return explosionDirection[dirIndex] = false;

        return explosionDirection[dirIndex];
    }
    /*
    void enableBombAndBody()
    {
        bomb.enabled = !enabled;
        body.enabled = !enabled;
    }
    */
}
