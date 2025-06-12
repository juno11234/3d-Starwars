using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    private PlayerStateMachine player;

    public AttackState(PlayerStateMachine player)
    {
        this.player = player;
    }
    
    public void Enter() { }
    public void Input() { }
    public void UpdateLogic() { }
    public void Exit() { }
}