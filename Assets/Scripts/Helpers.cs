using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;
using VRTK.SecondaryControllerGrabActions;

public enum LetterColor { Red, Blue, Yellow }

public static class Helpers {

    public static Color FromLetterColor(LetterColor color)
    {
        switch (color)
        {
            case LetterColor.Red: return Color.red;
            case LetterColor.Blue: return Color.blue;
            case LetterColor.Yellow: return Color.yellow;
            default:
                Debug.LogError("Missing color for: " + color);
                return Color.black;
        }
    }

    public static void SetStampColor(GameObject obj, LetterColor color)
    {
        var stamp = obj.transform.FindChild("Stamp");
        if (stamp == null)
            Debug.LogError(obj + " is missing Stamp");
        else
        {
            var stampMat = stamp.GetComponent<Renderer>();
            if (stampMat == null)
                Debug.LogError(obj + " is missing StampMaterial");
            else
                stampMat.material.color = Helpers.FromLetterColor(color);
        }
    }
}
