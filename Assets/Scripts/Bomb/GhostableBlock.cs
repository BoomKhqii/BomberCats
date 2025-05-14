using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostableBlock : MonoBehaviour
{
    public Collider ownCollider;
    private List<Collider> ghostedColliders = new List<Collider>();

    void Start()
    {
        //ownCollider = GetComponent<Collider>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ghostedColliders.Contains(other) && other.CompareTag("Player"))
        {
            ownCollider.enabled = true;
            Physics.IgnoreCollision(ownCollider, other, true);
            ghostedColliders.Add(other);

            Debug.Log($"{other.name} is now ghosted for {name}");
        }
    }

    public void AddGhost(Collider other)
    {
        Debug.Log("ca");
        if (!ghostedColliders.Contains(other))
        {
            Physics.IgnoreCollision(ownCollider, other, true);
            ghostedColliders.Add(other);

            Debug.Log($"{other.name} is now ghosted for {name}");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (ghostedColliders.Contains(other))
        {
            Physics.IgnoreCollision(ownCollider, other, false);
            ghostedColliders.Remove(other);
            Debug.Log($"{other.name} exited and is now colliding with {name}");
        }
    }
 
}
