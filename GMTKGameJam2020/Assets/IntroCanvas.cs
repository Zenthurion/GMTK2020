using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCanvas : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += (scene, mode) => { gameObject.SetActive(scene.name == "LVL 1 - Intro"); };
    }
}