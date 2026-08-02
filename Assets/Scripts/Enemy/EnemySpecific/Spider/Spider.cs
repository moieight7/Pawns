using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Entity
{
    public Spider_IdleState IdleState { get; private set; }
    public Spider_MoveState MoveState { get; private set; }
    public Spider_DashState DashState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_IdleState IdleStateData;
    [SerializeField]
    private D_MoveState MoveStateData;
    [SerializeField]
    private D_DashState DashStateData;
    
    public override void Start()
    {
        base.Start();

        IdleState = new Spider_IdleState(this, stateMachine, "Idle", IdleStateData, this);
        MoveState = new Spider_MoveState(this, stateMachine, "Move", MoveStateData, this);
        DashState = new Spider_DashState(this, stateMachine, "Dash", DashStateData, this);
        
        stateMachine.Initialize(IdleState);
    }

    public D_DashState GetDashStateData() {  return DashStateData; }
}
