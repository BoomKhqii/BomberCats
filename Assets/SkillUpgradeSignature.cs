using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeSignature : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.signatureSkill += 1;
            Destroy(this.gameObject);
        }
        else
            return;
    }
}
