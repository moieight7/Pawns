using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Entity
{
    public Test_IdleState IdleState { get; private set; }
    
    [SerializeField]
    private D_IdleState IdleStateData;
    
    public override void Start()
    {
        base.Start();

        IdleState = new Test_IdleState(this, stateMachine, "Idle", IdleStateData, this);
        
        stateMachine.Initialize(IdleState);
    }
}
