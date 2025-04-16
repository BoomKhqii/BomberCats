using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Collider[] hits = Physics.OverlapBox(fire.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, playerLayer);
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                PlayerController playerController = col.GetComponent<PlayerController>();
                //Debug.Log(playerController.isPlayerAlive);
                if (playerController != null)
                {
                    playerController.PlayerStatusUpdate(playerController.isPlayerAlive = false);
                    Debug.Log("Player hit! " + col.name);
                    //Debug.Log(playerController.isPlayerAlive);
                }
                else
                {
                    Debug.LogWarning("No PlayerController found on: " + col.name);
                }
            }
        }
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
