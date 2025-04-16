using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public Transform bombLocation;
    public int bombLocationX;
    public int bombLocationZ;
    float bombLocationY = 0.9160001f;
    public int range = 2;

    public MeshRenderer bomb;
    public SphereCollider body;
    public ParticleSystem explosion;

    public LayerMask unbreakable;
    public LayerMask fire;
    public LayerMask player;
    public bool[] explosionDirection = { true, true, true, true }; // N, S, W, E


    // ghost
    private bool isPlayerInside = true;
    private GameObject spawningPlayer;
    private Collider blockCollider;

    public void SetSpawningPlayer(GameObject player)
    {
        spawningPlayer = player;
        Debug.Log("Spawning player set to: " + spawningPlayer);
    }

    void Start()
    {
        blockCollider = GetComponent<Collider>();
        //blockCollider.isTrigger = true;

        bomb = GetComponent<MeshRenderer>();
        body = GetComponent<SphereCollider>();
        StartCoroutine(waiter());
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position, 0.1f, fire))
        {
            StopCoroutine(waiter());
            Explode(range, bombLocationX, bombLocationZ, bombLocationY);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("[EXIT] by: " + other.gameObject.name);
        Debug.Log("Reference Equals: " + (other.gameObject == spawningPlayer));
        Debug.Log(other.gameObject + " and " + spawningPlayer);

        if (other.gameObject == spawningPlayer && isPlayerInside)
        {
            Debug.Log("Solid");
            isPlayerInside = false;
            blockCollider.isTrigger = false;
        }
    }

    IEnumerator waiter()
    {
        bombLocationX = Mathf.RoundToInt(bombLocation.position.x);
        bombLocationZ = Mathf.RoundToInt(bombLocation.position.z);
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(0.2f);
        Explode(range, bombLocationX, bombLocationZ, bombLocationY);
        Destroy(gameObject);
    }

    void Explode(int range, int x, int z, float y)
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

                    Instantiate(explosion, explosionOrigin, Quaternion.identity);
                    raycastExplosion(explosionOrigin, directions[j], j);
                }
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
}