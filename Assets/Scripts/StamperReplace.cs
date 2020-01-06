using UnityEngine;

public class StamperReplace : MonoBehaviour
{
    public Transform ReplaceToPosition;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Stamper")
        {
            other.transform.rotation = ReplaceToPosition.rotation;
            other.transform.position = ReplaceToPosition.position;
        }
    }
}
