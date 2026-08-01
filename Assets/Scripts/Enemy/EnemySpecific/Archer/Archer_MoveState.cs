public class Archer_MoveState : MoveState
{
    private Archer enemy;

    public Archer_MoveState() { }

    public Archer_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        if (enemy.CheckPlayerMaxRange() && enemy.CanSeePlayerWithClearLineOfSight)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
