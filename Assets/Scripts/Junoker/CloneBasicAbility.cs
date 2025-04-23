using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloneBasicAbility : MonoBehaviour
{
    // Bomb values
    public GameObject bomb;
    public Transform playerLocation;
    private bool isCoroutineRunning = false;
    public LayerMask playerOnBomb;
    private BombController bombController;

    public CurseEnergyLogic curseEnergy;
    public string ceName;

    private void Start()
    {
        curseEnergy = GameObject.Find(ceName).GetComponent<CurseEnergyLogic>();
    }

    public void SpawnBomb(float ceCost)
    {
        if (!isCoroutineRunning && !Physics.CheckSphere(playerLocation.position, 0.6f, playerOnBomb) && curseEnergy.CEReduction(ceCost))
        {
            StartCoroutine(waiter());
        }
        else { return; }
        //Debug.Log("Clone didn't use basic ability");  
    }
    IEnumerator waiter()
    {
        isCoroutineRunning = true;

        GameObject bombInstance = Instantiate(bomb,
            new Vector3(
                Mathf.RoundToInt(playerLocation.position.x),
                0.9160001f,
                Mathf.RoundToInt(playerLocation.position.z)
            ),
            bomb.transform.rotation);

        BombController bombController = bombInstance.GetComponent<BombController>();
        bombController.SetSpawningPlayer(this.gameObject, 0);

        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
}
