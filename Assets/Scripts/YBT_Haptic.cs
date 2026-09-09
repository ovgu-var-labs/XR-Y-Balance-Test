//Y-Balance-Test Code fuer die Masterarbeit von Alexander Schwadtke

using System.ComponentModel;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Wave.Essence;
using Wave.Native;

public class YBT_Haptic : MonoBehaviour
{
    //Referenzskripts
    public Collisiontestlinks collisiontestlinks;
    public Collisiontestrechts collisiontestrechts;
    
    //UI-Komponenten
    public TMP_Text canvasText; // Textfeld auf dem Canvas
    public TMP_Text weiteText;
    public TMP_Text versucheGueltigText; //Gültige Versuche = 3 benötigt, dann bester Wert
    public TMP_Text versucheUngueltigText; //Ungültige Versuhce = maximal 4, dann 0 Punkte
    public AudioSource audioSource; // AudioSource für die Sprachwiedergabe
    public AudioClip[] textAudioClipWeiter; // Liste der Audioclips für jeden Text
    public AudioClip audioClipUngueltig;
    public AudioClip audioClipGueltig;
    [TextArea(3, 10)] // Minimum 3 Zeilen, Maximum 10 Zeilen
    public string[] textMessagesWeiter; // Liste der Texte für die Weiter Phasen
    //public GameObject weiterButton; // Button für "Weiter"

    //Variablen 
    private int currentTextIndexWeiter = 0; //Index für den aktuellen Weiter-Schritt
    private int currentTextIndexStart = -1; //Index für den aktuellen Start-Schritt
    private int minVersucheGueltig = 0; //mindestens drei Versuche muessen gueltig sein pro Bewegunsgrichtung
    private int maxVersucheUngueltig = 0; //bei vier ungueltigen Versuchen wird die Bewegunsgrichtung mit der Punktzahl Null bewertet
    string gueltigeVersuche = "";
    string offeneFehlversuche = "";
    float failWert = 0f;
    //float xzWert_A = 0f;
    //float xWert_PM = 0f;
    //float zWert_PL = 0f;
    float PM_Wert = 0f;
    float Ant_Wert = 0f;
    float PL_Wert = 0f;

    //Statusueberpruefungen
    private bool isCurrentFootValid = false;
    private bool shouldMove_A = false;
    private bool shouldMove_PL = false;
    private bool shouldMove_PM = false;
    private bool listChecklinks = false;
    private bool listCheckrechts = false;

    //Objecte mit Collidern zur Positionsueberwachung
    public GameObject WaveRig;
    public GameObject Fusslinks;
    public Transform FussTrackerlinksGlobalTransform;
    public GameObject Fussrechts;
    public Transform FussTrackerrechtsGlobalTransform;
    public GameObject Standblock;
    public GameObject Standlinie;
    public GameObject Standlinie_Stop;
    public GameObject Barriere_SB_links;
    public GameObject Barriere_SB_rechts;
    public GameObject Schiebebalken_A;
    public GameObject Schiebebalken_PL;
    public GameObject Schiebebalken_PM;
    public GameObject Boden;
    public GameObject YBT;

    public Transform TrackerAntParent;
    public Transform TrackerPMParent;
    public Transform TrackerPLParent;

    private GameObject currentFootToCheck; // Der Fuß, der in der aktuellen Phase kontrolliert wird

