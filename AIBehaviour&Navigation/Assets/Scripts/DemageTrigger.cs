using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemageTrigger : MonoBehaviour
{

    PlayerLogic playerLogic;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            playerLogic = other.GetComponent<PlayerLogic>();
            if (playerLogic)
            {
                playerLogic.TakeDamage(15);
            }
        }
    }
}
