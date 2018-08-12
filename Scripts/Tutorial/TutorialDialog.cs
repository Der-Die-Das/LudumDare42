using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialDialog
{
    [TextArea(3, 10)]
    public string Text;
    public TutorialMarker[] Markers;

    public bool SpawnCar = false;
    public bool WaitForServiceCompleted = false;
    public bool WaitForDepartureCompleted = false;
    public bool WaitForSpotBought = false;

    public void Show()
    {
        if (Markers != null && Markers.Length > 0)
        {
            foreach (var marker in Markers)
            {
                marker.gameObject.SetActive(true);
            }
        }
    }

    public void Hide()
    {
        if (Markers != null && Markers.Length > 0)
        {
            foreach (var marker in Markers)
            {
                marker.gameObject.SetActive(false);
            }
        }
    }

}
