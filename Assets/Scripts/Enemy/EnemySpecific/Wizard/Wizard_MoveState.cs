public class Wizard_MoveState : MoveState
{
    private Wizard enemy;

    public Wizard_MoveState() { }

    public Wizard_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Wizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.speed = entity.entityData.navMeshAgentMovementSpeed;
        enemy.navMeshAgent.acceleration = stateData.acceleration;
        enemy.navMeshAgent.stoppingDistance = stateData.stoppingDistance;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.navMeshAgent.SetDestination(enemy.target.position);
        if (enemy.CheckDanger() && enemy.abilities.FindAbilityByIndex(2).numberOfCharges > 0)
        {
            enemy.abilities.TriggerAbilityByIndex(2);
        }
        else if (enemy.CheckPlayerMaxRange() && !enemy.CheckPlayerMinRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
        else if (enemy.CheckPlayerMinRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            if (enemy.abilities.FindAbilityByIndex(2).numberOfCharges >= 1) { }
            else stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
