using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : IPlayerState
{
    private PlayerStateMachine player;
    private List<Transform> waypoints;
    private int index;
    private float flySpeed = 20f;
    private bool thisTimeLook=false;

    public FlyingState(PlayerStateMachine player, List<Transform> waypoints)
    {
        this.player = player;
        this.waypoints = waypoints;
    }

    public void Enter()
    {
        player.Animator.SetTrigger("Flying");
        player.jumpCount = 0;
        player.WarpSpeedLine.SetActive(true);
    }

    public void Input() { }

    public void UpdateLogic()
    {
        if (waypoints == null || waypoints.Count == 0) return;
        Transform target = waypoints[index];
        Vector3 dir = (target.position - player.transform.position).normalized;

        player.Controller.Move(dir * (flySpeed * Time.deltaTime));

        player.Model.rotation =
            Quaternion.Slerp(player.Model.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);

        if (Vector3.Distance(player.transform.position, target.position) < 0.2f)
        {
            index++;
            thisTimeLook = true;
            if (index >= waypoints.Count)
                player.ChangeState(new JumpState(player), PlayerStateType.Jump);
        }
    }

    public void Exit()
    {
        player.StopWallRun_Or_Flying();
        player.WarpSpeedLine.SetActive(false);
    }
}