using System;
using Script;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CorridorGenerator : MonoBehaviour
{
    public GameObject CorridorBlockPrefab;
    public Transform StarBlockPosition;
    public Transform StarBlockEndPosition;
    public Transform EndRoomOffsetPoint;
    public GameObject EndRoomPrefab;
    
    public DoorNumber StartDoor;
    public CorridorBlock StartCorridorBlock;

    public int MinBlocksCount = 30;
    public int MaxBlocksCount = 50;
    
    private void Awake()
    {
        var blocksCount = Random.Range(MinBlocksCount, MaxBlocksCount);
        var doorCount = blocksCount * 2 + 1;
        
        StartDoor.SetNumber(doorCount);
        StartCorridorBlock.RightDoor.SetNumber(doorCount - 1);
        StartCorridorBlock.LeftDoor.SetNumber(doorCount - 2);
        
        GenerateCorridorBoxes(blocksCount);
        
        GenerateEndRoom(blocksCount);
        
        GenerateDecorationDecals();
    }

    private void GenerateEndRoom(int blocksCount)
    {
        var position = StarBlockPosition.position;
        
        var endRoomOffset = EndRoomOffsetPoint.position.z - position.z;
        position.AddZ(BlockLength * blocksCount + endRoomOffset);
        
        Instantiate(EndRoomPrefab, position.AddZ(BlockLength * blocksCount - endRoomOffset), Quaternion.Euler(0, 180, 0));
    }

    public float BlockLength => StarBlockEndPosition.transform.position.z - StarBlockPosition.transform.position.z;

    void GenerateCorridorBoxes(int blocksCount)
    {
        for (int i = 1; i < blocksCount; i++)
        {
            var generatedBlock = Instantiate(CorridorBlockPrefab, StarBlockPosition.position.AddZ(BlockLength * i), Quaternion.identity);
            CorridorBlock corridorBlock = generatedBlock.GetComponent<CorridorBlock>();
            int number = (blocksCount - i) * 2 - 1;
            corridorBlock.RightDoor.SetNumber(number);
            corridorBlock.LeftDoor.SetNumber(number + 1);
        }
    }

    void GenerateDecorationDecals()
    {
        
    }

    void GenerateEventOn()
    {
        
    }
}
