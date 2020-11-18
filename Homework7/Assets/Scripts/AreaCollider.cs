using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    public int sign = 0;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            FirstController firstController = SSDirector.GetInstance().CurrentScenceController as FirstController;
            firstController.wall_sign = sign;
        }
    }
}
