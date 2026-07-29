using UnityEngine;

public class Spider_DashState : DashState
{
    private Spider enemy;
    private Attack attackToAvoid;

    public Spider_DashState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DashState stateData, Spider enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        enemy.isDashing = true;

        attackToAvoid = enemy.GetDangerousObject().GetComponentInParent<Attack>();
        if (attackToAvoid == null) { stateMachine.ChangeState(enemy.IdleState); enemy.isDashing = false; }
        Debug.Log("AttackToAvoid: " + attackToAvoid.name);

        Vector2 dashDir = Vector2.zero;
        if (attackToAvoid is Projectile)
        {
            Debug.Log("AttackToAvoid projectile");

            Projectile projectile = (Projectile)attackToAvoid;
            //dashDir = Vector2.Perpendicular(projectile.moveDir);
            dashDir = new Vector2(projectile.moveDir.y, -projectile.moveDir.x);

            dashDir.x *= dashDir.x * stateData.dashForce;
            dashDir.y *= dashDir.y * stateData.dashForce;
            enemy.Dash(dashDir, stateData.dashDuration);
        }
        else if (attackToAvoid is Swing)
        {
            Debug.Log("AttackToAvoid swing");

            Swing swing = (Swing)attackToAvoid;

            dashDir = swing.sender.transform.position - enemy.transform.position;
            dashDir.Normalize();

            dashDir.x *= dashDir.x * stateData.dashForce;
            dashDir.y *= dashDir.y * stateData.dashForce;

            if (swing.sender.transform.position.x > enemy.transform.position.x) dashDir.x *= -1;
            if (swing.sender.transform.position.y > enemy.transform.position.y) dashDir.y *= -1;

            enemy.Dash(dashDir, stateData.dashDuration);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!enemy.isDashing) stateMachine.ChangeState(enemy.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
