using UnityEngine.AI;

public class MoveState : State, IEnemyState
{
    protected D_MoveState stateData;

    protected bool wallDetected;
    protected bool groundDetected;
    protected bool playerDetectedInMinRange;
    protected bool playerDetectedInMaxRange;
    protected bool closeAction;
    protected bool flipImmediately;
    protected bool knockedBack;

    public MoveState() { }

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Checks()
    {
        base.Checks();

        playerDetectedInMinRange = entity.CheckPlayerMinRange();
        playerDetectedInMaxRange = entity.CheckPlayerMaxRange();
        closeAction = entity.CheckCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();
        entity.navMeshAgent.speed = stateData.moveSpeed;
        entity.navMeshAgent.acceleration = stateData.acceleration;
        entity.navMeshAgent.stoppingDistance = stateData.stoppingDistance;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //entity.SetVelocity(stateData.moveSpeed, entity.SetDirection(entity.target.position));
        if (!knockedBack) entity.navMeshAgent.SetDestination(entity.target.transform.position);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
