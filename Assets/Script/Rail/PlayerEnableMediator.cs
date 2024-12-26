using System;
using Script.Rail;
using UnityEngine;

public class PlayerEnableMediator : MonoBehaviour
{
    public PlayerDirectionState PlayerDirectionState;
    public PlayerForwardMove ForwardMove;
    public PlayerOpenDoor OpenDoor;

    public void Awake()
    {
        PlayerDirectionState.enabled = true;
        ForwardMove.enabled = true;
        OpenDoor.enabled = false;
    }

    public void OnEnable()
    {
        PlayerDirectionState.enabled = true;
        ForwardMove.enabled = true;
        OpenDoor.enabled = false;

        PlayerDirectionState.BeginRotating += OnBeginRotating;
        PlayerDirectionState.EndRotating += OnEndRotating;
        ForwardMove.BeginRailEvent += OnBeginRailEvent;
        ForwardMove.EndRailEvent += OnEndRailEvent;
        OpenDoor.BeginOpening += OnBeginOpening;
        OpenDoor.EndOpening += OnEndOpening;
    }

    private void OnBeginRotating(PlayerDirectionState.DirectionState from, PlayerDirectionState.DirectionState to)
    {
        ForwardMove.enabled = false;
        OpenDoor.enabled = false;
    }

    private void OnEndRotating(PlayerDirectionState.DirectionState updatedState)
    {
        switch (updatedState)
        {
            case PlayerDirectionState.DirectionState.Forward:
                ForwardMove.enabled = true;
                break;
            case PlayerDirectionState.DirectionState.Left:
            case PlayerDirectionState.DirectionState.Right:
                OpenDoor.enabled = true;
                break;
        }
    }

    private void OnBeginRailEvent()
    {
        PlayerDirectionState.enabled = false;
    }

    private void OnEndRailEvent()
    {
        PlayerDirectionState.enabled = true;
    }

    private void OnBeginOpening()
    {
        PlayerDirectionState.enabled = false;
    }

    private void OnEndOpening()
    {
        PlayerDirectionState.enabled = true;
    }
}