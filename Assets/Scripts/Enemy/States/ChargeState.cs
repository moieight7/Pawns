using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ChargeState : State, IEnemyState
{
    protected D_ChargeState stateData;

    protected float timer, staggerTimer;
    protected bool canDoCharge = false, isCharging = false;

    protected bool playerDetectedInMinRange;
    protected bool playerDetectedInMaxRange;
    protected bool isFacingLedge;
    protected bool isFacingWall;
    protected bool hasLineOfSight;

    private float oldNavMeshAgentSpeed;
    private Vector3 dir;

    public ChargeState() { }

    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Checks()
    {
        base.Checks();

        playerDetectedInMinRange = entity.CheckPlayerMinRange();
        playerDetectedInMaxRange = entity.CheckPlayerMaxRange();

        hasLineOfSight = entity.CanSeePlayerWithClearLineOfSight;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("ChargeState state - Enter, isCharging: " + isCharging);
        entity.SetVelocity(0f);

        timer = 0f; staggerTimer = 0f;
        canDoCharge = false; isCharging = false;
        oldNavMeshAgentSpeed = entity.navMeshAgent.speed;

        //entity.ToggleFacePlayer(false);
    }

    public override void Exit()
    {
        base.Exit();
        
        isCharging = false;
        entity.navMeshAgent.speed = oldNavMeshAgentSpeed;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Debug.Log("ChargeState state - LogicUpdate, isCharging: " + isCharging);
        if (!isCharging)
        {
            timer += Time.deltaTime;

            if (timer > stateData.windupTime) { canDoCharge = true; }
            else Debug.Log("Waiting for charge");

            if (canDoCharge)
            {
                Debug.Log("Do charge");
                entity.navMeshAgent.speed = entity.navMeshAgent.speed * stateData.chargeSpeedMultiplier;
                entity.navMeshAgent.SetDestination(entity.target.transform.position);

                dir = entity.target.transform.position - entity.gameObject.transform.position;

                canDoCharge = false;
                isCharging = true;

                //entity.ToggleFacePlayer(false);
            }    
        }

        if (isCharging) { Debug.Log("IsCharging"); entity.navMeshAgent.SetDestination(entity.navMeshAgent.destination + dir.normalized * 2); }

        if (playerDetectedInMinRange) TriggerAttack();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack()
    {
        // enable charge attack hitbox, do knockback, then stagger the enemy for a moment
    }

    public virtual void FinishAttack()
    {
        
    }
}
