using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void Input();
    void UpdateLogic();
    void Exit();
}