    private GameObject currentFootToPlay; // Der Fuss, mit dem das Schiebeelement bewegt wird etc. 
    private static class ButtonFacade //Controller Button zordnung für Durchlauf gültig (A) oder ungültig (B)
    {
        public static bool AButtonPressed => //A = gültig für allright ""
            WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_A);
        public static bool BButtonPressed => //B = ungültig für bad ""
            WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_B);
        public static bool TriggerButtonPressed => //Trigger = weiter
        WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_Trigger);
    }
    //Start is called before the first frame update
    void Start()
    {
        //weiterButton.SetActive(true); //Button aktivieren
        currentTextIndexWeiter = 0;
        UpdateWeiterText(); //Initialen Text anzeigen
        //AdjustFootTransform(); //Skalierung der Füße anpassen 
    }

    private void Update()
    {        
        //CheckConditionFootlinks();
        //CheckConditionFootrechts();
        ActivateWeiterButton(); //Weiter Button in bestimmten Phasen aktivieren
        OnControllerButtonAPressed(); //Controllerbefeh für Versuch Gülltig
        OnControllerButtonBPressed(); //Controllerbefehl für Versuch Ungültig
        ShowSchiebebalkenWertOnCanva();

        if (ButtonFacade.TriggerButtonPressed)
        {
            
            if (currentTextIndexWeiter == 0)
            {
                OnWeiterButtonPressed();
            }
        }
        if(currentTextIndexWeiter == 1)
        {
            YBT.SetActive(false);
        }
    }

    //Fuss betrefende Funktionen WICHTIGE überlegungen: Tracker müsste ehr auf dem Fuß platziert werden. Wo?, Wie FussModell dazu in Relation setzen? 
    //private void AdjustFootTransform()
    //{
    //    float field4ValueLinks = Probandendaten.Field4;
    //    float field5ValueRechts = Probandendaten.Field5;

    //    if (Fusslinks != null)
    //    {
    //        // Skalierung berechnen
    //        Vector3 newScaleFusslinks = Fusslinks.transform.localScale;
    //        newScaleFusslinks *= field4ValueLinks * 0.001f; // Einheitliche Skalierung
    //        Fusslinks.transform.localScale = newScaleFusslinks;            
    //    }

    //    if (Fussrechts != null)
    //    {
    //        // Skalierung berechnen
    //        Vector3 newScaleFussrechts = Fussrechts.transform.localScale;
    //        newScaleFussrechts *= field5ValueRechts * 0.001f;
    //        Fussrechts.transform.localScale = newScaleFussrechts;
    //    }
    //}
    private void UpdateFootToCheck() //Teilde den Füßen ihre Rolle, in der aktuellen Phase zu
    {
        int[] validIndexes1 = { 0, 1, 3, 5 };
        int[] validIndexes2 = { 2, 4, 6 };
        //Testen ob Phase gerade oder ungerade ist
        if (validIndexes1.Contains(currentTextIndexWeiter)) //Beispiel: Phase 0, 1, 3, 5 = linker Fuß
        {
            currentFootToCheck = Fusslinks;
            currentFootToPlay = Fussrechts;
        }
        else if (validIndexes2.Contains(currentTextIndexWeiter))//Phase 2, 4 = rechter Fuß
        {
            currentFootToCheck = Fussrechts;
            currentFootToPlay = Fusslinks;
        }

        Renderer CurrentFootToCheckRenderer = currentFootToCheck.GetComponent<Renderer>();
        CurrentFootToCheckRenderer.material.SetColor("_BaseColor", Color.green);

        Renderer CurrentFootToPlayRenderer = currentFootToPlay.GetComponent<Renderer>();
        CurrentFootToPlayRenderer.material.SetColor("_BaseColor", Color.yellow);

        //Zurücksetzen des Kollisionsstatus für die neue Phase
        isCurrentFootValid = false;
    }

    //Schiebebalken betrefende Funktionen
    //private void DeactivateSchiebebalken() //Deaktivieren der nicht benötigten Schiebebalken = Phasenabhängig
    //{
    //    //Deaktivieren der nocht benötigten Schiebebalken in den jeweiligen Phasen
    //    if (currentTextIndexWeiter == 1 || currentTextIndexWeiter == 2) //Phase anterior
    //    {
    //        Schiebebalken_A.SetActive(true);
    //        Schiebebalken_PL.SetActive(false);
    //        Schiebebalken_PM.SetActive(false);
    //    }
    //    else if (currentTextIndexWeiter == 3 || currentTextIndexWeiter == 4) //Phase posteromedial
    //    {
    //        Schiebebalken_PM.SetActive(true);
    //        Schiebebalken_A.SetActive(false);
    //        Schiebebalken_PL.SetActive(false);

    //    }
    //    else if (currentTextIndexWeiter == 5 || currentTextIndexWeiter == 6)//Phase posterolateral
    //    {
    //        Schiebebalken_PL.SetActive(true);
    //        Schiebebalken_A.SetActive(false);
    //        Schiebebalken_PM.SetActive(false);
    //    }
    //}

    private void ShowSchiebebalkenWertOnCanva()
    {
        if (currentTextIndexWeiter == 1 || currentTextIndexWeiter == 2)
        {
            Ant_Wert = (Mathf.Sqrt(((TrackerAntParent.transform.localPosition.z) * (TrackerAntParent.transform.localPosition.z))
            + (TrackerAntParent.transform.localPosition.x) * (TrackerAntParent.transform.localPosition.x)) - 0.0675f) * 100; //Berechnen der Weite zu Anterior            
            string Coordinate_A = Ant_Wert.ToString("F1");
            weiteText.text = $"anterior:\n{Coordinate_A} cm";
            //weiteText.text = $"anterior:\n{Mathf.Abs((TrackerAntParent.transform.localPosition.z - 0.0675f) * 100):F1}cm";
        }
        else if (currentTextIndexWeiter == 3 || currentTextIndexWeiter == 4)
        {
            PM_Wert = (Mathf.Sqrt(((TrackerPMParent.transform.localPosition.z) * (TrackerPMParent.transform.localPosition.z))
            + (TrackerPMParent.transform.localPosition.x) * (TrackerPMParent.transform.localPosition.x)) - 0.0675f) * 100; //Berechnen der Weite zu Anterior            
            string Coordinate_PM = PM_Wert.ToString("F1");
            weiteText.text = $"postero-\nmedial:\n{Coordinate_PM} cm";
            //weiteText.text = $"postero-\nmedial:\n{Mathf.Abs((TrackerPMParent.transform.localPosition.z - 0.0675f) * 100):F1}cm";
        }
        else if (currentTextIndexWeiter == 5 || currentTextIndexWeiter == 6)
        {

            PL_Wert = (Mathf.Sqrt(((TrackerPLParent.transform.localPosition.z) * (TrackerPLParent.transform.localPosition.z))
            + (TrackerPLParent.transform.localPosition.x) * (TrackerPLParent.transform.localPosition.x)) - 0.0675f) * 100; //Berechnen der Weite zu Anterior            
            string Coordinate_PL = PL_Wert.ToString("F1");
            weiteText.text = $"postero-\nlateral:\n{Coordinate_PL} cm";
            //zWert_PL = (Mathf.Abs(Schiebebalken_PL.transform.localPosition.z) - 0.0675f) * 100;
            //string zCoordinate_PL = zWert_PL.ToString("F1");
            //weiteText.text = $"postero-\nlateral:\n{zCoordinate_PL} cm";
            //weiteText.text = $"postero-\nlaterl:\n{Mathf.Abs((TrackerPLParent.transform.localPosition.z - 0.0675f) * 100):F1}cm";
        }


    }
    //Buttonfunktionen
    public void OnWeiterButtonPressed()
    {
        
            if (audioSource.clip != audioClipGueltig || audioSource.clip != audioClipUngueltig)
            {
                audioSource.Stop(); //stoppt die Audio die aktuell noch spiel, falls Proband ehr weiter möchte
            }
            currentTextIndexWeiter++;
            if (currentTextIndexWeiter < 8)
            {
                UpdateWeiterText();
                UpdateFootToCheck(); // Zu prüfenden Fuß anpassen, wenn nicht Phase 0
                //DeactivateSchiebebalken();
                minVersucheGueltig = 0;
                maxVersucheUngueltig = 0;
                UpdateGueltigText();
                UpdateUngueltigText();
                //weiterButton.SetActive(false);
            }
            if (currentTextIndexWeiter == 7)
            {
                ProbandendatenManager.Save();
                SceneManager.LoadScene("2. Szenenauswahl");
            }
        
        
    }
    public void OnControllerButtonAPressed() //Versuchsleiter basierte bestätigung der Versuchsgültigkeit über rechten Controller button A
    {
        if (ButtonFacade.AButtonPressed)
        {
            minVersucheGueltig++;
            UpdateGueltigText();
            SaveTrialDataGueltigVRHaptic();
            if (audioSource != null)
            {
                audioSource.clip = audioClipGueltig;
                audioSource.Play();
            }
        }


    }
    public void OnControllerButtonBPressed() //Versuchsleiter basierte bestätigung der Versuchsungültigkeit über rechten Controller button B
    {
        if (ButtonFacade.BButtonPressed)
        {
            maxVersucheUngueltig++;
            UpdateUngueltigText();
            SaveTrialDataUngueltigVRHaptic();
            if (audioSource != null)
            {
                audioSource.clip = audioClipUngueltig;
                audioSource.Play();
            }
        }


    }
    private void UpdateWeiterText()
    {
        if (canvasText != null && currentTextIndexWeiter < textMessagesWeiter.Length)
        {
            canvasText.text = textMessagesWeiter[currentTextIndexWeiter];

            if (audioSource != null && textAudioClipWeiter != null && currentTextIndexWeiter < textAudioClipWeiter.Length)
            {
                audioSource.Stop();
                audioSource.clip = textAudioClipWeiter[currentTextIndexWeiter];
                audioSource.Play();
            }
        }
    } //Updtate Textfeld wenn Weiter Button gedrückt wird    
    private void UpdateGueltigText()
    {
        if (versucheGueltigText != null)
        {
            gueltigeVersuche = minVersucheGueltig.ToString();
            versucheGueltigText.text = $"Gültige Versuche: {gueltigeVersuche}";
        }
    } //Update Textfeld für gültige Versuche
    private void UpdateUngueltigText()
    {
        if (versucheUngueltigText != null)
        {
            offeneFehlversuche = maxVersucheUngueltig.ToString();
            versucheUngueltigText.text = $"Ungültige Versuche: {offeneFehlversuche}";
        }
    } //Update Textfeld für ungültige Versuche
    private void ActivateWeiterButton()
    {
        if (minVersucheGueltig == 3 || maxVersucheUngueltig == 4)
        {
            OnWeiterButtonPressed();
        }
    } //WeiterButton in benötigten Phasen aktivieren

    //Speichern der Daten in die Probandendaten-Liste
    private void SaveTrialDataGueltigVRHaptic() // Funktion zum Speichern der Weiten-Daten, der gueltigen Versuche
    {
        if (currentTextIndexWeiter == 1) // Phase anterior Standbein links, Spielbein rechts
        {
           
           float currentValue = Probandendaten.anteriorrechtsmaxVRHaptic;
            if (currentValue < Ant_Wert)
            {
                Probandendaten.SetField(58, Ant_Wert);
            }
            if (minVersucheGueltig == 1)
            {
                Probandendaten.SetField(64, Ant_Wert);
            }
            else if (minVersucheGueltig == 2)
            {
                Probandendaten.SetField(65, Ant_Wert);
            }
            else if (minVersucheGueltig == 3)
            {
                Probandendaten.SetField(66, Ant_Wert);
            }
        }
        else if (currentTextIndexWeiter == 2) // Phase anterior Standbein rechts, Spielbein links
        {
            float currentValue = Probandendaten.anteriorlinksmaxVRHaptic;
            if (currentValue < Ant_Wert)
            {
                Probandendaten.SetField(59, Ant_Wert);
            }
            if (minVersucheGueltig == 1)
            {
                Probandendaten.SetField(67, Ant_Wert);
            }
            else if (minVersucheGueltig == 2)
            {
                Probandendaten.SetField(68, Ant_Wert);
            }
            else if (minVersucheGueltig == 3)
            {
                Probandendaten.SetField(69, Ant_Wert);
            }
        }
        else if (currentTextIndexWeiter == 3) // Phase posteromedial Standbein links, Spielbein rechts
        {
            float currentValue = Probandendaten.pmrechtsmaxVRHaptic;
            if (currentValue < PM_Wert)
            {
                Probandendaten.SetField(60, PM_Wert);
            }
            if (minVersucheGueltig == 1)
            {
                Probandendaten.SetField(70, PM_Wert);
            }
            else if (minVersucheGueltig == 2)
            {
                Probandendaten.SetField(71, PM_Wert);
            }
            else if (minVersucheGueltig == 3)
            {
                Probandendaten.SetField(72, PM_Wert);
            }
        }
        else if (currentTextIndexWeiter == 4) // Phase posteromedial Standbein rechts, Spielbein links
        {
            float currentValue = Probandendaten.pmlinksmaxVRHaptic;
            if (currentValue < PM_Wert)
            {
                Probandendaten.SetField(61, PM_Wert);
            }
            if (minVersucheGueltig == 1)
            {
                Probandendaten.SetField(73, PM_Wert);
            }
            else if (minVersucheGueltig == 2)
            {
                Probandendaten.SetField(74, PM_Wert);
            }
            else if (minVersucheGueltig == 3)
            {
                Probandendaten.SetField(75, PM_Wert);
            }
        }
        else if (currentTextIndexWeiter == 5) // Phase posterolateral Standbein links, Spielbein rechts
        {
            float currentValue = Probandendaten.plrechtsmaxVRHaptic;
            if (currentValue < PL_Wert)
            {
                Probandendaten.SetField(62, PL_Wert);
            }
            if (minVersucheGueltig == 1)
            {
                Probandendaten.SetField(76, PL_Wert);
            }
            else if (minVersucheGueltig == 2)
            {
                Probandendaten.SetField(77, PL_Wert);
            }
            else if (minVersucheGueltig == 3)
            {
                Probandendaten.SetField(78, PL_Wert);
            }
        }
        else if (currentTextIndexWeiter == 6) // Phase posterolateral Standbein rechts, Spielbein links
        {
            float currentValue = Probandendaten.pllinksmaxVRHaptic;
            if (currentValue < PL_Wert)
            {
                Probandendaten.SetField(63, PL_Wert);
            }
            if (minVersucheGueltig == 1)
            {
                Probandendaten.SetField(79, PL_Wert);
            }
            else if (minVersucheGueltig == 2)
            {
                Probandendaten.SetField(80, PL_Wert);
            }
            else if (minVersucheGueltig == 3)
            {
                Probandendaten.SetField(81, PL_Wert);
            }
        }
    }
    private void SaveTrialDataUngueltigVRHaptic() // Funktion zum Speichern der Weiten-Daten, der ungueltigen Versuche
    {
        if (currentTextIndexWeiter == 1 && maxVersucheUngueltig == 4)
        {
            Probandendaten.SetField(58, failWert);
        }
        else if (currentTextIndexWeiter == 2 && maxVersucheUngueltig == 4)
        {
            Probandendaten.SetField(59, failWert);
        }
        else if (currentTextIndexWeiter == 3 && maxVersucheUngueltig == 4)
        {
            Probandendaten.SetField(60, failWert);
        }
        else if (currentTextIndexWeiter == 4 && maxVersucheUngueltig == 4)
        {
            Probandendaten.SetField(61, failWert);
        }
        else if (currentTextIndexWeiter == 5 && maxVersucheUngueltig == 4)
        {
            Probandendaten.SetField(62, failWert);
        }
        else if (currentTextIndexWeiter == 6 && maxVersucheUngueltig == 4)
        {
            Probandendaten.SetField(63, failWert);
        }
    }

    //Kollisionsevents linsk
    //public void CollisionEnterEventStandblocklinks(Collider other)
    //{
    //    if (currentFootToCheck == null) return;

    //    //Prüfen, ob der Fuß den StaSndblock berührt
    //    if (listChecklinks == true)
    //    {
    //        //Debug.Log($"Kollision erkannt: {currentFootToCheck.name} berührt {other.gameObject.name}");
    //        isCurrentFootValid = true;
    //        //startButton.SetActive(true);
    //        Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
    //        standblockRenderer.material.SetColor("_StripeColour", Color.white);
    //    }
    //    CheckConditionFootlinks();

    //}
    //public void CollisionExitEventStandblocklinks(Collider other)
    //{
    //    if (currentFootToCheck == null) return;

    //    //Prüfen, ob der Fuß den Standblock verlässt
    //    if (!collisiontestlinks.collidingObjectslinks.Contains(Standblock) || !collisiontestlinks.collidingObjectslinks.Contains(Standlinie))
    //    {
    //        //Debug.Log($"Kollision beendet: {currentFootToCheck.name} verlässt {other.gameObject.name}");
    //        isCurrentFootValid = false;
    //        //startButton.SetActive(false);
    //        Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
    //        standblockRenderer.material.SetColor("_StripeColour", Color.yellow);
    //    }
    //}
    //private void CheckConditionFootlinks()
    //{
    //    if (collisiontestlinks.collidingObjectslinks.Contains(Standlinie_Stop) || collisiontestlinks.collidingObjectslinks.Contains(Barriere_SB_links) || collisiontestlinks.collidingObjectslinks.Contains(Barriere_SB_rechts))
    //    {
    //        listChecklinks = false;
    //    }
    //    else if (collisiontestlinks.collidingObjectslinks.Contains(Standblock) && collisiontestlinks.collidingObjectslinks.Contains(Standlinie))
    //    {
    //        listChecklinks = true;
    //    }
    //}

    //Kollisionsevents rechts
    //public void CollisionEnterEventStandblockrechts(Collider other)
    //{
    //    if (currentFootToCheck == null) return;

    //    //Prüfen, ob der Fuß den StaSndblock berührt
    //    if (listCheckrechts == true)
    //    {
    //        //Debug.Log($"Kollision erkannt: {currentFootToCheck.name} berührt {other.gameObject.name}");
    //        isCurrentFootValid = true;
    //        //startButton.SetActive(true);
    //        Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
    //        standblockRenderer.material.SetColor("_StripeColour", Color.white);
    //    }
    //    CheckConditionFootrechts();

    //}
    //public void CollisionExitEventStandblockrechts(Collider other)
    //{
    //    if (currentFootToCheck == null) return;

    //    //Prüfen, ob der Fuß den Standblock verlässt
    //    if (!collisiontestrechts.collidingObjectsrechts.Contains(Standblock) || !collisiontestrechts.collidingObjectsrechts.Contains(Standlinie))
    //    {
    //        //Debug.Log($"Kollision beendet: {currentFootToCheck.name} verlässt {other.gameObject.name}");
    //        isCurrentFootValid = false;
    //        //startButton.SetActive(false);
    //        Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
    //        standblockRenderer.material.SetColor("_StripeColour", Color.yellow);
    //    }
    //}
    //private void CheckConditionFootrechts()
    //{
    //    if (collisiontestrechts.collidingObjectsrechts.Contains(Standlinie_Stop) || collisiontestrechts.collidingObjectsrechts.Contains(Barriere_SB_links) || collisiontestrechts.collidingObjectsrechts.Contains(Barriere_SB_rechts))
    //    {
    //        listCheckrechts = false;
    //    }
    //    else if (collisiontestrechts.collidingObjectsrechts.Contains(Standblock) && collisiontestrechts.collidingObjectsrechts.Contains(Standlinie))
    //    {
    //        listCheckrechts = true;
    //    }
    //}

    //Kollisionsevents allgemein
    //public void CollisionEventExitSchiebeelement(Collider other)
    //{
    //    if (other.gameObject.name.Contains("Schiebebalken"))
    //    {
    //        Debug.Log($"Kollision erkannt: {currentFootToPlay.name} verlässt {other.gameObject.name}");
    //        if (other.gameObject == Schiebebalken_A)
    //        {
    //            shouldMove_A = false;
    //        }
    //        else if (other.gameObject == Schiebebalken_PL)
    //        {
    //            shouldMove_PL = false;
    //        }
    //        else { shouldMove_PM = false; }
    //    }

    //}
    //public void CollisionEventEnterSchiebeelement(Collider other)
    //{
    //    if (other.gameObject.name.Contains("Schiebebalken"))
    //    {
    //        Debug.Log($"Kollision erkannt: {currentFootToPlay.name} berührt {other.gameObject.name}");
    //        if (other.gameObject == Schiebebalken_A)
    //        {
    //            shouldMove_A = true;
    //        }
    //        else if (other.gameObject == Schiebebalken_PL)
    //        {
    //            shouldMove_PL = true;
    //        }
    //        else { shouldMove_PM = true; }
    //    }
        
    //}
    
}
