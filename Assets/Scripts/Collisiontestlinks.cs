//Collisionstest Code fuer die Masterarbeit von Alexander Schwadtke

using System.Collections.Generic;
using UnityEngine;

public class Collisiontestlinks : MonoBehaviour
{

    public Y_Balance_Test y_Balance_Test;

    [SerializeField]
    public List<GameObject> collidingObjectslinks = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggeredObject = other.gameObject;
        if (!collidingObjectslinks.Contains(triggeredObject))
        {
            collidingObjectslinks.Add(triggeredObject);
            Debug.Log("Objekt hinzugefügt: " + triggeredObject.name);
        }
        print("Collisiontrigger_Enter");
        
        y_Balance_Test.CollisionEnterEventStandblocklinks(other);
        y_Balance_Test.CollisionEventEnterSchiebeelement(other);


    }
    private void OnTriggerExit(Collider other)
    {
        GameObject exitedObject = other.gameObject;
        if (collidingObjectslinks.Contains(exitedObject))
        {
            collidingObjectslinks.Remove(exitedObject);
            Debug.Log("Objekt entfernt: " + exitedObject.name);
        }
        print("Collisiontrigger_Exit");
        y_Balance_Test.CollisionExitEventStandblocklinks(other);
        y_Balance_Test.CollisionEventExitSchiebeelement(other);

    }
}
