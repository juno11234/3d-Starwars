using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunState : IPlayerState
{
    private PlayerStateMachine player;
    private Vector3 wallNormal;
    private float wallRunSpeed;

    public WallRunState(PlayerStateMachine p, Vector3 wallNormal)
    {
        player = p;
        this.wallNormal = wallNormal;
        wallRunSpeed = player.runSpeed;
    }

    public void Enter()
    {
        Debug.Log("벽달리기 진입");
        player.StartWallRun(wallNormal);
    }

    public void Input()
    {
        // if(player.JumpInput)
        //     player.ChangeState(new WallJumpState(player,wallNormal),PlayerStateType.WallJump);
    }

    public void UpdateLogic()
    {
        Vector3 runDir = player.Model.forward;
        player.Controller.Move(runDir * (wallRunSpeed * Time.deltaTime));
        
        if (player.WallDetector.IsTouchingWall(out _)==false)
            player.ChangeState(new MoveState(player), PlayerStateType.Move);
    }

    public void Exit()
    {
        player.StopWallRun();
    }
}