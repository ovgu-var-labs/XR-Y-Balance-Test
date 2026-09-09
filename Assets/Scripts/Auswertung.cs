using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Auswertung : MonoBehaviour
{
    public TMP_Text canvasText; // Textfeld auf dem Canvas

    private readonly float BeinlaengeRight = Probandendaten.Field3/10;
    private readonly float BeinlaengeLeft = Probandendaten.Field2/10;

    //VR Scene
    float CompositeScoreRechtsVR = 0f;
    float CompositeScoreLinksVR = 0f;

    private readonly float maxAntRightVR = Probandendaten.anteriorrechtsmaxVR;
    private readonly float maxPmRightVR = Probandendaten.pmrechtsmaxVR;
    private readonly float maxPlRightVR = Probandendaten.plrechtsmaxVR;
    
        
    private readonly float maxAntLeftVR = Probandendaten.anteriorlinksmaxVR;
    private readonly float maxPmLeftVR = Probandendaten.pmlinksmaxVR;
    private readonly float maxPlLeftVR = Probandendaten.pllinksmaxVR;

    //AR Scene
    float CompositeScoreRechtsAR = 0f;
    float CompositeScoreLinksAR = 0f;

    private readonly float maxAntRightAR = Probandendaten.anteriorrechtsmaxAR;
    private readonly float maxPmRightAR = Probandendaten.pmrechtsmaxAR;
    private readonly float maxPlRightAR = Probandendaten.plrechtsmaxAR;


    private readonly float maxAntLeftAR = Probandendaten.anteriorlinksmaxAR;
    private readonly float maxPmLeftAR = Probandendaten.pmlinksmaxAR;
    private readonly float maxPlLeftAR = Probandendaten.pllinksmaxAR;

    //VRHaptic Scene
    float CompositeScoreRechtsVRHaptic = 0f;
    float CompositeScoreLinksVRHaptic = 0f;

    private readonly float maxAntRightVRHaptic = Probandendaten.anteriorrechtsmaxVRHaptic;
    private readonly float maxPmRightVRHaptic = Probandendaten.pmrechtsmaxVRHaptic;
    private readonly float maxPlRightVRHaptic = Probandendaten.plrechtsmaxVRHaptic;


    private readonly float maxAntLeftVRHaptic = Probandendaten.anteriorlinksmaxVRHaptic;
    private readonly float maxPmLeftVRHaptic = Probandendaten.pmlinksmaxVRHaptic;
    private readonly float maxPlLeftVRHaptic = Probandendaten.pllinksmaxVRHaptic;


    // Start is called before the first frame update
    void Start()
    {
        CalculateCompositeScoreVR();
        CalculateCompositeScoreAR();
        CalculateCompositeScoreVRHaptic();
        PrintData();
        OnSaveButtonPressed();
    }

    private void CalculateCompositeScoreVR()
    {
        CompositeScoreRechtsVR = ((maxAntRightVR + maxPmRightVR + maxPlRightVR) / (3 * BeinlaengeRight))*100;
        Probandendaten.SetField(30, CompositeScoreRechtsVR);

        CompositeScoreLinksVR = ((maxAntLeftVR + maxPmLeftVR + maxPlLeftVR) / (3 * BeinlaengeLeft))*100;
        Probandendaten.SetField(31, CompositeScoreLinksVR);
    }
    private void CalculateCompositeScoreAR()
    {
        CompositeScoreRechtsAR = ((maxAntRightAR + maxPmRightAR + maxPlRightAR) / (3 * BeinlaengeRight))*100;
        Probandendaten.SetField(56, CompositeScoreRechtsAR);

        CompositeScoreLinksAR = ((maxAntLeftAR + maxPmLeftAR + maxPlLeftAR) / (3 * BeinlaengeLeft))*100;
        Probandendaten.SetField(57, CompositeScoreLinksAR);
    }
    private void CalculateCompositeScoreVRHaptic()
    {
        CompositeScoreRechtsVRHaptic = ((maxAntRightVRHaptic + maxPmRightVRHaptic + maxPlRightVRHaptic) / (3 * BeinlaengeRight))*100;
        Probandendaten.SetField(82, CompositeScoreRechtsVRHaptic);

        CompositeScoreLinksVRHaptic = ((maxAntLeftVRHaptic + maxPmLeftVRHaptic + maxPlLeftVRHaptic) / (3 * BeinlaengeLeft))*100;
        Probandendaten.SetField(83, CompositeScoreLinksVRHaptic);
    }
    private void PrintData()
    {
        //string maxAntRightStringVR = maxAntRightVR.ToString();
        //string maxPmRightStringVR = maxPmRightVR.ToString();
        //string maxPlRightStringVR = maxPlRightVR.ToString();
        //string maxAntLeftStringVR = maxAntLeftVR.ToString();
        //string maxPmLeftStringVR = maxPmLeftVR.ToString();
        //string maxPlLeftStringVR = maxPmLeftVR.ToString();
        string compScorerechtsVR = CompositeScoreRechtsVR.ToString("F1");
        string compScorelinksVR = CompositeScoreLinksVR.ToString("F1");
        string compScorerechtsAR = CompositeScoreRechtsAR.ToString("F1");
        string compScorelinksAR = CompositeScoreLinksAR.ToString("F1");
        string compScorerechtsVRHaptic = CompositeScoreRechtsVRHaptic.ToString("F1");
        string compScorelinksVRHaptic = CompositeScoreLinksVRHaptic.ToString("F1");

        canvasText.text = /*$"beste Reichweite:\n" +*/
            //$"- nach anterior (vorne) rechts = {maxAntRightStringVR}\n" +
            //$"- anterior links = {maxAntLeftStringVR}\n" +
            //$"- posteromedial (hinten mittig) rechts = {maxPmRightStringVR}\n" +
            //$"- posteromedial links = {maxPmLeftStringVR}\n" +
            //$"- posterolateral (hinten seitlich) rechts = {maxPlRightStringVR}\n" +
            //$"- posterolateral links = {maxPmLeftStringVR}\n\n" +
            $"Composite Score VR [%] rechts = {compScorerechtsVR}\n" +
            $"Composite Score VR [%] links = {compScorelinksVR}\n" +
            $"Composite Score AR [%] rechts = {compScorerechtsAR}\n" +
            $"Composite Score AR [%] links = {compScorelinksAR}\n" +
            $"Composite Score VRHaptic [%] rechts = {compScorerechtsVRHaptic}\n" +
            $"Composite Score VRHaptic [%] links = {compScorelinksVRHaptic}";
    }
    public void OnSaveButtonPressed()
    {
        ProbandendatenManager.Save();
    }
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("2. Szenenauswahl");
    }
}
