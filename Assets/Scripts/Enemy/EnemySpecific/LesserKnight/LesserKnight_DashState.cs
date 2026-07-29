public class LesserKnight_DashState : DashState
{
    private LesserKnight enemy;

    public LesserKnight_DashState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DashState stateData, LesserKnight enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (!enemy.isDashing) stateMachine.ChangeState(enemy.StaggerState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
