using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        bomb = GetComponent<MeshRenderer>();
        body = GetComponent<SphereCollider>();
        playerLocation = GameObject.FindWithTag("Player");
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        playerLocationX = Mathf.RoundToInt(playerLocation.gameObject.transform.position.x);
        playerLocationZ = Mathf.RoundToInt(playerLocation.gameObject.transform.position.z);
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(0.2f);
        enableBombAndBody();
        Explode(range, playerLocationX, playerLocationZ, playerLocationY);

    }
    void Explode(int range, int x, int z, float y)
    {
        for (int i = 1; i <= range; i++)
        {
            Instantiate(explosion, new Vector3(x + i, y, z), Quaternion.identity);
            Instantiate(explosion, new Vector3(x - i, y, z), Quaternion.identity);
            Instantiate(explosion, new Vector3(x, y, z + i), Quaternion.identity);
            Instantiate(explosion, new Vector3(x, y, z - i), Quaternion.identity);
        }
    }
    void enableBombAndBody()
    {
        bomb.enabled = !enabled;
        body.enabled = !enabled;
    }
}
