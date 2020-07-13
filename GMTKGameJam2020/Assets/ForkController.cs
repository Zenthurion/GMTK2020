using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ForkController : MonoBehaviour
{
    private enum ForkState
    {
        WaitingBottom,
        MovingUp,
        WaitingTop,
        MovingDown
    }

    public Transform minHeight;
    public Transform maxHeight;
    public Transform forkBase;

    public Transform[] telescopeRails;

    public float maxTelescopeExtension;

    public bool controllable;

    public float moveSpeed = 2;
    public float pauseDuration = 1;

    private ForkState _state = ForkState.MovingUp;

    private Vector3[] _telescopeDefaultPositions;

    private float _currentTimer;
    private float _telescopeExtension;

    private void Awake()
    {
        _telescopeDefaultPositions = new Vector3[telescopeRails.Length];

        for (var i = 0; i < telescopeRails.Length; i++)
        {
            _telescopeDefaultPositions[i] = telescopeRails[i].position;
        }
    }


    private void Update()
    {
        if (controllable)
        {
            PlayerControl();
        }
        else
        {
            AutomaticMovement();
        }
    }

    private void PlayerControl()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MovingUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MovingDown();
        }
    }

    private void AutomaticMovement()
    {
        switch (_state)
        {
            case ForkState.MovingUp:
                MovingUp();
                break;
            case ForkState.MovingDown:
                MovingDown();
                break;
            case ForkState.WaitingBottom:
                Waiting();
                break;
            case ForkState.WaitingTop:
                Waiting();
                break;
        }
    }

    private void MovingUp()
    {
        if (forkBase.position.y >= maxHeight.position.y)
        {
            _state = ForkState.WaitingTop;
            return;
        }

        _state = ForkState.MovingUp;

        var move = 1 * Time.deltaTime * moveSpeed;

        forkBase.position += new Vector3(0, move, 0);

        if (telescopeRails == null) return;

        if (_telescopeExtension > maxTelescopeExtension)
        {
            return;
        }

        foreach (var rail in telescopeRails)
        {
            move /= telescopeRails.Length;
            _telescopeExtension += move;
            rail.position += new Vector3(0, move, 0);
        }
    }

    private void MovingDown()
    {
        if (forkBase.position.y <= minHeight.position.y)
        {
            _state = ForkState.WaitingBottom;
            return;
        }

        _state = ForkState.MovingDown;

        var move = 1 * Time.deltaTime * moveSpeed;

        forkBase.position -= new Vector3(0, move, 0);

        if (telescopeRails == null) return;

        if (_telescopeExtension <= 0)
        {
            return;
        }

        foreach (var rail in telescopeRails)
        {
            move /= telescopeRails.Length;
            _telescopeExtension -= move;
            rail.position -= new Vector3(0, move, 0);
        }
    }

    private void Waiting()
    {
        if (_currentTimer > pauseDuration)
        {
            _state = _state == ForkState.WaitingBottom ? ForkState.MovingUp : ForkState.MovingDown;
            _currentTimer = 0;
            return;
        }

        _currentTimer += Time.deltaTime;
    }
}