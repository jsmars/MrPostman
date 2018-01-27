using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBelt : MonoBehaviour
{
    public int ConstantSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private ConveyorSubjectBehavior ValidateCollisionObject(Collider collider) 
    {
        var isConveyorSubject = collider.tag == "ConveyorSubject";
        if (!isConveyorSubject)
        {
            return null;
        }

        var conveyorSubjectScript = collider.GetComponent<ConveyorSubjectBehavior>();
        if (conveyorSubjectScript == null)
        {
            throw new Exception("THIS IS SO WRONG");
        }

        return conveyorSubjectScript;
    }

    void OnCollisionEnter(Collision collision)
    {
        var conveyorSubjectScript = ValidateCollisionObject(collision.collider);
        if (conveyorSubjectScript != null)
        {
            conveyorSubjectScript.EnteredConveyorBelt(ConstantSpeed, transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var conveyorSubjectScript = ValidateCollisionObject(collision.collider);
        if (conveyorSubjectScript != null)
        {
            conveyorSubjectScript.ExitedConveyorBelt(ConstantSpeed, transform);
        }
    }
}
