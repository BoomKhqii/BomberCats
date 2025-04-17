using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OttoGojoController : MonoBehaviour
{
    //private bool infinitySkill = Random.value < 0.25f;  // Passive ability | Probability chance: 20% - 25%

    // Blue
    private int signatureSkill = 3;         
    [SerializeField]
    private GameObject objectBlue;
    
    // Red
    private int heavySkill = 6;
    [SerializeField]
    private GameObject objectRed;

    // Hollow purple
    private int ultimateSkill = 12;
    [SerializeField]
    private GameObject objectHollowPurple;

    [SerializeField]
    private PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public bool InfinityProbabilityChance()
    {
        return Random.value < 0.35f;
    }

    public void BlueSkill(InputAction.CallbackContext context)
    {
        //  0   =   North
        //  180 =   South
        //  -90 =   West
        //  90  =   East
        if (context.performed)
        {
            Instantiate(objectBlue,
                new Vector3(player.transform.position.x,
                1.32f,
                player.transform.position.z + 1), Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
