using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotation;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.MoveRotation(Quaternion.Euler(_rb.transform.localRotation.eulerAngles + (rotation * Time.deltaTime)));
    }
}