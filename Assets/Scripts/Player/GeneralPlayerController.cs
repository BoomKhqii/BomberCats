using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralPlayerController : MonoBehaviour
{
    private CharacterController controller;

    // Player Movement
    private Vector3 playerVelocity;
    private Vector2 movementInput = Vector2.zero;
    //[SerializeField]
    public float playerSpeed = 4.5f;
    [SerializeField]
    private float gravityValue = -9.81f;
    private float originSpeed;

    // Skill Increment Values
    public int bombSkill = 0;
    public float signatureSkill = 0;
    public int heavySkill = 0;
    public int ultimateSkill = 0;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void PlayerStun(float stun) { StartCoroutine(StunAction(stun)); }

    IEnumerator StunAction(float stun)
    {
        originSpeed = playerSpeed;
        playerSpeed = 0;
        yield return new WaitForSeconds(stun);
        playerSpeed = originSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
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
