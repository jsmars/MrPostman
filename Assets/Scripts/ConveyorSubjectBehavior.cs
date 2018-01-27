using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSubjectBehavior : MonoBehaviour
{
    private List<Transform> _enteredConveyorBelts = new List<Transform>();
    private Vector3 _currentVelocity = Vector3.zero;
    private Rigidbody _rigidBody;
    // Use this for initialization
    void Start()
    {
        _rigidBody = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enteredConveyorBelts.Count > 0)
        {
            _rigidBody.velocity = _currentVelocity;
        }
    }

    public void EnteredConveyorBelt(int constantSpeed, Transform beltTransform)
    {
        _enteredConveyorBelts.Add(beltTransform);
        if (!HasQueuedConveyorBelt())
        {
            MoveOnBelt(constantSpeed, beltTransform);
        }
    }

    public void ExitedConveyorBelt(int constantSpeed, Transform beltTransform)
    {
        var queuedBelt = HasQueuedConveyorBelt();
        _enteredConveyorBelts.Remove(beltTransform);

        if (queuedBelt)
        {
            MoveOnBelt(constantSpeed, _enteredConveyorBelts[0]);
        }
    }

    private bool HasQueuedConveyorBelt()
    {
        return _enteredConveyorBelts.Count > 1;
    }

    private void MoveOnBelt(int constantSpeed, Transform beltTransform)
    {
        var vector3 = beltTransform.right * -1.2f;
        _currentVelocity = constantSpeed * vector3;
    }
}
