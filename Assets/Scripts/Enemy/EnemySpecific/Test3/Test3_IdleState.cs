public class Test3_IdleState : IdleState
{
    private Test3 enemy;

    public Test3_IdleState() { }

    public Test3_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Test3 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        enemy.navMeshAgent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.CheckDanger() && enemy.abilities.FindAbilityByIndex(2).numberOfCharges > 0 && !enemy.isDashing)
        {
            enemy.abilities.TriggerAbilityByIndex(2);
            stateMachine.ChangeState(enemy.DashState);
        }
        else if (enemy.CheckPlayerMaxRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            enemy.abilities.TriggerAbilityByIndex(0);
        }
        else stateMachine.ChangeState(enemy.MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
