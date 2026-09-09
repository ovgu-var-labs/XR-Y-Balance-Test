//Probandendaten Code fuer die Masterarbeit von Alexander Schwadtke

using UnityEngine;


public static class Probandendaten
{
    public static float Field0 = 0f; //Probandennummer
    public static float Field1 = 0f; //Testeinheit
    public static float Field2 = 0f; //Beinl‰nge links
    public static float Field3 = 0f; //Beinl‰nge rechts
    public static float Field4 = 0f; //Fuﬂl‰nge links
    public static float Field5 = 0f; //Fuﬂl‰nge rechts

    //Speicherdaten VR-Scene
    public static float anteriorrechtsmaxVR = 0f; //Bester Wert anterior rechts
    public static float anteriorlinksmaxVR = 0f; //Bester Wert anterior links
    public static float pmrechtsmaxVR = 0f; //Bester Wert posteromedial rechts
    public static float pmlinksmaxVR = 0f; //Bester Wert posteromedial links
    public static float plrechtsmaxVR = 0f; //Bester Wert posterolateral rechts
    public static float pllinksmaxVR = 0f; //Bester Wert posterolateral links
    
    public static float antrechts1VR = 0f;
    public static float antrechts2VR = 0f;
    public static float antrechts3VR = 0f;
    
    public static float antlinks1VR = 0f;
    public static float antlinks2VR = 0f;
    public static float antlinks3VR = 0f;
    
    public static float pmrechts1VR = 0f;
    public static float pmrechts2VR = 0f;
    public static float pmrechts3VR = 0f;
    
    public static float pmlinks1VR = 0f;
    public static float pmlinks2VR = 0f;
    public static float pmlinks3VR = 0f;
   
    public static float plrechts1VR = 0f;
    public static float plrechts2VR = 0f;
    public static float plrechts3VR = 0f;
    
    public static float pllinks1VR = 0f;
    public static float pllinks2VR = 0f;
    public static float pllinks3VR = 0f;
   
    public static float ComposeiteScoreRightFieldVR = 0f; //Composite Score rechts in %
    public static float ComposeiteScoreLeftFieldVR = 0f; //Composite Score links in %

    //Speicherdaten AR-Scene
    public static float anteriorrechtsmaxAR = 0f; //Bester Wert anterior rechts
    public static float anteriorlinksmaxAR = 0f; //Bester Wert anterior links
    public static float pmrechtsmaxAR = 0f; //Bester Wert posteromedial rechts
    public static float pmlinksmaxAR = 0f; //Bester Wert posteromedial links
    public static float plrechtsmaxAR = 0f; //Bester Wert posterolateral rechts
    public static float pllinksmaxAR = 0f; //Bester Wert posterolateral links

    public static float antrechts1AR = 0f;
    public static float antrechts2AR = 0f;
    public static float antrechts3AR = 0f;

    public static float antlinks1AR = 0f;
    public static float antlinks2AR = 0f;
    public static float antlinks3AR = 0f;

    public static float pmrechts1AR = 0f;
    public static float pmrechts2AR = 0f;
    public static float pmrechts3AR = 0f;

    public static float pmlinks1AR = 0f;
    public static float pmlinks2AR = 0f;
    public static float pmlinks3AR = 0f;

    public static float plrechts1AR = 0f;
    public static float plrechts2AR = 0f;
    public static float plrechts3AR = 0f;

    public static float pllinks1AR = 0f;
    public static float pllinks2AR = 0f;
    public static float pllinks3AR = 0f;

    public static float ComposeiteScoreRightFieldAR = 0f; //Composite Score rechts in %
    public static float ComposeiteScoreLeftFieldAR = 0f; //Composite Score links in %

    //Speicherdaten VRHaptic-Scene
    public static float anteriorrechtsmaxVRHaptic = 0f; //Bester Wert anterior rechts
    public static float anteriorlinksmaxVRHaptic = 0f; //Bester Wert anterior links
    public static float pmrechtsmaxVRHaptic = 0f; //Bester Wert posteromedial rechts
    public static float pmlinksmaxVRHaptic = 0f; //Bester Wert posteromedial links
    public static float plrechtsmaxVRHaptic = 0f; //Bester Wert posterolateral rechts
    public static float pllinksmaxVRHaptic = 0f; //Bester Wert posterolateral links

    public static float antrechts1VRHaptic = 0f;
    public static float antrechts2VRHaptic = 0f;
    public static float antrechts3VRHaptic = 0f;

    public static float antlinks1VRHaptic = 0f;
    public static float antlinks2VRHaptic = 0f;
    public static float antlinks3VRHaptic = 0f;

    public static float pmrechts1VRHaptic = 0f;
    public static float pmrechts2VRHaptic = 0f;
    public static float pmrechts3VRHaptic = 0f;

    public static float pmlinks1VRHaptic = 0f;
    public static float pmlinks2VRHaptic = 0f;
    public static float pmlinks3VRHaptic = 0f;

    public static float plrechts1VRHaptic = 0f;
    public static float plrechts2VRHaptic = 0f;
    public static float plrechts3VRHaptic = 0f;

    public static float pllinks1VRHaptic = 0f;
    public static float pllinks2VRHaptic = 0f;
    public static float pllinks3VRHaptic = 0f;

    public static float ComposeiteScoreRightFieldVRHaptic = 0f; //Composite Score rechts in %
    public static float ComposeiteScoreLeftFieldVRHaptic = 0f; //Composite Score links in %

