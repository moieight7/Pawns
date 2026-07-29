using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggerState : State, IEnemyState
{
    D_StaggerState stateData;

    public StaggerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StaggerState stateData) : base(entity, stateMachine, animBoolName)
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
