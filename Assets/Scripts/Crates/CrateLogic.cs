using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateLogic : MonoBehaviour
{
    public LayerMask destroy;
    public Transform crate;

    private float chanceBomb = 0.40f;       // Bomb    35% -> 40%
    private float chanceSignature = 0.35f;   // Sig     30% -> 35%
    private float chanceHeavy = 0.2f;       // Heavy   20% -> 20%
    //                                         Ult     15% -> 05%

    public GameObject upgradeBomb;
    public GameObject upgradeSignature;
    public GameObject upgradeHeavy;
    public GameObject upgradeUltimate;

    public void CrateDrop()
    {
        if (Random.value < 0.3f)
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue < chanceBomb)
            {
                Debug.Log("Bomb");
                Instantiate(upgradeBomb, new Vector3(crate.position.x, 1f, crate.position.z),Quaternion.identity);
            }
            else if (randomValue < chanceBomb + chanceSignature)
            {
                Debug.Log("sig");
                Instantiate(upgradeSignature, new Vector3(crate.position.x, 1f, crate.position.z), Quaternion.identity);
            }
            else if (randomValue < chanceSignature + chanceSignature + chanceHeavy)
            {
                Debug.Log("heavy");
                Instantiate(upgradeHeavy, new Vector3(crate.position.x, 1f, crate.position.z), Quaternion.identity);
            }
            else
            {
                Debug.Log("ult");
                Instantiate(upgradeUltimate, new Vector3(crate.position.x, 1f, crate.position.z), Quaternion.identity);
            }
        }

        Destroy(this.gameObject);
    }
}
