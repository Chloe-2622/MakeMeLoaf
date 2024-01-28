using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingTrigger : MonoBehaviour
{


    private void OnTriggerStay(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if(otherObj == null)
        {
            return;
        }

        Selectable selectable = otherObj.GetComponent<Selectable>();
        if(selectable == null)
        {
            return;
        }

         CommandManager.instance.TryToSell(selectable);
    }
}
