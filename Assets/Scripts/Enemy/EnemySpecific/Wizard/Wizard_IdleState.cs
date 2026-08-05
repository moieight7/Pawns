public class Wizard_IdleState : IdleState
{
    private Wizard enemy;

    public Wizard_IdleState() { }

    public Wizard_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Wizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (enemy.CheckDanger() && enemy.abilities.FindAbilityByIndex(2).numberOfCharges > 0)
        {
            enemy.abilities.TriggerAbilityByIndex(2);
        }
        else if (enemy.CheckPlayerMaxRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            if (enemy.abilities.FindAbilityByIndex(1).numberOfCharges > 0) enemy.abilities.TriggerAbilityByIndex(1, enemy.abilities.entityAbilities[1].EnemyCastDelayTime);
            else if (enemy.abilities.FindAbilityByIndex(0).numberOfCharges > 0) enemy.abilities.TriggerAbilityByIndex(0, enemy.abilities.entityAbilities[0].EnemyCastDelayTime);
        }
        else stateMachine.ChangeState(enemy.MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
