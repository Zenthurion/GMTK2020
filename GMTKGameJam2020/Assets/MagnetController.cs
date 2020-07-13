using System;
using System.Collections;
using System.Collections.Generic;
using TW.Scripts;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    public class MagnetisedObject
    {
        public Rigidbody rb;
        public Vector3 offset;
        public Vector3 eulerRotation;
        public Vector3 initialFacing;

        public MagnetisedObject(Rigidbody rb, Vector3 offset, Vector3 initialFacing)
        {
            this.rb = rb;
            this.offset = offset;
            this.initialFacing = initialFacing;

            eulerRotation = rb.rotation.eulerAngles;
        }
    }

    public Material magnetisedMaterial;
    public Material demagnetisedMaterial;

    public InputMode magnetControl = InputMode.Hold;
    public List<KeyCode> magnetActivators;

    private List<Rigidbody> _inRange;
    private List<MagnetisedObject> _onMagnet;
    private List<MagnetisedObject> _invalidated = new List<MagnetisedObject>();

    private bool _isHolding;
    private MeshRenderer _renderer;

    public List<MeshRenderer> otherIndicators = new List<MeshRenderer>();
    public List<Light> coloredLights = new List<Light>();

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();

        _inRange = new List<Rigidbody>();
        _onMagnet = new List<MagnetisedObject>();
    }

    private void FixedUpdate()
    {
        if (!_isHolding) return;

        _invalidated.Clear();

        foreach (var obj in _onMagnet)
        {
            if (obj.rb == null)
            {
                _invalidated.Add(obj);
                continue;
            }

            var rotated = Quaternion.FromToRotation(obj.initialFacing, transform.TransformDirection(transform.forward));

            var newOffset = Quaternion.AngleAxis(rotated.eulerAngles.y, new Vector3(0, 1, 0)) * obj.offset;
            obj.rb.MovePosition(transform.position + newOffset);

            var objOffset = obj.offset;
            objOffset.y = 0;
            newOffset.y = 0;

            var newRotation = Quaternion.FromToRotation(objOffset, newOffset);
            var newRot = (newRotation.eulerAngles + obj.eulerRotation);

            obj.rb.MoveRotation(Quaternion.Euler(newRot));
        }

        foreach (var item in _invalidated)
        {
            _onMagnet.Remove(item);
            _inRange.Remove(item.rb);
        }
    }

    private void Update()
    {
        if (magnetControl == InputMode.Hold)
        {
            foreach (var key in magnetActivators)
            {
                if (Input.GetKey(key))
                {
                    if (!_isHolding)
                        Magnetise();
                    return;
                }
            }

            if (_isHolding)
            {
                Demagnetise();
            }
        }
        else
        {
            foreach (var key in magnetActivators)
            {
                if (Input.GetKeyDown(key))
                {
                    if (_isHolding)
                    {
                        Demagnetise();
                    }
                    else
                    {
                        Magnetise();
                    }
                }
            }
        }
    }

    private void ChangeMaterial(Material material)
    {
        var materials = _renderer.materials;
        if (materials.Length <= 1)
        {
            return;
        }

        materials[1] = material;
        _renderer.materials = materials;

        foreach (var other in otherIndicators)
        {
            var index = other.gameObject.name.Equals("AlarmLight") ? 0 : 2;
            materials = other.materials;
            if (materials.Length <= index)
            {
                return;
            }

            materials[index] = material;
            other.materials = materials;
        }

        foreach (var l in coloredLights)
        {
            l.color = material.color;
        }
    }

    private void Magnetise()
    {
        if (_isHolding) return;

        ChangeMaterial(magnetisedMaterial);

        foreach (var item in _inRange)
        {
            item.isKinematic = true;
            item.useGravity = false;

            _onMagnet.Add(new MagnetisedObject(item, item.transform.position - transform.position,
                transform.TransformDirection(transform.forward)));
        }

        _isHolding = true;
    }

    private void Demagnetise()
    {
        if (!_isHolding) return;

        ChangeMaterial(demagnetisedMaterial);

        foreach (var item in _onMagnet)
        {
            item.rb.isKinematic = false;
            item.rb.useGravity = true;
        }

        _onMagnet.Clear();
        _isHolding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var rb = other.GetComponent<Rigidbody>();
            _inRange.Add(rb);
        }
        else if (other.CompareTag("Container"))
        {
            var rb = other.transform.parent.GetComponent<Rigidbody>();
            _inRange.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var rb = other.GetComponent<Rigidbody>();
            _inRange.Remove(rb);
        }
        else if (other.CompareTag("Container"))
        {
            var rb = other.transform.parent.GetComponent<Rigidbody>();
            _inRange.Remove(rb);
        }
    }
}