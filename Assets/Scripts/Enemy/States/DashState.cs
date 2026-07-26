using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : State, IEnemyState
{
    protected D_DashState stateData;

    protected bool isDashOver;

    public DashState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DashState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Checks()
    {
        base.Checks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
