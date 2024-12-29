using Script;
using UnityEngine;
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

    // TODO: Improve
    public PlayerEnableMediator Player;
    public DummyTimer DummyTimer;

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
    }

    private void GenerateEndRoom(int blocksCount)
    {
        var position = StarBlockPosition.position;

        var endRoomOffset = EndRoomOffsetPoint.position.z - position.z;
        position.AddZ(BlockLength * blocksCount + endRoomOffset);

        var endRoom = Instantiate(EndRoomPrefab, position.AddZ(BlockLength * blocksCount - endRoomOffset), Quaternion.Euler(0, 180, 0));
        endRoom.GetComponent<YouWinCutsceneOnDoorOpen>().Init(Player, DummyTimer);
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

            DoorOpeningEvent leftDoor = corridorBlock.LeftDoor.GetComponent<DoorOpeningEvent>();
            DoorOpeningEvent rightDoor = corridorBlock.RightDoor.GetComponent<DoorOpeningEvent>();
            if (i + 1 != blocksCount)
            {
                GenerateRandomEventDoorValues(leftDoor);
                GenerateRandomEventDoorValues(rightDoor);
            }
            else
            {
                leftDoor.Door.IsLocked = true;
                rightDoor.Door.IsLocked = true;
            }
        }
    }

    private static void GenerateRandomEventDoorValues(DoorOpeningEvent randomEventDoor)
    {
        // TODO: REWRITE!
        var randomValue = Random.Range(0, 4);
        if (randomValue == 3)
            randomEventDoor.Door.IsLocked = true;
        else
            randomEventDoor.EventType = (DoorOpeningEvent.OpeningEventType)randomValue;
        
    }
}
