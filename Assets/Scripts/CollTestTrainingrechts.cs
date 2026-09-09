using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollTestTrainingrechts : MonoBehaviour
{
    public YBT_Training y_Balance_Test;

    [SerializeField]
    public List<GameObject> collidingObjectsrechts1 = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggeredObject = other.gameObject;
        if (!collidingObjectsrechts1.Contains(triggeredObject))
        {
            collidingObjectsrechts1.Add(triggeredObject);
            Debug.Log("Objekt hinzugefügt: " + triggeredObject.name);
        }
        print("Collisiontrigger_Enter");

        y_Balance_Test.CollisionEnterEventStandblockrechts(other);
        y_Balance_Test.CollisionEventEnterSchiebeelement(other);


    }
    private void OnTriggerExit(Collider other)
    {
        GameObject exitedObject = other.gameObject;
        if (collidingObjectsrechts1.Contains(exitedObject))
        {
            collidingObjectsrechts1.Remove(exitedObject);
            Debug.Log("Objekt entfernt: " + exitedObject.name);
        }
        print("Collisiontrigger_Exit");
        y_Balance_Test.CollisionExitEventStandblockrechts(other);
        y_Balance_Test.CollisionEventExitSchiebeelement(other);

    }
}
