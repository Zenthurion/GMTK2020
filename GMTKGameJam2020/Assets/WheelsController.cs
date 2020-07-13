using System;
using System.Collections;
using System.Collections.Generic;
using TW.Scripts;
using UnityEngine;

public class WheelsController : MonoBehaviour
{
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform backLeftWheel;
    public Transform backRightWheel;

    public WheelControl motorWheels = WheelControl.Front;
    public float torqueIncrease = 10;
    public float maxWheelTorque = 25;
    public float brakeTorque = 1.5f;

    public WheelControl steering = WheelControl.Back;
    public float rotationSpeed = 30;
    public float maxWheelRotation = 30;
    private float _currentRotation;

    private Quaternion _frontLeftWheelDefaultRotation;
    private Quaternion _frontRightWheelDefaultRotation;
    private Quaternion _backLeftWheelDefaultRotation;
    private Quaternion _backRightWheelDefaultRotation;

    private WheelCollider _frontLeftWheelCollider;
    private WheelCollider _frontRightWheelCollider;
    private WheelCollider _backLeftWheelCollider;
    private WheelCollider _backRightWheelCollider;

    private WheelCollider[] _wheelColliders;

    private void Awake()
    {
        _frontLeftWheelDefaultRotation = frontLeftWheel.localRotation;
        _frontRightWheelDefaultRotation = frontRightWheel.localRotation;
        _backLeftWheelDefaultRotation = backLeftWheel.localRotation;
        _backRightWheelDefaultRotation = backRightWheel.localRotation;

        _frontLeftWheelCollider = frontLeftWheel.GetComponent<WheelCollider>();
        _frontRightWheelCollider = frontRightWheel.GetComponent<WheelCollider>();
        _backLeftWheelCollider = backLeftWheel.GetComponent<WheelCollider>();
        _backRightWheelCollider = backRightWheel.GetComponent<WheelCollider>();

        _wheelColliders = new WheelCollider[4];
        _wheelColliders[0] = _frontLeftWheelCollider;
        _wheelColliders[1] = _frontRightWheelCollider;
        _wheelColliders[2] = _backLeftWheelCollider;
        _wheelColliders[3] = _backRightWheelCollider;
    }

    public void ResetWheelRotation()
    {
        frontLeftWheel.localRotation = _frontLeftWheelDefaultRotation;
        frontRightWheel.localRotation = _frontRightWheelDefaultRotation;
        backLeftWheel.localRotation = _backLeftWheelDefaultRotation;
        backRightWheel.localRotation = _backRightWheelDefaultRotation;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }
        else
        {
            ReleaseSteering();
        }

