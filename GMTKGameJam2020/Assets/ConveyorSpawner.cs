using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    public Transform spawn;
    public ConveyorBelt conveyorBeltPrefab;

    private Transform _parent;

    private void Awake()
    {
        _parent = transform.parent;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Conveyor")) return;

        Instantiate(conveyorBeltPrefab, spawn.position, Quaternion.Euler(0, -90, 90));
    }
}