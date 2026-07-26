using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State, IEnemyState
{
    protected D_AttackState stateData;

    protected bool animFinish;
    protected bool playerDetectedInMinRange;
    protected bool playerDetectedInMaxRange;

    public AttackState() { }

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_AttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Checks()
    {
        base.Checks();

        playerDetectedInMinRange = entity.CheckPlayerMinRange();
        playerDetectedInMaxRange = entity.CheckPlayerMaxRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.atsm.attackState = this;
        animFinish = false;
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

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        animFinish = true;
    }
}
