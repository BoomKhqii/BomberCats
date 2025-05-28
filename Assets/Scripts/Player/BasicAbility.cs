
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAbility : MonoBehaviour
{
    public GeneralPlayerController player;

    // Bomb values
    public GameObject bomb;
    public Transform playerLocation;
    private bool isCoroutineRunning = false;
    public LayerMask playerOnBomb;
    private BombController bombController;

    /*
    public GeneralPlayerController skill;
    public CurseEnergyLogic curseEnergy;
    */
    public string ceName;

    public float bombCost = 100;

    
    void Start()
    {
        player = gameObject.GetComponent<GeneralPlayerController>();
    }
    
    
    public void SpawnBomb(InputAction.CallbackContext context)
    {
        Debug.Log("pressed");

        if (!isCoroutineRunning && 
            !Physics.CheckSphere(playerLocation.position, 0.6f, playerOnBomb) && 
            context.performed &&
            player.curseEnergy.CEReduction(bombCost))/*curseEnergy.CEReduction(bombCost)*/ // UIGameObject.GetComponent<CurseEnergyLogic>().CEReduction(bombCost)
        {
            Debug.Log("Bomb Spawned");
            StartCoroutine(waiter());
        }
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
        bombController.SetSpawningPlayer(this.gameObject, player.bombSkill);

        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
}
