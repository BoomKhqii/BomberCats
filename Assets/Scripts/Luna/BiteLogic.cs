using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteLogic : MonoBehaviour
{
    public LayerMask afftectedLayers;

    void Update()
    {
        SplashAction();
    }

    void SplashAction()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, afftectedLayers);
        //HashSet<GameObject> stunnedObjects = new HashSet<GameObject>(); // uses hash to avoid double calling

        foreach (Collider col in colliders)
        {
            GameObject obj = col.gameObject;

            //if (col.gameObject == lunaObject) continue;
            /*
            if (!stunnedObjects.Contains(obj))
            {
                stunnedObjects.Add(obj);
                ObjectStatus enemy = obj.GetComponent<ObjectStatus>();

                enemy.StatusUpdate(false); 
            }
            */
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
