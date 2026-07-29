public class LesserKnight_MoveState : MoveState
{
    private LesserKnight enemy;

    public LesserKnight_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, LesserKnight enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        if (enemy.CheckDanger() && enemy.abilities.FindAbilityByIndex(2).numberOfCharges > 0 && !enemy.isDashing)
        {
            enemy.abilities.TriggerAbilityByIndex(2);
            stateMachine.ChangeState(enemy.DashState);
        }
        else if (enemy.CheckPlayerMaxRange() && enemy.abilities.FindAbilityByIndex(1).numberOfCharges > 0 && !enemy.CheckPlayerMinRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            enemy.abilities.TriggerAbilityByIndex(1);
        }
        else if (enemy.CheckPlayerMinRange() && enemy.abilities.FindAbilityByIndex(0).numberOfCharges > 0 && enemy.CanSeePlayerWithClearLineOfSight)
        {
            enemy.abilities.TriggerAbilityByIndex(0);
        }
        else if (enemy.CheckPlayerMinRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