        if (Input.GetKey(KeyCode.W))
        {
            AccelerateForward();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            AccelerateBackward();
        }
        else
        {
            ReleaseAccelerator();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Brake();
        }
        else
        {
            ReleaseBake();
        }
    }

    public void AccelerateForward()
    {
        switch (motorWheels)
        {
            case WheelControl.All:
                AccelerateWheel(_frontLeftWheelCollider);
                AccelerateWheel(_frontRightWheelCollider);
                AccelerateWheel(_backLeftWheelCollider);
                AccelerateWheel(_backRightWheelCollider);
                break;
            case WheelControl.Back:
                AccelerateWheel(_backLeftWheelCollider);
                AccelerateWheel(_backRightWheelCollider);
                break;
            case WheelControl.Front:
                AccelerateWheel(_frontLeftWheelCollider);
                AccelerateWheel(_frontRightWheelCollider);
                break;
        }
    }

    private void AccelerateWheel(WheelCollider wheel)
    {
        var change = torqueIncrease * Time.deltaTime;
        if (wheel.motorTorque + change >= maxWheelTorque)
        {
            wheel.motorTorque = maxWheelTorque;
            return;
        }

        wheel.motorTorque += change;
    }

    private void DecelerateWheel(WheelCollider wheel)
    {
        var change = -torqueIncrease * Time.deltaTime;
        if (wheel.motorTorque + change <= torqueIncrease)
        {
            wheel.motorTorque = -torqueIncrease;
            return;
        }

        wheel.motorTorque += change;
    }

    public void AccelerateBackward()
    {
        switch (motorWheels)
        {
            case WheelControl.All:
                DecelerateWheel(_frontLeftWheelCollider);
                DecelerateWheel(_frontRightWheelCollider);
                DecelerateWheel(_backLeftWheelCollider);
                DecelerateWheel(_backRightWheelCollider);
                break;
            case WheelControl.Back:
                DecelerateWheel(_backLeftWheelCollider);
                DecelerateWheel(_backRightWheelCollider);
                break;
            case WheelControl.Front:
                DecelerateWheel(_frontLeftWheelCollider);
                DecelerateWheel(_frontRightWheelCollider);
                break;
        }
    }

    public void ReleaseAccelerator()
    {
        foreach (var wheel in _wheelColliders)
        {
            wheel.motorTorque = 0;
        }
    }

    public void Brake()
    {
        foreach (var wheel in _wheelColliders)
        {
            wheel.brakeTorque = brakeTorque;
        }
    }

    public void ReleaseBake()
    {
        foreach (var wheel in _wheelColliders)
        {
            wheel.brakeTorque = 0;
        }
    }

    public void TurnRight()
    {
        var change = rotationSpeed * Time.deltaTime;
        if (_currentRotation + change >= maxWheelRotation)
            _currentRotation = maxWheelRotation;
        else
            _currentRotation += change;

        switch (steering)
        {
            case WheelControl.All:
                _frontLeftWheelCollider.steerAngle = _currentRotation;
                _frontRightWheelCollider.steerAngle = _currentRotation;
                _backLeftWheelCollider.steerAngle = -_currentRotation;
                _backRightWheelCollider.steerAngle = -_currentRotation;

                frontLeftWheel.localRotation = Quaternion.Euler(0, _currentRotation + 180, 0);
                frontRightWheel.localRotation = Quaternion.Euler(0, _currentRotation, 0);
                backLeftWheel.localRotation = Quaternion.Euler(0, -_currentRotation + 180, 0);
                backRightWheel.localRotation = Quaternion.Euler(0, -_currentRotation, 0);
                break;
            case WheelControl.Back:
                _backLeftWheelCollider.steerAngle = -_currentRotation;
                _backRightWheelCollider.steerAngle = -_currentRotation;

                backLeftWheel.localRotation = Quaternion.Euler(0, -_currentRotation + 180, 0);
                backRightWheel.localRotation = Quaternion.Euler(0, -_currentRotation, 0);
                break;
            case WheelControl.Front:
                _frontLeftWheelCollider.steerAngle = _currentRotation;
                _frontRightWheelCollider.steerAngle = _currentRotation;

                frontLeftWheel.localRotation = Quaternion.Euler(0, _currentRotation + 180, 0);
                frontRightWheel.localRotation = Quaternion.Euler(0, _currentRotation, 0);
                break;
        }
    }

    public void TurnLeft()
    {
        var change = -rotationSpeed * Time.deltaTime;
        if (_currentRotation + change <= -maxWheelRotation)
            _currentRotation = -maxWheelRotation;
        else
            _currentRotation += change;


        switch (steering)
        {
            case WheelControl.All:
                _frontLeftWheelCollider.steerAngle = _currentRotation;
                _frontRightWheelCollider.steerAngle = _currentRotation;
                _backLeftWheelCollider.steerAngle = -_currentRotation;
                _backRightWheelCollider.steerAngle = -_currentRotation;

                frontLeftWheel.localRotation = Quaternion.Euler(0, _currentRotation + 180, 0);
                frontRightWheel.localRotation = Quaternion.Euler(0, _currentRotation, 0);
                backLeftWheel.localRotation = Quaternion.Euler(0, -_currentRotation + 180, 0);
                backRightWheel.localRotation = Quaternion.Euler(0, -_currentRotation, 0);
                break;
            case WheelControl.Back:
                _backLeftWheelCollider.steerAngle = -_currentRotation;
                _backRightWheelCollider.steerAngle = -_currentRotation;

                backLeftWheel.localRotation = Quaternion.Euler(0, -_currentRotation + 180, 0);
                backRightWheel.localRotation = Quaternion.Euler(0, -_currentRotation, 0);
                break;
            case WheelControl.Front:
                _frontLeftWheelCollider.steerAngle = _currentRotation;
                _frontRightWheelCollider.steerAngle = _currentRotation;

                frontLeftWheel.localRotation = Quaternion.Euler(0, _currentRotation + 180, 0);
                frontRightWheel.localRotation = Quaternion.Euler(0, _currentRotation, 0);
                break;
        }
    }

    public void ReleaseSteering()
    {
        float change = 0;
        if (_currentRotation < 0)
        {
            change = rotationSpeed * Time.deltaTime;
            if (_currentRotation + change >= 0)
                _currentRotation = 0;
            else
                _currentRotation += change;
        }
        else if (_currentRotation > 0)
        {
            change = -rotationSpeed * Time.deltaTime;
            if (_currentRotation + change <= 0)
                _currentRotation = 0;
            else
                _currentRotation += change;
        }

        switch (steering)
        {
            case WheelControl.All:
                _frontLeftWheelCollider.steerAngle = _currentRotation;
                _frontRightWheelCollider.steerAngle = _currentRotation;
                _backLeftWheelCollider.steerAngle = -_currentRotation;
                _backRightWheelCollider.steerAngle = -_currentRotation;

                frontLeftWheel.localRotation = Quaternion.Euler(0, _currentRotation + 180, 0);
                frontRightWheel.localRotation = Quaternion.Euler(0, _currentRotation, 0);
                backLeftWheel.localRotation = Quaternion.Euler(0, -_currentRotation + 180, 0);
                backRightWheel.localRotation = Quaternion.Euler(0, -_currentRotation, 0);
                break;
            case WheelControl.Back:
                _backLeftWheelCollider.steerAngle = -_currentRotation;
                _backRightWheelCollider.steerAngle = -_currentRotation;

                backLeftWheel.localRotation = Quaternion.Euler(0, -_currentRotation + 180, 0);
                backRightWheel.localRotation = Quaternion.Euler(0, -_currentRotation, 0);
                break;
            case WheelControl.Front:
                _frontLeftWheelCollider.steerAngle = _currentRotation;
                _frontRightWheelCollider.steerAngle = _currentRotation;

                frontLeftWheel.localRotation = Quaternion.Euler(0, _currentRotation + 180, 0);
                frontRightWheel.localRotation = Quaternion.Euler(0, _currentRotation, 0);
                break;
        }
    }
}