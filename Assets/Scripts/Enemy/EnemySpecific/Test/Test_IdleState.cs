public class Test_IdleState : IdleState
{
    private Test enemy;

    public Test_IdleState() { }

    public Test_IdleState(StateMachineEntity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Test enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (enemy.CheckPlayerMaxRange())
        {
            enemy.abilities.TriggerAbilityByIndex(0);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
