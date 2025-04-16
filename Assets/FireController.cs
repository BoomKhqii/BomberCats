using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class FireController : MonoBehaviour
{
    private Transform fire;
    public LayerMask playerLayer;

    void Start()
    {
        fire = GetComponent<Transform>();
        StartCoroutine(waiter());
    }

    void DidKill()
    {
        
        //Collider[] hits = Physics.OverlapBox(fire.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, playerLayer);
        Collider hit = Physics.OverlapBox(fire.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, playerLayer).FirstOrDefault();
        if (hit != null && hit.CompareTag("Player"))
        {

            PlayerController playerController = hit.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.PlayerStatusUpdate(false); // Call to update player status to false (dead)
            }
            else
            {
                Debug.LogWarning("No PlayerController found on: " + hit.name);
            }
        }
        /*
        foreach (Collider col in hits)
        {
            Debug.Log("lots of times");

            if (col.CompareTag("Player"))
            {
                Debug.Log("Caught");

                PlayerController playerController = col.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.PlayerStatusUpdate(false);
                }
                else
                {
                    Debug.LogWarning("No PlayerController found on: " + col.name);
                }
            }
        }
        */
    }

    /*
    public void Killed(bool willPlayerBeAlive)
    {
        Debug.Log("During Player Status: " + willPlayerBeAlive);

        Collider[] hits = Physics.OverlapBox(fire.position, new Vector3(0.5f, 0.5f, 0.5f));
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                player.PlayerStatusUpdate(willPlayerBeAlive = false);
            }
        }
        player.PlayerStatusUpdate(willPlayerBeAlive = true);
    }
    */

    IEnumerator waiter()
    {
        //Debug.Log(player.isPlayerAlive);
        DidKill();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
