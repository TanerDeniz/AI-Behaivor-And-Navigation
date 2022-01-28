using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrateLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GunLogic gunLogic=other.GetComponentInChildren<GunLogic>();
            if (gunLogic)
            {
                gunLogic.RefillAmmo();
                Destroy(gameObject);
            }
        }
    }
}
