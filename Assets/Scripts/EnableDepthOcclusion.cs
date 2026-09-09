//using UnityEngine;
//using Wave.XR.Settings;
//using Wave.Native;

//public class EnableDepthOcclusion : MonoBehaviour
//{
//    void Start()
//    {
//        Debug.Log("Passthrough aktivieren...");
//        Interop.WVR_ShowPassthroughUnderlay(true); // Passthrough aktivieren

//        Debug.Log("Scene Perception aktivieren...");
//        WaveXRSettings.Instance.enableScenePerception = true; // Scene Perception aktivieren

//        Debug.Log("Setze Passthrough-Bildqualität auf höchste Qualität...");
//        Interop.WVR_SetPassthroughImageQuality(WVR_PassthroughImageQuality.QualityMode); // Höchste Qualität

//        Debug.Log("Aktiviere Depth Occlusion...");
//        bool success = Interop.WVR_StartScenePerception(WVR_ScenePerceptionType.Depth);
//        if (success)
//        {
//            Debug.Log("Depth Occlusion erfolgreich gestartet!");
//        }
//        else
//        {
//            Debug.LogWarning("Depth Occlusion konnte nicht gestartet werden!");
//        }

//        Debug.Log("Setze Overlay Alpha auf 1.0 für richtige Darstellung...");
//        Interop.WVR_SetPassthroughOverlayAlpha(1.0f);
//    }
//}
