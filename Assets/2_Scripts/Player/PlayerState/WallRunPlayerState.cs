using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunPlayerState : IPlayerState
{
    private PlayerStateMachine player;
    private Vector3 wallNormal;
    private float wallRunSpeed;
    private Quaternion originalRotation;

    public WallRunPlayerState(PlayerStateMachine p, Vector3 wallNormal)
    {
        player = p;
        this.wallNormal = wallNormal;
        wallRunSpeed = player.runSpeed;
    }

    public void Enter()
    {
        originalRotation = player.Model.rotation;
        player.StartWallRun();
        player.ResetJumpCount();
    }

    public void Input()
    {
        if (player.JumpInput)
        {
            player.ChangeState(new WallJumpPlayerState(player, wallNormal), PlayerStateType.WallJump);
        }
    }

    public void UpdateLogic()
    {
        player.UsePortal(player);
        // 외적 
        Vector3 moveDir = Vector3.Cross(wallNormal, Vector3.up).normalized;
        // 방향 조절
        if (Vector3.Dot(moveDir, player.Model.forward) < 0) moveDir = -moveDir;
        // 모델 회전
        player.Model.rotation = Quaternion.LookRotation(moveDir, wallNormal);
        // 이동
        player.Controller.Move(moveDir * (wallRunSpeed * Time.deltaTime));

        // 벽을 벗어나면 MoveState로 복귀
        if (player.WallDetector.IsTouchingWall(out _) == false)
        {
            player.ChangeState(new WallJumpPlayerState(player, wallNormal), PlayerStateType.WallJump);
        }
    }

    public void Exit()
    {
        player.Model.rotation = originalRotation;
        player.StopWallRun_Or_Flying();
    }
}