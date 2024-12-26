using Script.Rail;
using UnityEngine;

public class DoorRailEventAdapter : MonoBehaviour, IRailEvent
{
    public Door Door;
    
    public bool IsPerformed => Door.IsOpened;
    
    public void Perform()
    {
        Door.Open();
    }
}