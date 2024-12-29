using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CorridorDecalSpawnerManager : MonoBehaviour
{
    private const float GizmoWidth = 0.05f;
    private enum DecalOnCorridorGeneration { None = 0, Right = 1, Left = 2, All = 3 }
    
    public CorridorDecalsScriptableObject CorridorDecals;
    public DecalProjector DecalProjectorPrefab;
    public float MaxRandomRotation = 10.0f;
    public bool ShowGizmo = false;
    
    public DecalSquare DecalSquaresRight;
    public DecalSquare DecalSquaresLeft;

    public void Awake()
    {
        var decalOnCorridorGeneration = (DecalOnCorridorGeneration)Random.Range(0, 4);
        switch (decalOnCorridorGeneration)
        {
            case DecalOnCorridorGeneration.Right:
                InstantiateDecalAtRandomPosition(DecalSquaresRight);
                break;
            case DecalOnCorridorGeneration.Left:
                InstantiateDecalAtRandomPosition(DecalSquaresLeft);
                break;
            case DecalOnCorridorGeneration.All:
                InstantiateDecalAtRandomPosition(DecalSquaresRight);
                InstantiateDecalAtRandomPosition(DecalSquaresLeft);
                break;
        }
    }

    private void InstantiateDecalAtRandomPosition(DecalSquare decalSquares)
    {
        var decalProjector = Instantiate(DecalProjectorPrefab, decalSquares.Position);
        decalProjector.material = CorridorDecals.DecalsMaterials[Random.Range(0, CorridorDecals.DecalsMaterials.Length)];
        
        var halfWidth = (decalSquares.Size.x - decalSquares.DecalSize) * 0.5f;
        var halfHeight = (decalSquares.Size.y - decalSquares.DecalSize) * 0.5f;
        decalProjector.transform.localPosition = new Vector3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0);
        
        decalProjector.transform.Rotate(0f, 0f, Random.Range(-MaxRandomRotation, MaxRandomRotation));
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmo)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawCube(DecalSquaresRight.Position.transform.position, new Vector3(GizmoWidth, DecalSquaresRight.Size.y, DecalSquaresRight.Size.x));
        Gizmos.DrawCube(DecalSquaresLeft.Position.transform.position, new Vector3(GizmoWidth, DecalSquaresLeft.Size.y, DecalSquaresLeft.Size.x));
    }

    [Serializable]
    public struct DecalSquare
    {
        public Transform Position;
        public Vector2 Size;
        public float DecalSize;
    }
}