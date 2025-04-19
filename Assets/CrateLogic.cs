using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateLogic : MonoBehaviour
{
    public LayerMask destroy;
    public Transform crate;

    //private float willDrop = 0.5f;
    private float chanceBomb = 0.35f;
    private float chanceSignature = 0.3f;
    private float chanceHeavy = 0.2f;
    private float chanceUltimate = 0.15f;

    public GameObject upgradeBomb;
    public GameObject upgradeSignature;
    public GameObject upgradeHeavy;
    public GameObject upgradeUltimate;

    public void CrateDrop()
    {
        if (Random.value < 0.6f)
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue < chanceBomb)
            {
                Instantiate(upgradeBomb, new Vector3(crate.position.x, 1f, crate.position.z),Quaternion.identity);
                Debug.Log("B (Common) is shown!");
            }
            else if (randomValue < chanceBomb + chanceSignature)
            {
                Instantiate(upgradeSignature, new Vector3(crate.position.x, 1f, crate.position.z), Quaternion.identity);
                Debug.Log("S (Uncommon) is shown!");
            }
            else if (randomValue < chanceSignature + chanceSignature + chanceHeavy)
            {
                Instantiate(upgradeHeavy, new Vector3(crate.position.x, 1f, crate.position.z), Quaternion.identity);
                Debug.Log("H (Rare) is shown!");
            }
            else
            {
                Instantiate(upgradeUltimate, new Vector3(crate.position.x, 1f, crate.position.z), Quaternion.identity);
                Debug.Log("U (Ultra Rare) is shown!");
            }
        }
        else
            Debug.Log("NONE");

        Destroy(this.gameObject);
    }
}
