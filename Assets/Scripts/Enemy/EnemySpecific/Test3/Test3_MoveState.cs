public class Test3_MoveState : MoveState
{
    private Test3 enemy;

    public Test3_MoveState() { }

    public Test3_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Test3 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        enemy.navMeshAgent.speed = stateData.moveSpeed;
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
        else if (enemy.CheckPlayerMaxRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
