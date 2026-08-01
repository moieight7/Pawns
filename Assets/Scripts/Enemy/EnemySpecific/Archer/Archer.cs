using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Entity
{
    public Archer_IdleState IdleState { get; private set; }
    public Archer_MoveState MoveState { get; private set; }
    
    [SerializeField]
    private D_IdleState IdleStateData;
    [SerializeField]
    private D_MoveState MoveStateData;
    
    public override void Start()
    {
        base.Start();

        IdleState = new Archer_IdleState(this, stateMachine, "Idle", IdleStateData, this);
        MoveState = new Archer_MoveState(this, stateMachine, "Move", MoveStateData, this);
        
        stateMachine.Initialize(IdleState);
    }
}
