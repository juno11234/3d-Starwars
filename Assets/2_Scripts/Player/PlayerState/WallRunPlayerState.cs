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
        player.StartWallRun(wallNormal);
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
        
        // 벽면 법선과 up으로 달리기 방향 계산
        Vector3 forward = player.Model.forward;
        Vector3 runDir = Vector3.ProjectOnPlane(forward, wallNormal).normalized;

        // 모델 회전: 앞 방향은 runDir, up 방향은 wallNormal
        player.Model.rotation = Quaternion.LookRotation(runDir, wallNormal);

        // 이동
        player.Controller.Move(runDir * (wallRunSpeed * Time.deltaTime));

        // 벽을 벗어나면 MoveState로 복귀
        if (!player.WallDetector.IsTouchingWall(out _))
        {
            player.InitiateRotationRestore(originalRotation, 0.5f);
            player.ChangeState(new MovePlayerState(player), PlayerStateType.Move);
        }
    }

    public void Exit()
    {
        player.Model.rotation = originalRotation;
        player.StopWallRun_Or_Flying();
    }
}