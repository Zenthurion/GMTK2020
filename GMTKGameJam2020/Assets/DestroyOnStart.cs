using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    public bool active = true;

    void Start()
    {
        if (active) Destroy(gameObject);
    }
}