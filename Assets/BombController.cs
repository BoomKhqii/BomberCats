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

        bool[] explosionDirection = { true, true, true, true }; // N, S, W, E
        Instantiate(explosion);

        for (int i = 1; i <= range; i++)
        {
            // North
            if (explosionDirection[0] && !Physics.Raycast(transform.position, Vector3.forward, 1f, unbreakable)) 
                Instantiate(explosion, new Vector3(x, y, z + i), Quaternion.identity);
            else { explosionDirection[0] = false; }
            // South
            if (explosionDirection[1] && !Physics.Raycast(transform.position, Vector3.back, 1f, unbreakable))
                Instantiate(explosion, new Vector3(x, y, z - i), Quaternion.identity);
            else { explosionDirection[1] = false; }
            // West
            if (explosionDirection[2] && !Physics.Raycast(transform.position, Vector3.left, 1f, unbreakable))
                Instantiate(explosion, new Vector3(x - i, y, z), Quaternion.identity);
            else { explosionDirection[2] = false; }
            // East
            if (explosionDirection[3] && !Physics.Raycast(transform.position, Vector3.right, 1f, unbreakable))
                Instantiate(explosion, new Vector3(x + i, y, z), Quaternion.identity);
            else { explosionDirection[3] = false; }

            /*
            if(gameObject.tag != "Bedrock" && explosionN)
                Instantiate(explosion, new Vector3(x, y, z+i), Quaternion.identity);
            else
                explosionN = false; 
            
            /*
            if (gameObject.tag != "Bedrock" && explosionS)
                Instantiate(explosion, new Vector3(x, y, z-i), Quaternion.identity);
            else { explosionS = false; }
            if (gameObject.tag != "Bedrock" && explosionW)
                Instantiate(explosion, new Vector3(x-i, y, z), Quaternion.identity);
            else { explosionW = false; }
            if (gameObject.tag != "Bedrock" && explosionE)
                Instantiate(explosion, new Vector3(x+i, y, z), Quaternion.identity);
            else { explosionE = false; }
            
            Instantiate(explosion, new Vector3(x + i, y, z), Quaternion.identity);
            Instantiate(explosion, new Vector3(x - i, y, z), Quaternion.identity);
            Instantiate(explosion, new Vector3(x, y, z + i), Quaternion.identity);
            Instantiate(explosion, new Vector3(x, y, z - i), Quaternion.identity);
            */
        }
    }

    void enableBombAndBody()
    {
        bomb.enabled = !enabled;
        body.enabled = !enabled;
    }
}
