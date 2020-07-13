using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsInfo : MonoBehaviour
{
    public Canvas controls;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        controls.gameObject.SetActive(!controls.gameObject.activeSelf);
    }
}