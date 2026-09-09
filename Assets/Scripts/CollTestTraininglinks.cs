using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollTestTraininglinks : MonoBehaviour
{
    public YBT_Training y_Balance_Test;

    [SerializeField]
    public List<GameObject> collidingObjectslinks1 = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggeredObject = other.gameObject;
        if (!collidingObjectslinks1.Contains(triggeredObject))
        {
            collidingObjectslinks1.Add(triggeredObject);
            Debug.Log("Objekt hinzugefügt: " + triggeredObject.name);
        }
        print("Collisiontrigger_Enter");

        y_Balance_Test.CollisionEnterEventStandblocklinks(other);
        y_Balance_Test.CollisionEventEnterSchiebeelement(other);


    }
    private void OnTriggerExit(Collider other)
    {
        GameObject exitedObject = other.gameObject;
        if (collidingObjectslinks1.Contains(exitedObject))
        {
            collidingObjectslinks1.Remove(exitedObject);
            Debug.Log("Objekt entfernt: " + exitedObject.name);
        }
        print("Collisiontrigger_Exit");
        y_Balance_Test.CollisionExitEventStandblocklinks(other);
        y_Balance_Test.CollisionEventExitSchiebeelement(other);

    }
}
