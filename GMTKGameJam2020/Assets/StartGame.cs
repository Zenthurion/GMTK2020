using System;
using System.Collections;
using System.Collections.Generic;
using TW.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() => { Game.Instance.NextLevel(); });
    }
}