using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Entity
{
    public Wizard_IdleState IdleState { get; private set; }
    public Wizard_MoveState MoveState { get; private set; }
    public Wizard_TeleportState TeleportState { get; private set; }
    
    [SerializeField]
    private D_IdleState IdleStateData;
    [SerializeField]
    private D_MoveState MoveStateData;
    [SerializeField]
    private D_TeleportState TeleportStateData;
    
    public override void Start()
    {
        base.Start();

        IdleState = new Wizard_IdleState(this, stateMachine, "Idle", IdleStateData, this);
        MoveState = new Wizard_MoveState(this, stateMachine, "Move", MoveStateData, this);
        TeleportState = new Wizard_TeleportState(this, stateMachine, "Teleport", TeleportStateData, this);
        
        stateMachine.Initialize(IdleState);
    }

    public float GetTeleportRadius()
    {
        return TeleportStateData.teleportRadius;
    }
}
