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

    // Skill Increment Values
    public int bombSkill = 0;
    public float signatureSkill = 1;
    public int heavySkill = 0;
    public int ultimateSkill = 0;

    public PlayerInput stun;
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void PlayerStun(float duration) { StartCoroutine(StunAction(duration)); }

    IEnumerator StunAction(float duration)
    {
        stun.enabled = false;
        yield return new WaitForSeconds(duration);
        stun.enabled = true;    
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (movementInput != Vector2.zero)
        {
            // Determine the angle of the movement input
            float angle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg;

            // Round angle to nearest 90 degrees (0, 90, 180, 270)
            float snappedAngle = Mathf.Round(angle / 90f) * 90f;

            // Convert the snapped angle back into a direction vector
            Vector3 moveDir = Quaternion.Euler(0, snappedAngle, 0) * Vector3.forward;

            controller.Move(moveDir * Time.deltaTime * playerSpeed);
            transform.forward = moveDir;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GhostableBlock ghostBlock))
        {
            ghostBlock.AddGhost(GetComponent<Collider>());
        }
    }
}
