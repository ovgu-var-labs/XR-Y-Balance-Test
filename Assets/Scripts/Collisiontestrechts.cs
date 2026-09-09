//Collisionstest Code fuer die Masterarbeit von Alexander Schwadtke

using System.Collections.Generic;
using UnityEngine;

public class Collisiontestrechts : MonoBehaviour
{

    public Y_Balance_Test y_Balance_Test;

    [SerializeField]
    public List<GameObject> collidingObjectsrechts = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggeredObject = other.gameObject;
        if (!collidingObjectsrechts.Contains(triggeredObject))
        {
            collidingObjectsrechts.Add(triggeredObject);
            Debug.Log("Objekt hinzugefügt: " + triggeredObject.name);
        }
        print("Collisiontrigger_Enter");

        y_Balance_Test.CollisionEnterEventStandblockrechts(other);
        y_Balance_Test.CollisionEventEnterSchiebeelement(other);


    }
    private void OnTriggerExit(Collider other)
    {
        GameObject exitedObject = other.gameObject;
        if (collidingObjectsrechts.Contains(exitedObject))
        {
            collidingObjectsrechts.Remove(exitedObject);
            Debug.Log("Objekt entfernt: " + exitedObject.name);
        }
        print("Collisiontrigger_Exit");
        y_Balance_Test.CollisionExitEventStandblockrechts(other);
        y_Balance_Test.CollisionEventExitSchiebeelement(other);

    }
}