    public static void SetField(int fieldIndex, float value)
    {
        float roundedValue = Mathf.Round(value * 2f) / 2f; // Rundet auf eine Dezimalstelle
        switch (fieldIndex)
        {
            case 0: Field0 = value; break;
            case 1: Field1 = value; break;
            case 2: Field2 = value; break;
            case 3: Field3 = value; break;
            case 4: Field4 = value; break;
            case 5: Field5 = value; break;

            case 6: anteriorrechtsmaxVR = roundedValue; break;
            case 7: anteriorlinksmaxVR = roundedValue; break;
            case 8: pmrechtsmaxVR = roundedValue; break;
            case 9: pmlinksmaxVR = roundedValue; break;
            case 10: plrechtsmaxVR = roundedValue; break;
            case 11: pllinksmaxVR = roundedValue; break;

            case 12: antrechts1VR = roundedValue; break;
            case 13: antrechts2VR = roundedValue; break;
            case 14: antrechts3VR = roundedValue; break;
            case 15: antlinks1VR = roundedValue; break;
            case 16: antlinks2VR = roundedValue; break;
            case 17: antlinks3VR = roundedValue; break;

            case 18: pmrechts1VR = roundedValue; break;
            case 19: pmrechts2VR = roundedValue; break;
            case 20: pmrechts3VR = roundedValue; break;
            case 21: pmlinks1VR = roundedValue; break;
            case 22: pmlinks2VR = roundedValue; break;
            case 23: pmlinks3VR = roundedValue; break;

            case 24: plrechts1VR = roundedValue; break;
            case 25: plrechts2VR = roundedValue; break;
            case 26: plrechts3VR = roundedValue; break;
            case 27: pllinks1VR = roundedValue; break;
            case 28: pllinks2VR = roundedValue; break;
            case 29: pllinks3VR = roundedValue; break;

            case 30: ComposeiteScoreRightFieldVR = value; break;
            case 31: ComposeiteScoreLeftFieldVR = value; break;

            case 32: anteriorrechtsmaxAR = roundedValue; break;
            case 33: anteriorlinksmaxAR = roundedValue; break;
            case 34: pmrechtsmaxAR = roundedValue; break;
            case 35: pmlinksmaxAR = roundedValue; break;
            case 36: plrechtsmaxAR = roundedValue; break;
            case 37: pllinksmaxAR = roundedValue; break;

            case 38: antrechts1AR = roundedValue; break;
            case 39: antrechts2AR = roundedValue; break;
            case 40: antrechts3AR = roundedValue; break;
            case 41: antlinks1AR = roundedValue; break;
            case 42: antlinks2AR = roundedValue; break;
            case 43: antlinks3AR = roundedValue; break;

            case 44: pmrechts1AR = roundedValue; break;
            case 45: pmrechts2AR = roundedValue; break;
            case 46: pmrechts3AR = roundedValue; break;
            case 47: pmlinks1AR = roundedValue ; break;
            case 48: pmlinks2AR = roundedValue ; break;
            case 49: pmlinks3AR = roundedValue; break;

            case 50: plrechts1AR = roundedValue; break;
            case 51: plrechts2AR = roundedValue; break;
            case 52: plrechts3AR = roundedValue; break;
            case 53: pllinks1AR = roundedValue; break;
            case 54: pllinks2AR = roundedValue; break;
            case 55: pllinks3AR = roundedValue; break;

            case 56: ComposeiteScoreRightFieldAR = value; break;
            case 57: ComposeiteScoreLeftFieldAR = value; break;

            case 58: anteriorrechtsmaxVRHaptic = roundedValue; break;
            case 59: anteriorlinksmaxVRHaptic = roundedValue; break;
            case 60: pmrechtsmaxVRHaptic = roundedValue; break;
            case 61: pmlinksmaxVRHaptic = roundedValue; break;
            case 62: plrechtsmaxVRHaptic = roundedValue; break;
            case 63: pllinksmaxVRHaptic = roundedValue; break;

            case 64: antrechts1VRHaptic = roundedValue; break;
            case 65: antrechts2VRHaptic = roundedValue; break;
            case 66: antrechts3VRHaptic = roundedValue; break;
            case 67: antlinks1VRHaptic = roundedValue; break;
            case 68: antlinks2VRHaptic = roundedValue; break;
            case 69: antlinks3VRHaptic = roundedValue; break;

            case 70: pmrechts1VRHaptic = roundedValue; break;
            case 71: pmrechts2VRHaptic = roundedValue; break;
            case 72: pmrechts3VRHaptic = roundedValue; break;
            case 73: pmlinks1VRHaptic = roundedValue; break;
            case 74: pmlinks2VRHaptic = roundedValue; break;
            case 75: pmlinks3VRHaptic = roundedValue; break;

            case 76: plrechts1VRHaptic = roundedValue; break;
            case 77: plrechts2VRHaptic = roundedValue; break;
            case 78: plrechts3VRHaptic = roundedValue; break;
            case 79: pllinks1VRHaptic = roundedValue; break;
            case 80: pllinks2VRHaptic = roundedValue; break;
            case 81: pllinks3VRHaptic = roundedValue; break;

            case 82: ComposeiteScoreRightFieldVRHaptic = value; break;
            case 83: ComposeiteScoreLeftFieldVRHaptic = value; break;

        }
    }
}