using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetButton : MonoBehaviour
{

    public UnityEvent onShootEvent;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnShoot()
    {
        onShootEvent.Invoke();
        Debug.Log("this is invoke");
    }
}
