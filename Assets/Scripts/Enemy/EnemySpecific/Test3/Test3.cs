using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : Entity
{
    public Test3_IdleState IdleState { get; private set; }
    public Test3_MoveState MoveState { get; private set; }
    public Test3_DashState DashState { get; private set; }
    
    [SerializeField]
    private D_IdleState IdleStateData;
    [SerializeField]
    private D_MoveState MoveStateData;
    [SerializeField]
    private D_DashState DashStateData;
    
    public override void Start()
    {
        base.Start();

        IdleState = new Test3_IdleState(this, stateMachine, "Idle", IdleStateData, this);
        MoveState = new Test3_MoveState(this, stateMachine, "Move", MoveStateData, this);
        DashState = new Test3_DashState(this, stateMachine, "Dash", DashStateData, this);
        
        stateMachine.Initialize(IdleState);
    }
}
