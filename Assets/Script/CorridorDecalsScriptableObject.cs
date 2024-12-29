using UnityEngine;

[CreateAssetMenu(fileName = "CorridorDecals", menuName = "ScriptableObjects/CorridorDecals", order = 1)]
public class CorridorDecalsScriptableObject : ScriptableObject
{
    public Material[] DecalsMaterials;
}