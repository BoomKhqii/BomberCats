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

    // Ghost
    private bool inPlayer = true;
    private Collider bombCollider;
    public Collider playerCollider;

    private void Start()
    {
        // Ghoost
        bombCollider = GetComponent<Collider>();
        playerCollider = GetComponent<Collider>();

        controller = gameObject.GetComponent<CharacterController>();
    }

    void ghostBomb()
    {
        // idfk anuymorte
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
        Instantiate(bomb, new Vector3(
            Mathf.RoundToInt(playerLocation.position.x), 
            0.9160001f, Mathf.RoundToInt(playerLocation.position.z)), 
            bomb.transform.rotation);
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
}
