using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public class DummyTimer : MonoBehaviour
{
    public PlayerDirectionState PlayerDirectionState;
    public GameObject PlayerRailGameObject;
    
    public GameObject[] Poses;
    public float DelayAfterLookBack = 10f;
    public float Speed = 0.2f;
    public float DistanceAfterTeleport = 10f;
    public float TeleportBackDistance = 30f;
    public float TeleportTime = 1f;
    public AudioSource TeleportSound;

    private float _movingDelayTimer;
    private float _teleportCounter;
    private bool _isRotating;
    private bool _isLookingBack;
    private GameObject _activePose;
    
    private void OnEnable()
    {
        PlayerDirectionState.BeginRotating += OnBeginRotating;
        PlayerDirectionState.EndRotating += OnEndRotating;

        ChangePose(0);
    }

    private void OnBeginRotating(PlayerDirectionState.DirectionState from, PlayerDirectionState.DirectionState to)
    {
        _isRotating = true;
    }

    private void OnEndRotating(PlayerDirectionState.DirectionState updatedState)
    {
        _isRotating = false;
        _isLookingBack = updatedState == PlayerDirectionState.DirectionState.Back;
        if (_isLookingBack)
        {
            _movingDelayTimer = DelayAfterLookBack;
        }
    }

    public void Update()
    {
        if (_isRotating || _isLookingBack)
            return;

        if ((PlayerRailGameObject.transform.position - transform.position).sqrMagnitude > TeleportBackDistance * TeleportBackDistance)
        {
            var position = PlayerRailGameObject.gameObject.transform.position - PlayerRailGameObject.transform.forward * DistanceAfterTeleport;
            transform.position = new Vector3(0, 0, position.z);
        }
        
        if (_movingDelayTimer > 0f)
        {
            _movingDelayTimer -= Time.deltaTime;
            if (_movingDelayTimer > 0f)
                return;

            ChangePose(Random.Range(0, Poses.Length));
            _teleportCounter = TeleportTime;
        }
        else
        {
            _teleportCounter += Time.deltaTime;
        }
        
        if (_teleportCounter < TeleportTime)
            return;
        
        TeleportSound.PlayWithRandomPitch(0.75f, 1.25f);
        transform.position += PlayerRailGameObject.transform.forward * (Speed * _teleportCounter);
        _teleportCounter = 0.0f;
    }

    private void ChangePose(int posNumber)
    {
        if (_activePose != null)
            _activePose.SetActive(false);
        
        _activePose = Poses[posNumber];
        _activePose.SetActive(true);
    }
}
