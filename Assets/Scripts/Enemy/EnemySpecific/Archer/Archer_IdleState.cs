public class Archer_IdleState : IdleState
{
    private Archer enemy;

    public Archer_IdleState() { }

    public Archer_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (enemy.CheckPlayerMaxRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            if (enemy.abilities.FindAbilityByIndex(1).numberOfCharges > 0) enemy.abilities.TriggerAbilityByIndex(1);
            enemy.abilities.TriggerAbilityByIndex(0);
        }
        else stateMachine.ChangeState(enemy.MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
