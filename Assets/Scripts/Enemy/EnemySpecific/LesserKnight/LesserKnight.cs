using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserKnight : Entity
{
    public LesserKnight_IdleState IdleState { get; private set; }
    public LesserKnight_MoveState MoveState { get; private set; }
    public LesserKnight_DashState DashState { get; private set; }
    public LesserKnight_StaggerState StaggerState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_IdleState IdleStateData;
    [SerializeField]
    private D_MoveState MoveStateData;
    [SerializeField]
    private D_DashState DashStateData;
    [SerializeField]
    private D_StaggerState StaggerStateData;
    
    public override void Start()
    {
        base.Start();

        IdleState = new LesserKnight_IdleState(this, stateMachine, "Idle", IdleStateData, this);
        MoveState = new LesserKnight_MoveState(this, stateMachine, "Move", MoveStateData, this);
        DashState = new LesserKnight_DashState(this, stateMachine, "Dash", DashStateData, this);
        StaggerState = new LesserKnight_StaggerState(this, stateMachine, "Stagger", StaggerStateData, this);
        
        stateMachine.Initialize(IdleState);
    }

    public D_DashState GetDashStateData() { return DashStateData; }
}
