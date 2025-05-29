using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeHeavy : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GeneralPlayerController player = other.GetComponent<GeneralPlayerController>();
            if (player == null) return;

            //player.heavySkill += 1;
            player.MaxLevelingSystem(2); // new system to avoid leveling issues
            Destroy(this.gameObject);
        }
        else
            return;
    }
}
