using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    // Player Movement
    private Vector3 playerVelocity;
    private Vector2 movementInput = Vector2.zero;
    [SerializeField]
    private float playerSpeed = 4.5f;
    [SerializeField]
    private float gravityValue = -9.81f;

    // Bomb values
    public GameObject bomb;
    public Transform playerLocation;
    BoxCollider location;
    private bool isCoroutineRunning = false;
    public LayerMask playerOnBomb;
    private BombController bombController;

    // Skill Increment Values
    public OttoGojoController characterController;
    public int bombSkill = 0;
    public float signatureSkill = 0;
    public int heavySkill = 0;
    public int ultimateSkill = 0;

   public CurseEnergyLogic curseEnergy;

    private void Start()
    {
        curseEnergy = GameObject.Find("CE Pool of Otto Gojo").GetComponent<CurseEnergyLogic>();
        characterController = GetComponent<OttoGojoController>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       movementInput = context.ReadValue<Vector2>();
    }

    public void SpawnBomb(InputAction.CallbackContext context)
    {
        if (!isCoroutineRunning && !Physics.CheckSphere(playerLocation.position, 0.6f, playerOnBomb) && context.performed && curseEnergy.CEReduction(100))
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
        bombController.SetSpawningPlayer(this.gameObject, bombSkill);

        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
    void Update()
    {
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public bool PlayerStatusUpdate(bool playerStatus)
    {

        Debug.Log("Explosion Experienced");

        if (playerStatus == false)
        {
            // exclusive for Gojo
            if (characterController.InfinityProbabilityChance() == true)
            {
                Debug.Log("===LIVED===");
                return true;
            }
            else
            {
                Debug.Log("DIED");
                Destroy(gameObject);
                return false;
            }
        } else 
            return true;


    }
}
