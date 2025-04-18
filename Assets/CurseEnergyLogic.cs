using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseEnergyLogic : MonoBehaviour
{
    [SerializeField]
    private float maxPool = 4000;
    [SerializeField]
    private float currentPool;
    private float ceReduction = 0;

    private float regenRate = 50;

    //public Image ceBarStatus;
    public Text ceTextStatus;

    void Start()
    {
        Debug.Log(gameObject.name); currentPool = maxPool;
        //currentPool -= 2000;
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

    public float CEReduction(float reduction)
    {
        currentPool -= reduction;
        return currentPool;
    }

    public void UpdateCE()
    {
        ceTextStatus.text = "Curse Energy: " + Mathf.RoundToInt(currentPool).ToString();
        //ceBarStatus.fillAmount = currentPool / maxPool;
    }

}
