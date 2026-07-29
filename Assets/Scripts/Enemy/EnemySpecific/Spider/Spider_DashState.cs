using UnityEngine;

public class Spider_DashState : DashState
{
    private Spider enemy;

    public Spider_DashState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DashState stateData, Spider enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
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

        if (!enemy.isDashing) stateMachine.ChangeState(enemy.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
