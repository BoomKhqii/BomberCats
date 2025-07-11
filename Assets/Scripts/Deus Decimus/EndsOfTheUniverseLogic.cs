using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndsOfTheUniverseLogic : MonoBehaviour
{
    [Header("Pull Settings")]
    public float pullStrength = 2f;
    private float pullRadius = 6f;
    private float killRadius = 4f;
    public LayerMask affectedLayers;

    public GameObject deusDecimus;
    public float levelEndsOfTheUniverse = 0;

    private int level;

    // Start is called before the first frame update
    void Start()
    {
        level = deusDecimus.GetComponent<GeneralPlayerController>().ultimateSkill;
        Upgrade(level);

        StartCoroutine(BlackHoleDeath());
    }

    public void Upgrade(float lvl)
    {
        if (lvl < 2)     // 1
            return;
        else if (lvl < 3)     // 2
        {
            pullRadius = 6.5f;
        }
        else                    // 3
        {
            pullRadius = 6.5f;
            pullStrength = 15f;
        }
    }

    IEnumerator BlackHoleDeath()
    {
        yield return new WaitForSeconds(5f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, killRadius, affectedLayers);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == deusDecimus) continue; // Wont pull the caster

            // Kills players inside
            ObjectStatus objectStatus = col.GetComponent<ObjectStatus>();
            if (objectStatus != null)
            {
                objectStatus.StatusUpdate(false);
            }

            // Crates breaks
            CrateLogic crate = col.gameObject.GetComponent<CrateLogic>();
            if (col.gameObject.CompareTag("Breakable"))
            {
                crate.CrateDrop();
            }

            // Bombs Destroy
            GameObject bomb = GetComponent<GameObject>();
            if (col.CompareTag("bomb"))
            {
                Destroy(bomb);
            }
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius, affectedLayers);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == deusDecimus) continue; // Wont pull the caster

            // Pull Rigidbody objects using force
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null && !rb.isKinematic)
            {
                Vector3 direction = (transform.position - rb.position).normalized;
                rb.AddForce(direction * pullStrength, ForceMode.Acceleration);
            }

            // Pull CharacterController objects manually
            CharacterController cc = col.GetComponent<CharacterController>();
            if (cc != null)
            {
                Vector3 direction = (transform.position - col.transform.position).normalized;
                cc.Move(direction * pullStrength * Time.fixedDeltaTime);
            }

            CrateLogic crate = col.gameObject.GetComponent<CrateLogic>();
            if (col.gameObject.CompareTag("Breakable"))
            {
                crate.CrateDrop();
            }

        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
}
