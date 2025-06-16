using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : IPlayerState
{
    private PlayerStateMachine player;
    private Quaternion originalRotation;
    private Vector3 slopeNormal;

    public SlidingState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public void Enter()
    {
        originalRotation = player.Model.rotation;
        player.Animator.SetTrigger("Slide");
        player.Animator.SetFloat("Speed", 0f);
        player.jumpCount = 0;
        player.SlidingSpeedLine.SetActive(true);
    }

    public void Input() { }

    public void UpdateLogic()
    {
        if (player.SlidingTrigger == false)
        {
            player.ChangeState(new JumpState(player), PlayerStateType.Sliding);
        }

        UpdateSlopeNormal();
        Vector3 downDir = Vector3.ProjectOnPlane(Vector3.down, slopeNormal).normalized;
        Vector3 move = downDir.normalized * (player.slidingSpeed * Time.deltaTime);
        player.Controller.Move(move);
        player.SlidingMove();


        player.Model.rotation = player.SlidingTransform.rotation;
    }

    public void Exit()
    {
        player.SlidingTrigger = false;
        player.Animator.ResetTrigger("Slide");
        player.SlidingSpeedLine.SetActive(false);
    }

    private void UpdateSlopeNormal()
    {
        // 캐릭터 바로 아래에서 레이캐스트로 경사면 법선 구하기
        if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 1.5f,
                ~(1 << LayerMask.NameToLayer("Player"))))
        {
            slopeNormal = hit.normal;
        }
        else
        {
            slopeNormal = Vector3.up;
        }
    }
}