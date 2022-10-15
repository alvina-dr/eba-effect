using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCtrl : MonoBehaviour
{
    [SerializeField] List<Light> lights;



    public void LightPulseFunction()
    {
        StartCoroutine(LightPulse(lights[0]));
    }

    public IEnumerator LightPulse(Light _light)
    {
        _light.range = 5;
        yield return new WaitForSeconds(0.1f);
        _light.range = 10;
    }
}
