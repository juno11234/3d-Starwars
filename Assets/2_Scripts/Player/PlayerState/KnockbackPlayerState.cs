using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayerState : IPlayerState
{
    private PlayerStateMachine player;

    public KnockbackPlayerState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        
    }

    public void Input()
    {
    }

    public void UpdateLogic()
    {
    }

    public void Exit()
    {
    }
}