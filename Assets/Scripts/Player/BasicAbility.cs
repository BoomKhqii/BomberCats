using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAbility : GeneralPlayerController
{
    // Bomb values
    public GameObject bomb;
    public Transform playerLocation;
    private bool isCoroutineRunning = false;
    public LayerMask playerOnBomb;
    private BombController bombController;

    public GeneralPlayerController skill;
    public CurseEnergyLogic curseEnergy;
    public string ceName;

    public float bombCost = 100;

    private void Start()
    {
        //curseEnergy = GameObject.Find(ceName).GetComponent<CurseEnergyLogic>();
        skill = GetComponent<GeneralPlayerController>();
    }

    public void SpawnBomb(InputAction.CallbackContext context)
    {
        if (!isCoroutineRunning && !Physics.CheckSphere(playerLocation.position, 0.6f, playerOnBomb) && context.performed && curseEnergy.CEReduction(bombCost))
        {
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
        bombController.SetSpawningPlayer(this.gameObject, skill.bombSkill);

        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
}
