using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseEnergyLogic : MonoBehaviour
{
    [SerializeField]
    private float maxPool = 4000;
    public float currentPool;
    private float regenRate = 50;

    //public Image ceBarStatus;
    public Text ceTextStatus;

    void Start()
    {
        currentPool = maxPool;
        UpdateCE();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCE();

        if (currentPool < maxPool)
        {
            currentPool += regenRate * Time.deltaTime;
        }
        if (currentPool < 0)
            currentPool = 0;
    }

    public bool CEReduction(float reduction)
    {
        if (currentPool-reduction < 0)
        {
            //Debug.Log("cdred passed false");
            return false;
        }
        else
        {
            //Debug.Log("cdred passed true");
            currentPool -= reduction;
            return true;
        }
    }

    public void UpdateCE()
    {
        ceTextStatus.text = "Curse Energy: " + Mathf.RoundToInt(currentPool).ToString();
    }

}
