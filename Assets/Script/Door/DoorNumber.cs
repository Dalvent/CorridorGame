using TMPro;
using UnityEngine;

public class DoorNumber : MonoBehaviour
{
    public TextMeshPro Text;

    public void SetNumber(int number)
    {
        Text.text = number.ToString();
    }
}