using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public float maxSpeed = 7f;
    public Transform centerOfMass;
    public bool debug;

    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        if (debug)
        {
            _rigidbody.centerOfMass = centerOfMass.localPosition;
        }

        var speed = Vector3.Magnitude(_rigidbody.velocity); // test current object speed

        if (speed > maxSpeed)
        {
            var brakeSpeed = speed - maxSpeed; // calculate the speed decrease

            var normalisedVelocity = _rigidbody.velocity.normalized;
            var brakeVelocity = normalisedVelocity * brakeSpeed; // make the brake Vector3 value

            _rigidbody.AddForce(-brakeVelocity); // apply opposing brake force
        }
    }
}