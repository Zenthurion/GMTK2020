using System;
using System.Collections;
using System.Collections.Generic;
using TW.Scripts;
using UnityEngine;

public class ObjectDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Container"))
        {
            Destroy(other.transform.parent.gameObject);
        }
        else if (other.CompareTag("Item"))
        {
            var item = other.GetComponent<Item>();
            if (item != null)
                item.Score();
            else
                Destroy(other.gameObject);
        }
    }
}