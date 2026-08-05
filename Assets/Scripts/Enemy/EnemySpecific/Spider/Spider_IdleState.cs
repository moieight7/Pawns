public class Spider_IdleState : IdleState
{
    private Spider enemy;

    public Spider_IdleState() { }

    public Spider_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Spider enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        else if (enemy.CheckPlayerMinRange() && enemy.abilities.FindAbilityByIndex(1).numberOfCharges > 0 && enemy.CanSeePlayerWithClearLineOfSight)
        {
            enemy.abilities.TriggerAbilityByIndex(1, enemy.abilities.entityAbilities[1].EnemyCastDelayTime);
        }
        else if (enemy.CheckPlayerMinRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            enemy.abilities.TriggerAbilityByIndex(0, enemy.abilities.entityAbilities[0].EnemyCastDelayTime);
        }
        else stateMachine.ChangeState(enemy.MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
