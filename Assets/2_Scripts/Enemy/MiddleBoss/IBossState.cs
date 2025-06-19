using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState
{
    public void Enter();
    public void UpdateLogic();
    public void Exit();
}
