using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenenManager : MonoBehaviour
{
    //public void OnTrainingButtonPressed()
    //{
    //    SceneManager.LoadScene("YBT_Training");
    //}
    public void OnVRButtonPressed()
    {
        SceneManager.LoadScene("3. YBT_VR");
    }
    public void OnARButtonPressed()
    {
        SceneManager.LoadScene("3. YBT_AR");
    }
    public void OnVRHapticButtonPressed()
    {
        SceneManager.LoadScene("3. YBT_VR_Haptic");
    }
    public void OnAuswertungButtonPressed()
    {
        SceneManager.LoadScene("4. Auswertung");
    }


}
