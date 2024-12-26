using System;
using UnityEngine;

public class CorridorDoor : MonoBehaviour
{
    public Door Door;
    public void Open(Action onFinish)
    {
        Door.Open(onFinish);
    }
}