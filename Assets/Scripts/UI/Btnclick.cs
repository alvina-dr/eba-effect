using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btnclick : MonoBehaviour
{
    public void OnButtonPress()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

    }
}