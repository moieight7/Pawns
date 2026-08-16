using DG.Tweening;
using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    [Header("Entity Data")]
    [HideInInspector] public float health;
    [HideInInspector] public float maxHealth;
    public EntityType type;

    public FiniteStateMachine stateMachine;

    public Transform firePoint;
    public D_Entity entityData;
    [HideInInspector] public Transform target;

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public EntityAbilities abilities { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }

    private EnemyHealthSlider healthSlider;

    [HideInInspector] public bool isDashing = false;

    private float iFrameTimer = 0;
    private float lifedrainDuration = 30;
    private float lifedrainStep;

    private bool invincible = false;
    private bool lifedrain = false;
    private bool pauseStateMachine = false;
    private bool playerEntityDead = false;

    private Coroutine dashCooldown, lifedrainCoroutine;

    private Vector2 velocityWorkspace;

    [Header("Events")]
    public UltEvent OnSwitchedToEvent;
    public UltEvent OnSwitchedFromEvent;

    public delegate void SwitchAction(Entity to, Entity from);
    public static event SwitchAction OnSwitch;

    public delegate void PlayerDamagedAction();
    public static event PlayerDamagedAction OnPlayerDamaged;

    public delegate void PlayerKilledAction();
    public static event PlayerKilledAction OnPlayerKilled;

    public delegate void EntityDamagedAction(Entity entity);
    public static event EntityDamagedAction OnEntityDamaged;

    public delegate void EnemyKilledAction();
    public static event EnemyKilledAction OnEnemyKilled;

    public delegate void LifedrainEnabledAction();
    public static event LifedrainEnabledAction OnLifedrainEnabled;

    private void Awake()
    {
        health = entityData.health;
        maxHealth = health;

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        if (type == EntityType.Player) navMeshAgent.enabled = false;

        Target.OnTargetSet += OnTargetSet;

        DebugLogConsole.AddCommandStatic("buddha", "Upon death sets player HP to 1, ensuring they can never die.", "SetInvincibleFlag", typeof(Entity));
        DebugLogConsole.AddCommandStatic("hurt", "Deals damage to the player entity.", "TakeDamageConsole", typeof(Entity));
        DebugLogConsole.AddCommandStatic("ghost", "Turns off the player's collisions.", "SetPlayerCollisions", typeof(Entity));
        DebugLogConsole.AddCommand("dracula", "Enables/disables the lifedrain flag on the player entity.", SetLifedrainFlagConsole);
    }

    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (type == EntityType.Player) rb.bodyType = RigidbodyType2D.Dynamic;

        animator = gameObject.GetComponent<Animator>();
        abilities = gameObject.GetComponent<EntityAbilities>();

        healthSlider = GetComponentInChildren<EnemyHealthSlider>();
        if (type == EntityType.Player) healthSlider.Hide();

        if (GetComponent<PlayerMovement>() != null) GetComponent<PlayerMovement>().movementSpeed = entityData.playerMovementSpeed;

        stateMachine = new FiniteStateMachine();
        target = Target.instance.TargetTransform;
    }

    public virtual void Update()
    {
        iFrameTimer -= Time.deltaTime;
        iFrameTimer = Mathf.Clamp(iFrameTimer, 0, entityData.iFrameTime);
        if (type == EntityType.Enemy && !pauseStateMachine) 
            stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        if (type == EntityType.Enemy && !pauseStateMachine) 
            stateMachine.currentState.PhysicsUpdate();
    }

    public void TakeDamage(float damage, bool canKill = true, bool triggerIframe = true)
    {
        if (health < 0 && !invincible || iFrameTimer > 0) return;

        health -= damage;
        if (health <= 0 && invincible) health = 1;

        if (type == EntityType.Player)
        {
            if (OnPlayerDamaged != null) OnPlayerDamaged.Invoke();
            if (triggerIframe)
            {
                iFrameTimer = entityData.iFrameTime;
                if (GetComponent<IFrameAnimation>() != null) GetComponent<IFrameAnimation>().DoIFrameAnim(entityData.iFrameTime);
            }
        }
        if (OnEntityDamaged != null) OnEntityDamaged.Invoke(this);

        if (health <= 0 && canKill) Die();
    }

    private void SetHealth(float setTo)
    {
        health = setTo;
        if (OnPlayerDamaged != null) OnPlayerDamaged.Invoke();
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
            playerEntityDead = true;
            if (OnPlayerKilled != null) OnPlayerKilled.Invoke();
            PlayerDeath.instance.TriggerDeathSequence(this);
        }
    }

    private IEnumerator LifedrainCoroutine()
    {
        while (lifedrain)
        {
            yield return new WaitForSeconds(entityData.lifedrainDelay);
            if (health == 1 || playerEntityDead) continue;
            if (health - Mathf.Abs(lifedrainStep) < 1) 
            {
                #if UNITY_EDITOR
                    Debug.Log("LifedrainCoroutine: " + (lifedrainStep - health));
                #endif
                SetHealth(1); 
            }
            else if (health > 1) TakeDamage(lifedrainStep, false, false);
        }
    }

    public virtual void Dash(Vector2 dash, float duration = 0.4f)
    {
        isDashing = true;
        velocityWorkspace.Set(dash.x, dash.y);

        if (!CheckForObstaclesInDashDirection(velocityWorkspace, duration)) velocityWorkspace.Set(-dash.x, -dash.y);

        rb.velocity = velocityWorkspace;
        navMeshAgent.isStopped = true;

        dashCooldown = StartCoroutine(DashCooldown(duration));
    }

    private bool CheckForObstaclesInDashDirection(Vector2 dash, float duration)
    {
        List<RaycastHit2D> wallCheck = Physics2D.RaycastAll(gameObject.transform.position, dash.normalized, (dash * duration).magnitude, entityData.whatIsWall).ToList();
        return wallCheck.Count == 0;
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
        if (!entityData.seeThroughObstacles)
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

        RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position, entityData.circleCastCheckRadius, direction, distance, entityData.whatIsWall);

        #if UNITY_EDITOR
        for (int i = 0; i < hits.Length; i++) Debug.Log(gameObject.name + " hit " + hits[i].collider.gameObject.name + ", which is on layer " + hits[i].collider.gameObject.layer.ToString());
        #endif

        bool hit = Physics2D.CircleCast(gameObject.transform.position, entityData.circleCastCheckRadius, direction, distance, entityData.whatIsWall);

        if (hit) CanSeePlayer = false;
        else CanSeePlayer = true;
    }

    public virtual bool CheckDanger()
    {
        List<Collider2D> dangerous = new List<Collider2D>();
        if (Physics2D.OverlapCircleAll(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger).Length != 0) 
        {
            dangerous = Physics2D.OverlapCircleAll(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger).ToList();

            List<Collider2D> nonFriendly = new List<Collider2D>();
            foreach (Collider2D collider in dangerous)
                if (!((entityData.whatIsFriendly | (1 << collider.transform.parent.gameObject.layer)) == entityData.whatIsFriendly)) 
                    nonFriendly.Add(collider);

            if (nonFriendly.Count == 0) return false;
            return true;
        }
        else return false;
    }

    public virtual GameObject GetDangerousObject()
    {
        return Physics2D.OverlapCircle(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger).gameObject;
    }

    public virtual bool CheckCloseRangeAction()
    {
        return Physics.OverlapSphere(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsPlayer).Length > 0;
    }

    private IEnumerator DashCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        isDashing = false;
        velocityWorkspace = Vector2.zero;
        rb.velocity = Vector2.zero;
        if (type != EntityType.Player) navMeshAgent.isStopped = false;
    }

    public virtual void OnDrawGizmos()
    {
        if (type == EntityType.Player) return;

        Vector3 groundCheckOrigin = transform.position + Vector3.up * 0.1f;

        Gizmos.DrawWireSphere(transform.position, entityData.closeRangeDist);
        Gizmos.DrawWireSphere(transform.position, entityData.playerMinCheckDist);

        Gizmos.DrawWireSphere(transform.position, entityData.playerMaxCheckDist);
    }

    private static void SetInvincibleFlag()
    {
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        player.invincible = !player.invincible;
        if (player.invincible) Debug.Log("Buddha Mode on...");
        else if (!player.invincible) Debug.Log("Buddha Mode off...");
    }

    private static void TakeDamageConsole(float damage)
    {
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        player.TakeDamage(damage);
    }

    public void SetLifedrainFlagConsole()
    {
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        player.lifedrain = !player.lifedrain;
        if (player.lifedrain) 
        { 
            Debug.Log("Lifedrain on...");
            lifedrainDuration = entityData.lifedrainDuration;

            lifedrainStep = entityData.health / lifedrainDuration;
            
            if (lifedrainCoroutine != null) StopCoroutine(lifedrainCoroutine);
            lifedrainCoroutine = StartCoroutine(LifedrainCoroutine());

            if (OnLifedrainEnabled != null) OnLifedrainEnabled.Invoke();
        }
        else if (!player.lifedrain)
        {
            Debug.Log("Lifedrain off...");
            if (lifedrainCoroutine != null) StopCoroutine(lifedrainCoroutine);
        }
    }

    public void SetLifedrainFlag(bool set)
    {
        if (set)
        {
            Debug.Log("Lifedrain on...");
            lifedrain = true;
            lifedrainDuration = entityData.lifedrainDuration;

            lifedrainStep = entityData.health / lifedrainDuration;

            if (lifedrainCoroutine != null) StopCoroutine(lifedrainCoroutine);
            lifedrainCoroutine = StartCoroutine(LifedrainCoroutine());

            if (OnLifedrainEnabled != null) OnLifedrainEnabled.Invoke();
        }
        else if (!set)
        {
            lifedrain = false;
            Debug.Log("Lifedrain off...");
            if (lifedrainCoroutine != null) StopCoroutine(lifedrainCoroutine);
        }
    }

    private static void SetPlayerCollisions()
    {
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        Collider2D collider = player.GetComponent<Collider2D>();

        collider.enabled = !collider.enabled;

        if (collider.enabled) Debug.Log("Ghost Mode on...");
        else if (!collider.enabled) Debug.Log("Ghost Mode off...");
    }

    public void PauseNavMeshAgent()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
    }

    public void UnpauseNavMeshAgent()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.enabled = true;
    }

    public void PauseStateMachine(bool pause)
    {
        pauseStateMachine = pause;
    }

    public void OnSwitchedTo(Entity from)
    {
        #if UNITY_EDITOR
        Debug.Log("OnSwitchedTo entity " + name + " from entity " + from.name);
        #endif

        abilities.CancelAbility();
        abilities.ResetAllCooldowns();

        invincible = from.invincible;
        SetLifedrainFlag(from.lifedrain);

        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";
        type = EntityType.Player;

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        if (dashCooldown != null) StopCoroutine(dashCooldown);
        isDashing = false;

        healthSlider.Hide();

        iFrameTimer = entityData.iFrameTime;
        if (GetComponent<IFrameAnimation>() != null) GetComponent<IFrameAnimation>().DoIFrameAnim(entityData.iFrameTime);

        if (from.health <= 0) from.Die();

        rb.bodyType = RigidbodyType2D.Dynamic;

        OnSwitchedToEvent.Invoke();
    }

    public void OnSwitchedFrom(Entity to)
    {
        #if UNITY_EDITOR
        Debug.Log("OnSwitchedFrom entity " + name + " to entity " + to.name);
        #endif

        invincible = false;
        SetLifedrainFlag(false);

        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.tag = "Enemy";
        type = EntityType.Enemy;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector3.zero;
        healthSlider.Show();

        navMeshAgent.isStopped = false;
        navMeshAgent.enabled = true;

        OnSwitchedFromEvent.Invoke();
        OnSwitch.Invoke(to, this);
    }

    private void OnTargetSet()
    {
        target = Target.instance.TargetTransform;
    }
}

public enum EntityType
{
    None,
    Player,
    Enemy
}