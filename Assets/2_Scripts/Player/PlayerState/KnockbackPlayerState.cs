using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayerState : IPlayerState
{
    private PlayerStateMachine player;
    private float knockbackTime = 2f;
    private float timer = 0f;
    private float knockbakcPower = 10f;
    private Vector3 knockbackDir = Vector3.zero;

    public KnockbackPlayerState(PlayerStateMachine player, Vector3 knockbackDir)
    {
        this.player = player;
        this.knockbackDir = knockbackDir;
    }

    public void Enter()
    {
        player.Animator.SetBool("Knockback", true);
    }

    public void Input() { }

    public void UpdateLogic()
    {
        player.Controller.Move(knockbackDir * (knockbakcPower * Time.deltaTime));

        timer += Time.deltaTime;
        if (timer > knockbackTime)
        {
            player.ChangeState(new MovePlayerState(player), PlayerStateType.Move);
        }
    }

    public void Exit()
    {
        player.Animator.SetBool("Knockback", false);
    }
}