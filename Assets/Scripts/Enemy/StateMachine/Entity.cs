using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public int enemyID;
    public float health;
    [HideInInspector] public float maxHealth;
    public EntityType type;

    public FiniteStateMachine stateMachine;

    public Transform firePoint;
    [HideInInspector] public Transform target;
    public D_Entity entityData;

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public EntityAbilities abilities { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    [SerializeField] private EnemyHealthSlider healthSlider;

    [HideInInspector] public bool playDeathSound = true;

    [SerializeField] private bool seeThroughObstacles = false;
    private bool isKnockedBack = false;

    private bool invincible = false;

    private Vector3 velocityWorkspace;
    private static int id;

    public delegate void SwitchAction();
    public static event SwitchAction OnSwitch;

    public delegate void PlayerDamagedAction();
    public static event PlayerDamagedAction OnPlayerDamaged;

    public delegate void EntityDamagedAction(Entity entity);
    public static event EntityDamagedAction OnEntityDamaged;

    public delegate void PlayerKilledAction();
    public static event PlayerKilledAction OnPlayerKilled;

    public delegate void EnemyKilledAction();
    public static event EnemyKilledAction OnEnemyKilled;

    private void Awake()
    {
        maxHealth = health;

        DebugLogConsole.AddCommand("buddha", "Upon death sets player HP to 1, ensuring they can never die.", SetInvincibleFlag);
    }

    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        abilities = gameObject.GetComponent<EntityAbilities>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        atsm = gameObject.GetComponent<AnimationToStateMachine>();

        healthSlider = GetComponentInChildren<EnemyHealthSlider>();
        if (type == EntityType.Player) healthSlider.Hide();

        stateMachine = new FiniteStateMachine();

        target = FindObjectOfType<PlayerMovement>().transform;

        enemyID = id++;
    }

    public virtual void Update()
    {
        if (type == EntityType.Enemy) stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        if (type == EntityType.Enemy) stateMachine.currentState.PhysicsUpdate();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && invincible) health = 1;

        if (type == EntityType.Player && OnPlayerDamaged != null) OnPlayerDamaged.Invoke();
        if (OnEntityDamaged != null) { Debug.Log("OnEntityDamaged " + this.name); OnEntityDamaged.Invoke(this); }

        if (health <= 0) Die();
    }

    private void Die()
    {
        if (type == EntityType.Enemy)
        {
            if (OnEnemyKilled != null) OnEnemyKilled.Invoke();
            Destroy(gameObject);
        }
        else if (type == EntityType.Player) 
        {
            if (OnPlayerKilled != null) OnPlayerKilled.Invoke();
            PlayerDeath.instance.TriggerDeathSequence();
        }
    }

    public virtual Vector3 SetDirection(Vector3 target)
    {
        Vector3 value = gameObject.transform.position - target;
        value.Normalize();
        return value;
    }

    public virtual void SetVelocity(float vel)
    {
        velocityWorkspace.Set(vel, rb.velocity.y, vel);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float vel, Vector3 direction)
    {
        velocityWorkspace.Set(vel * direction.x, rb.velocity.y, vel * direction.z);
        rb.velocity = velocityWorkspace;
    }

    public void SetPosition(Transform point)
    {
        gameObject.transform.position = point.position;
    }

    public void ResetState()
    {
        animator.Rebind();
        animator.Update(0f);
    }

    private bool canSeePlayer;

    public bool CanSeePlayer
    {
        get { SeesPlayer(); return canSeePlayer; }
        set
        {
            if (value == canSeePlayer)
                return;

            canSeePlayer = value;
            //ToggleFacePlayer(canSeePlayer);
        }
    }

    public bool CanSeePlayerWithClearLineOfSight
    {
        get { SeesPlayerWithNoObstructions(); return canSeePlayer; }
        set
        {
            if (value == canSeePlayer)
                return;

            canSeePlayer = value;
            //ToggleFacePlayer(canSeePlayer);
        }
    }

    public virtual bool CheckPlayerMinRange()
    {
        if (CanSeePlayer)
        {
            if (Physics2D.OverlapCircleAll(gameObject.transform.position, entityData.playerMinCheckDist, entityData.whatIsPlayer).Length != 0) return true;
            else return false;
        }
        else return false;
    }

    public virtual bool CheckPlayerMaxRange()
    {
        if (CanSeePlayer)
        {
            if (Physics2D.OverlapCircleAll(gameObject.transform.position, entityData.playerMaxCheckDist, entityData.whatIsPlayer).Length != 0) return true;
            else return false;
        }
        else return false;
    }

    private void SeesPlayer()
    {
        if (!seeThroughObstacles)
        {
            SeesPlayerWithNoObstructions();
        }
        else CanSeePlayer = true;
    }

    private void SeesPlayerWithNoObstructions()
    {
        Vector3 direction = target.gameObject.transform.position - gameObject.transform.position;
        direction.Normalize();
        float distance = Vector2.Distance(target.gameObject.transform.position, gameObject.transform.position);

        Debug.DrawRay(transform.position, target.gameObject.transform.position - gameObject.transform.position, Color.yellow);

        RaycastHit[] hits = Physics.RaycastAll(gameObject.transform.position, direction, distance, entityData.whatIsGround);
        for (int i = 0; i < hits.Length; i++) Debug.Log(gameObject.name + " hit " + hits[i].collider.gameObject.name + ", which is on layer " + hits[i].collider.gameObject.layer.ToString());

        bool hit = Physics.Raycast(gameObject.transform.position, direction, distance, entityData.whatIsGround);

        if (hit) CanSeePlayer = false;
        else CanSeePlayer = true;
    }

    /*public void ToggleFacePlayer(bool toggle)
    {
        if (gameObject.GetComponent<FacePlayer>() != null) gameObject.GetComponent<FacePlayer>().enabled = toggle;
    }*/

    public virtual bool CheckCloseRangeAction()
    {
        return Physics.OverlapSphere(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsPlayer).Length > 0;
    }

    public virtual Transform FindPlayersLastPosition()
    {
        return target.gameObject.transform;
    }

    public void DealDamage(float damage)
    {
        target.GetComponent<Entity>().TakeDamage(damage);
    }

    public bool KnockedBack()
    {
        return isKnockedBack;
    }

    public void OnReceivedKnockback()
    {
        StartCoroutine(KnockbackCoroutine());
    }

    public void OnTakeDamage()
    {
        //if (entityData.hurtSound) AudioManager.instance.Play(entityData.hurtSound.name, 5);
    }

    public void OnDeath()
    {
        //if (entityData.deathSound && playDeathSound) AudioManager.instance.Play(entityData.deathSound.name, 5);
    }

    private IEnumerator KnockbackCoroutine()
    {
        yield return null;
        navMeshAgent.enabled = false;
        //rb.useGravity = true;
        rb.isKinematic = false;
        isKnockedBack = true;

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.velocity.magnitude <= entityData.knockbackStillThreshold);
        yield return new WaitForSeconds(0.1f);

        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        navMeshAgent.enabled = true;
        //rb.useGravity = true;
        rb.isKinematic = false;
        navMeshAgent.Warp(transform.position);

        yield return null;

        isKnockedBack = false;
    }

    public virtual void OnDrawGizmos()
    {
        if (type == EntityType.Player) return;

        Vector3 groundCheckOrigin = transform.position + Vector3.up * 0.1f;

        Gizmos.DrawWireSphere(transform.position, entityData.closeRangeDist);
        Gizmos.DrawWireSphere(transform.position, entityData.playerMinCheckDist);

        Gizmos.DrawWireSphere(transform.position, entityData.playerMaxCheckDist);
    }

    private void SetInvincibleFlag()
    {
        if (type != EntityType.Player) return;

        invincible = !invincible;
        if (invincible) Debug.Log("Buddha Mode on...");
        else if (invincible) Debug.Log("Buddha Mode off...");
    }

    public void OnSwitchedTo(Entity from)
    {
        invincible = from.invincible;

        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";
        type = EntityType.Player;

        healthSlider.Hide();
    }

    public void OnSwitchedFrom(Entity to)
    {
        invincible = false;

        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.tag = "Enemy";
        type = EntityType.Enemy;

        rb.velocity = Vector3.zero;
        healthSlider.Show();

        OnSwitch.Invoke();
    }
}

public enum EntityType
{
    None,
    Player,
    Enemy
}