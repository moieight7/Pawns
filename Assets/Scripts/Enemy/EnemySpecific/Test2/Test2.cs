using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : Entity
{
    public Test2_IdleState IdleState { get; private set; }
    public Test2_MoveState MoveState { get; private set; }
    
    [SerializeField]
    private D_IdleState IdleStateData;
    [SerializeField]
    private D_MoveState MoveStateData;
    
    public override void Start()
    {
        base.Start();

        IdleState = new Test2_IdleState(this, stateMachine, "Idle", IdleStateData, this);
        MoveState = new Test2_MoveState(this, stateMachine, "Move", MoveStateData, this);
        
        stateMachine.Initialize(IdleState);
    }
}
