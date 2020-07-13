using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private void Awake()
    {
        SetTagRecursive(transform);
    }


    private void SetTagRecursive(Transform t)
    {
        t.gameObject.tag = "Container";

        for (var i = 0; i < t.childCount; i++)
        {
            SetTagRecursive(t.GetChild(i));
        }
    }
}