using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BigExplosionController : MonoBehaviour
{
    private Transform fire;
    public LayerMask everything;

    void Start()
    {
        fire = GetComponent<Transform>();
        StartCoroutine(waiter());
    }

    void DidKill()
    {
        Collider[] hit = Physics.OverlapBox(fire.position, new Vector3(3.5f, 5f, 3.5f), Quaternion.identity, everything);
        Debug.Log(hit.Length);
        foreach (Collider col in hit)
        {
            if (col != null && col.CompareTag("Player"))
            {
                ObjectStatus objectStatus = col.GetComponent<ObjectStatus>();
                if (objectStatus != null)
                {
                    objectStatus.StatusUpdate(false); // Call to update player status to false (dead)
                }
                else
                {
                    Debug.LogWarning("No PlayerController found on: " + col.name);
                }
            }
            
            if (((1 << col.gameObject.layer) & everything) != 0)
            {
                Debug.Log("destroyed " + col.name);
                Destroy(col.gameObject);
            }
        }
    }

    IEnumerator waiter()
    {
        //Debug.Log(player.isPlayerAlive);
        DidKill();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
} 