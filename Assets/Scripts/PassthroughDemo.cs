using UnityEngine;
using UnityEngine.SceneManagement;
using Wave.Essence;
using Wave.Native;

public class PassthroughDemo : MonoBehaviour
{
    [SerializeField] private PassthroughDemoHelper passthroughHelper;

    // Update is called once per frame
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "3. YBT_AR")
        {
            passthroughHelper.ShowPassthroughUnderlay(!Interop.WVR_IsPassthroughOverlayVisible());
        }
        else if (SceneManager.GetActiveScene().name == "3. YBT_VR_Haptic")
        {
            passthroughHelper.ShowPassthroughUnderlay(!Interop.WVR_IsPassthroughOverlayVisible());
        }

    }

    void Update()
    {
        if (ButtonFacade.YButtonPressed)
        {
            passthroughHelper.ShowPassthroughUnderlay(!Interop.WVR_IsPassthroughOverlayVisible());
        }
    }

    private static class ButtonFacade
    {
        public static bool YButtonPressed =>
            WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Left, WVR_InputId.WVR_InputId_Alias1_Y);
    }
}
