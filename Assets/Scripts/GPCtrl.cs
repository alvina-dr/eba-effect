using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPCtrl : MonoBehaviour
{

    public GameObject targetPrefab;
    public float currentTime;
    public float targetFrequence;

    //ici variable du fichier csv, on importe depuis gp ctrl
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CreateTarget();
        }
        if (currentTime >= targetFrequence)
        {
            CreateTarget();
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }

    public void CreateTarget()
    {
        Vector3 _position = new Vector3(Random.Range(-3, 3), Random.Range(-1, 3), 0);
        GameObject _target = Instantiate(targetPrefab);
        _target.transform.position = _position;
        _target.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
