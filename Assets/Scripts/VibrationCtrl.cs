using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class VibrationCtrl : MonoBehaviour
{
    public XRBaseController rightController;
    public XRBaseController leftController;

    public void SendHaptics(XRBaseController _controller)
    {
        if (_controller != null)
        {
            _controller.SendHapticImpulse(0.9f, DataHolder.instance.GameSettings.vibrationDuration);
        }
    }

}
