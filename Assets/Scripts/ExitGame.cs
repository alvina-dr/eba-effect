using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExitGame : MonoBehaviour
{
    Button myButton;
    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ExitFunction);
    }
    void ExitFunction()
    {
        Debug.Log("exit the game");
        Application.Quit();
    }
}