using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerEventHandler(GameObject trigger, GameObject other);

public class TriggerEventRouter : MonoBehaviour
{
    public TriggerEventHandler callback;
    // player contact with objects
    void OnTriggerEnter(Collider other)
    {
        if (callback != null)
        {
            callback(this.gameObject, other.gameObject);
        }
    }
}
