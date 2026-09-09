using System.ComponentModel;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Wave.Essence;
using Wave.Native;

public class YBT_Training : MonoBehaviour
{
    //Referenzskripts
    public CollTestTraininglinks collisiontestlinks1;
    public CollTestTrainingrechts collisiontestrechts1;

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
    float xzWert_A = 0f;
    float xWert_PM = 0f;
    float zWert_PL = 0f;
    float failWert = 0f;
    string gueltigeVersuche = "";
    string offeneFehlversuche = "";


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

    private GameObject currentFootToCheck; // Der Fuß, der in der aktuellen Phase kontrolliert wird

    private GameObject currentFootToPlay; // Der Fuss, mit dem das Schiebeelement bewegt wird etc. 
    private static class ButtonFacade //Controller Button zordnung für Durchlauf gültig (A) oder ungültig (B)
    {
        public static bool AButtonPressed => //A = gültig für allright ""
            WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_A);
        public static bool BButtonPressed => //B = ungültig für bad ""
            WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_B);
        public static bool TriggerButtonPressed => //rechter Trigger = weiter
            WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_Trigger);
    }

    //Start is called before the first frame update
    void Start()
    {
        currentTextIndexWeiter = 0;
        UpdateWeiterText(); //Initialen Text anzeigen
        AdjustFootTransform(); //Skalierung der Füße anpassen 
    }

    private void Update()
    {
        ShouldMoveCheck(); //Schiebebalken Bewegungsinteraktion
        CheckConditionFootlinks();
        CheckConditionFootrechts();
        ActivateWeiterButton(); //Weiter Button in bestimmten Phasen aktivieren
        OnControllerButtonAPressed(); //Controllerbefeh für Versuch Gülltig
        OnControllerButtonBPressed(); //Controllerbefehl für Versuch Ungültig

        if (ButtonFacade.TriggerButtonPressed)
        {
            if (currentTextIndexWeiter == 0)
            {
                OnWeiterButtonPressed();
            }
        }

    }

    //Fuss betrefende Funktionen WICHTIGE überlegungen: Tracker müsste ehr auf dem Fuß platziert werden. Wo?, Wie FussModell dazu in Relation setzen? 
    private void AdjustFootTransform()
    {
        float field4ValueLinks = Probandendaten.Field4;
        float field5ValueRechts = Probandendaten.Field5;

        if (Fusslinks != null)
        {
            // Skalierung berechnen
            Vector3 newScaleFusslinks = Fusslinks.transform.localScale;
            newScaleFusslinks *= field4ValueLinks * 0.001f; // Einheitliche Skalierung
            Fusslinks.transform.localScale = newScaleFusslinks;

        }

        if (Fussrechts != null)
        {
            // Skalierung berechnen
            Vector3 newScaleFussrechts = Fussrechts.transform.localScale;
            newScaleFussrechts *= field5ValueRechts * 0.001f;
            Fussrechts.transform.localScale = newScaleFussrechts;
        }
    }
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
    private void DeactivateSchiebebalken() //Deaktivieren der nicht benötigten Schiebebalken = Phasenabhängig
    {
        //Deaktivieren der nocht benötigten Schiebebalken in den jeweiligen Phasen
        if (currentTextIndexWeiter == 1 || currentTextIndexWeiter == 2) //Phase anterior
        {
            Schiebebalken_A.SetActive(true);
            Schiebebalken_PL.SetActive(false);
            Schiebebalken_PM.SetActive(false);
        }
        else if (currentTextIndexWeiter == 3 || currentTextIndexWeiter == 4) //Phase posteromedial
        {
            Schiebebalken_PM.SetActive(true);
            Schiebebalken_A.SetActive(false);
            Schiebebalken_PL.SetActive(false);

        }
        else if (currentTextIndexWeiter == 5 || currentTextIndexWeiter == 6)//Phase posterolateral
        {
            Schiebebalken_PL.SetActive(true);
            Schiebebalken_A.SetActive(false);
            Schiebebalken_PM.SetActive(false);
        }
    }
    private void ResetSchiebebalken() //Zurücksetzen der Schiebebalken nach einem Versuch
    {
        if (Schiebebalken_A != null)
        {
            Schiebebalken_A.transform.localPosition = new Vector3(0.15f, Schiebebalken_A.transform.localPosition.y, -0.15f);
        }
        if (Schiebebalken_PM != null)
        {
            Schiebebalken_PM.transform.localPosition = new Vector3(-0.3f, Schiebebalken_PM.transform.localPosition.y, Schiebebalken_PM.transform.localPosition.z);
        }
        if (Schiebebalken_PL != null)
        {
            Schiebebalken_PL.transform.localPosition = new Vector3(Schiebebalken_PL.transform.localPosition.x, Schiebebalken_PL.transform.localPosition.y, 0.3f);
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
            DeactivateSchiebebalken();
            minVersucheGueltig = 0;
            maxVersucheUngueltig = 0;

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
            audioSource.clip = audioClipGueltig;
            audioSource.Play();
            ResetSchiebebalken();
        }
    }
    public void OnControllerButtonBPressed() //Versuchsleiter basierte bestätigung der Versuchsungültigkeit über rechten Controller button B
    {
        if (ButtonFacade.BButtonPressed)
        {
            maxVersucheUngueltig++;
            audioSource.clip = audioClipUngueltig;
            audioSource.Play();
            ResetSchiebebalken();

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
   
    private void ActivateWeiterButton()
    {
        if (minVersucheGueltig == 3 || maxVersucheUngueltig == 4)
        {
            OnWeiterButtonPressed();
        }
    } //WeiterButton in benötigten Phasen aktivieren

    
    

    //Kollisionsevents linsk
    public void CollisionEnterEventStandblocklinks(Collider other)
    {
        if (currentFootToCheck == null) return;

        //Prüfen, ob der Fuß den StaSndblock berührt
        if (listChecklinks == true)
        {
            isCurrentFootValid = true;
            Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
            standblockRenderer.material.SetColor("_StripeColour", Color.white);
        }
        CheckConditionFootlinks();

    }
    public void CollisionExitEventStandblocklinks(Collider other)
    {
        if (currentFootToCheck == null) return;

        //Prüfen, ob der Fuß den Standblock verlässt
        if (!collisiontestlinks1.collidingObjectslinks1.Contains(Standblock) || !collisiontestlinks1.collidingObjectslinks1.Contains(Standlinie))
        {
            isCurrentFootValid = false;
            Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
            standblockRenderer.material.SetColor("_StripeColour", Color.yellow);
        }
    }
    private void CheckConditionFootlinks()
    {
        if (collisiontestlinks1.collidingObjectslinks1.Contains(Standlinie_Stop) || collisiontestlinks1.collidingObjectslinks1.Contains(Barriere_SB_links) || collisiontestlinks1.collidingObjectslinks1.Contains(Barriere_SB_rechts))
        {
            listChecklinks = false;
        }
        else if (collisiontestlinks1.collidingObjectslinks1.Contains(Standblock) && collisiontestlinks1.collidingObjectslinks1.Contains(Standlinie))
        {
            listChecklinks = true;
        }
    }

    //Kollisionsevents rechts
    public void CollisionEnterEventStandblockrechts(Collider other)
    {
        if (currentFootToCheck == null) return;

        //Prüfen, ob der Fuß den StaSndblock berührt
        if (listCheckrechts == true)
        {
            isCurrentFootValid = true;
            Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
            standblockRenderer.material.SetColor("_StripeColour", Color.white);
        }
        CheckConditionFootrechts();

    }
    public void CollisionExitEventStandblockrechts(Collider other)
    {
        if (currentFootToCheck == null) return;

        //Prüfen, ob der Fuß den Standblock verlässt
        if (!collisiontestrechts1.collidingObjectsrechts1.Contains(Standblock) || !collisiontestrechts1.collidingObjectsrechts1.Contains(Standlinie))
        {
            isCurrentFootValid = false;
            Renderer standblockRenderer = Standblock.GetComponent<Renderer>();
            standblockRenderer.material.SetColor("_StripeColour", Color.yellow);
        }
    }
    private void CheckConditionFootrechts()
    {
        if (collisiontestrechts1.collidingObjectsrechts1.Contains(Standlinie_Stop) || collisiontestrechts1.collidingObjectsrechts1.Contains(Barriere_SB_links) || collisiontestrechts1.collidingObjectsrechts1.Contains(Barriere_SB_rechts))
        {
            listCheckrechts = false;
        }
        else if (collisiontestrechts1.collidingObjectsrechts1.Contains(Standblock) && collisiontestrechts1.collidingObjectsrechts1.Contains(Standlinie))
        {
            listCheckrechts = true;
        }
    }

    //Kollisionsevents allgemein
    public void CollisionEventExitSchiebeelement(Collider other)
    {
        if (other.gameObject.name.Contains("Schiebebalken"))
        {
            Debug.Log($"Kollision erkannt: {currentFootToPlay.name} verlässt {other.gameObject.name}");
            if (other.gameObject == Schiebebalken_A)
            {
                shouldMove_A = false;
            }
            else if (other.gameObject == Schiebebalken_PL)
            {
                shouldMove_PL = false;
            }
            else { shouldMove_PM = false; }
        }

    }
    public void CollisionEventEnterSchiebeelement(Collider other)
    {
        if (other.gameObject.name.Contains("Schiebebalken"))
        {
            Debug.Log($"Kollision erkannt: {currentFootToPlay.name} berührt {other.gameObject.name}");
            if (other.gameObject == Schiebebalken_A)
            {
                shouldMove_A = true;
            }
            else if (other.gameObject == Schiebebalken_PL)
            {
                shouldMove_PL = true;
            }
            else { shouldMove_PM = true; }
        }
    }
    private void ShouldMoveCheck()
    {
        //bewegen der Schiebebalken anterior
        if (shouldMove_A == true)
        {
            Schiebebalken_A.transform.Translate(Vector3.forward * 0.01f);
            xzWert_A = (Mathf.Sqrt(((Schiebebalken_A.transform.localPosition.z) * (Schiebebalken_A.transform.localPosition.z))
                + (Schiebebalken_A.transform.localPosition.x) * (Schiebebalken_A.transform.localPosition.x)) - 0.0675f) * 100; //Berechnen der Weite zu Anterior
            string xzCoordinate_A = xzWert_A.ToString("F1");
            weiteText.text = $"anterior:\n{xzCoordinate_A} cm";
        }
        else
        {
            Schiebebalken_A.transform.Translate(Vector3.forward * 0);

        }
        //bewegen der Schiebebalken posteromedial
        if (shouldMove_PM == true)
        {
            Schiebebalken_PM.transform.Translate(Vector3.forward * 0.01f);
            xWert_PM = (Mathf.Abs(Schiebebalken_PM.transform.localPosition.x) - 0.0675f) * 100;
            string xCoordinate_PM = xWert_PM.ToString("F1");
            weiteText.text = $"postero-\nmedial:\n{xCoordinate_PM} cm";
        }
        else
        {
            Schiebebalken_PM.transform.Translate(Vector3.forward * 0);

        }
        //bewegen der Schiebebalken posterolateral
        if (shouldMove_PL == true)
        {
            Schiebebalken_PL.transform.Translate(Vector3.forward * 0.01f);
            zWert_PL = (Mathf.Abs(Schiebebalken_PL.transform.localPosition.z) - 0.0675f) * 100;
            string zCoordinate_PL = zWert_PL.ToString("F1");
            weiteText.text = $"postero-\nlateral:\n{zCoordinate_PL} cm";
        }
        else
        {
            Schiebebalken_PL.transform.Translate(Vector3.forward * 0);

        }
    }
}
