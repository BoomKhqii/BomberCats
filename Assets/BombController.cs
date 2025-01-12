using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject playerLocation;
    public int playerLocationX;
    public int playerLocationZ; 
    public int range = 2;
    public float speed = 0.2f;

    public MeshRenderer bomb;

    public ParticleSystem explosion;

    public SphereCollider body;

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
        bomb.enabled = !enabled;
        body.enabled = !enabled;

        //Instantiate(explosion, new Vector3(playerLocationX, 0.9160001f, playerLocationZ), Quaternion.identity);
        for (int i = 1; i <= range; i++)
        {
            Instantiate(explosion, new Vector3(playerLocationX+i, 0.9160001f, playerLocationZ), Quaternion.identity);
            Instantiate(explosion, new Vector3(playerLocationX-i, 0.9160001f, playerLocationZ), Quaternion.identity);
            Instantiate(explosion, new Vector3(playerLocationX, 0.9160001f, playerLocationZ+i), Quaternion.identity);
            Instantiate(explosion, new Vector3(playerLocationX, 0.9160001f, playerLocationZ-i), Quaternion.identity);
        }
       
        //Explode(range, speed, playerLocationX, playerLocationZ);

    }
    /*
    void Explode(int range, float speed, int x, int z)
    {
        for(int i = 0; i <= range; i++)
        {
            Instantiate(explosion, new Vector3(x++, 0.9160001f, z++), Quaternion.identity);
        }
    }
    */
}
