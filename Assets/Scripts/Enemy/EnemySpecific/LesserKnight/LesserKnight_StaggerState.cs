using UnityEngine;

public class LesserKnight_StaggerState : StaggerState
{
    private LesserKnight enemy;
    private D_StaggerState stateData;

    private float staggerTimer, staggerTime;

    public LesserKnight_StaggerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StaggerState stateData, LesserKnight enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        this.stateData = stateData;
    }

    public override void Checks()
    {
        base.Checks();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.navMeshAgent.isStopped = true;

        staggerTimer = 0;
        staggerTime = Random.Range(stateData.minStaggerTime, stateData.maxStaggerTime);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        staggerTimer += Time.deltaTime;

        if (staggerTimer > staggerTime)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
