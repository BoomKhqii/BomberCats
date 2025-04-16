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
    private Vector3 playerVelocity;
    private Vector2 movementInput = Vector2.zero;

    [SerializeField]
    private float playerSpeed = 4.5f;
    [SerializeField]
    private float gravityValue = -9.81f;

    public GameObject bomb;
    public Transform playerLocation;
    BoxCollider location;
    private bool isCoroutineRunning = false;
    public LayerMask playerOnBomb;
    private BombController bombController;

    private OttoGojoController characterController;
    private int bombSkill = 0;
    private int signatureSkill = 0;
    private int heavySkill = 0;
    private int ultimateSkill = 0;




    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       movementInput = context.ReadValue<Vector2>();
    }

    public void SpawnBomb()
    {
        if (!isCoroutineRunning && !Physics.CheckSphere(playerLocation.position, 0.6f, playerOnBomb))
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
        Debug.Log("PlayerController: " + this.gameObject);
        bombController.SetSpawningPlayer(this.gameObject);

        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
    /*
    IEnumerator waiter()
    {
        isCoroutineRunning = true;
        Instantiate(bomb, new Vector3(
            Mathf.RoundToInt(playerLocation.position.x), 
            0.9160001f, Mathf.RoundToInt(playerLocation.position.z)), 
            bomb.transform.rotation);

        BombController bombController = bomb.GetComponent<BombController>();
        Debug.Log("PlayerController: " + this.gameObject);
        bombController.SetSpawningPlayer(this.gameObject);

        yield return new WaitForSeconds(0.2f);
        isCoroutineRunning = false;
    }
    */
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
}
