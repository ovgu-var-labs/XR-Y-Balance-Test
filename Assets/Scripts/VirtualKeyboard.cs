using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; //wenn später der save button gedrückt wurde soll automatisch in die y-balance test scene gesprungen werden. Unten vorbereitet!

public class VirtualKeyboard : MonoBehaviour
{
    //public TMP_InputField[] inputFields; // Array der InputFields
    public TMP_InputField Testeinheit;
    public TMP_InputField Probandennummer;
    public TMP_InputField Beinlinks;
    public TMP_InputField Beinrechts;
    public TMP_InputField Fusslinks;
    public TMP_InputField Fussrechts;
    private TMP_InputField activeInputField; // Aktives InputField

    string inputText;
    int index;
    //string character;

    void Start()
    {
        SetActiveInputField();
    }

    private void SetActiveInputField()
    {
        if (index == 0)
        {
            activeInputField = Probandennummer;
            Probandennummer.Select();
            Probandennummer.ActivateInputField();
            Beinlinks.DeactivateInputField();
            Beinrechts.DeactivateInputField();
            Fussrechts.DeactivateInputField();
            Fusslinks.DeactivateInputField();
        }
        else if (index == 1)
        {
            activeInputField = Testeinheit;
            Testeinheit.Select();
            Beinlinks.DeactivateInputField();
            Probandennummer.DeactivateInputField();
            Beinrechts.DeactivateInputField();
            Fussrechts.DeactivateInputField();
            Fusslinks.DeactivateInputField();
        }
        else if (index == 2)
        {
            activeInputField = Beinlinks;
            Beinlinks.Select();
            Beinlinks.ActivateInputField();
            Probandennummer.DeactivateInputField();
            Beinrechts.DeactivateInputField();
            Fussrechts.DeactivateInputField();
            Fusslinks.DeactivateInputField();
        }
        else if (index == 3)
        {
            activeInputField = Beinrechts;
            Beinrechts.Select();
            Beinrechts.ActivateInputField();
            Probandennummer.DeactivateInputField();
            Fussrechts.DeactivateInputField();
            Fusslinks.DeactivateInputField();
        }
        else if (index == 4)
        {
            activeInputField = Fusslinks;
            Fusslinks.Select();
            Fusslinks.ActivateInputField();
            Probandennummer.DeactivateInputField();
            Beinrechts.DeactivateInputField();
            Fussrechts.DeactivateInputField();
        }
        else
        {
            activeInputField = Fussrechts;
            Fussrechts.Select();
            Fussrechts.ActivateInputField();
            Probandennummer.DeactivateInputField();
            Beinrechts.DeactivateInputField();
            Fusslinks.DeactivateInputField();
        }
    }

    // Füge Text in das aktive InputField ein
    public void InsertText(string character)
    {
        Debug.Log($"InsertText aufgerufen mit Zeichen: {character}");
        if (index == 0)
        {
            Probandennummer.text += character;
            Probandennummer.ActivateInputField();
        }
        else if (index == 1)
        {
            Testeinheit.text += character;
            Testeinheit.ActivateInputField();
        }
        else if (index == 2)
        {
            Beinlinks.text += character;
            Beinlinks.ActivateInputField();
        }
        else if (index == 3)
        {
            Beinrechts.text += character;
            Beinrechts.ActivateInputField();
        }
        else if (index == 4)
        {
            Fusslinks.text += character;
            Fusslinks.ActivateInputField();
        }
        else if(index == 5)
        {
            Fussrechts.text += character;
            Fussrechts.ActivateInputField();
        }
    }

    public void RemoveLastCharacter()
    {
        if (activeInputField != null && activeInputField.text.Length > 0)
        {
            activeInputField.text = activeInputField.text.Substring(0, activeInputField.text.Length - 1);
            activeInputField.ActivateInputField();
        }
    }

    // Speichere Eingaben als Floats in die InputFieldData-Klasse
    public void SaveInputFields()
    {
        
        if (index == 0)
        {
            inputText = Probandennummer.text;
        }
        else if (index == 1)
        {
            inputText = Testeinheit.text;
        }
        else if (index == 2)
        {
            inputText = Beinlinks.text;
        }
        else if (index == 3)
        {
            inputText = Beinrechts.text;
        }
        else if (index == 4)
        {
            inputText = Fusslinks.text;
        }
        else if (index == 5)
        {
            inputText = Fussrechts.text;
            SceneManager.LoadScene("2. Szenenauswahl"); //In die Y-Balance Test Scene springen, sobal die Daten gespeichert wurden
        }
        
        // Versuche, die Texte der InputFields direkt in Floats zu speichern
        if (float.TryParse(inputText, out float parsedValue))
        {
            // Speichere den Wert basierend auf dem Index
            Probandendaten.SetField(index, parsedValue); // Index 1-basiert
        }
        else
        {
            Debug.LogError($"Eingabe im Feld {index} konnte nicht in Float konvertiert werden: {inputText}");
        }

        index++;
        SetActiveInputField();
    }

    

}