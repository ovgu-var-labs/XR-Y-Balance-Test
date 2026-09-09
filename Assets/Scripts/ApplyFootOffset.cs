using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Wave.Essence;
using Wave.Native;

[ExecuteInEditMode]
public class ApplyFootOffset : MonoBehaviour
{
    private static class ButtonFacade //Controller Button zordnung für Durchlauf gültig (A) oder ungültig (B)
    {
        public static bool TriggerleftButtonPressed => //X = kalibrieren
            WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Left, WVR_InputId.WVR_InputId_Alias1_Trigger);
    }
    
    //public bool applyOffset;
    public Transform tracker;
    private void Update()
    {
        if (ButtonFacade.TriggerleftButtonPressed)
        {
            ApplyOffset();
            //if (applyOffset)
            //{
            //    applyOffset = false;
                
            //}
        }

    }
    public void ApplyOffset()
    {
        transform.localPosition = new Vector3(0, -tracker.localPosition.y, 0);
        transform.localRotation = Quaternion.Inverse(tracker.localRotation);
        print("appying offset");
    }
}
