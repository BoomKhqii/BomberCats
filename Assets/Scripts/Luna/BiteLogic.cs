using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteLogic : MonoBehaviour
{
    public LayerMask afftectedLayers;
    public GameObject lunaObject;

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        SplashAction();
    }

    public void Upgrade(float level)
    {
        if (level < 2)     // 1
            return;
        else if (level < 3)     // 2
        {

        }
        else                    // 3
        {

        }
    }

    void SplashAction()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, afftectedLayers);
        //HashSet<GameObject> stunnedObjects = new HashSet<GameObject>(); // uses hash to avoid double calling

        foreach (Collider col in colliders)
        {
            GameObject obj = col.gameObject;

            if (col.gameObject == lunaObject) continue;

            ObjectStatus enemy = obj.GetComponent<ObjectStatus>();
            CrateLogic crate = obj.gameObject.GetComponent<CrateLogic>();

            if (enemy != null) 
                enemy.StatusUpdate(false);
            else if(crate != null)
                crate.CrateDrop();
            else
                Destroy(obj.gameObject);
        }
    }
}
