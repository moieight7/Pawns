using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public int enemyID;
    [HideInInspector] public float health;
    [HideInInspector] public float maxHealth;
    public EntityType type;

    public FiniteStateMachine stateMachine;

    public Transform firePoint;
    public Transform target;
    public D_Entity entityData;

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public EntityAbilities abilities { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    [SerializeField] private EnemyHealthSlider healthSlider;

    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool playDeathSound = true;
    [HideInInspector] public List<ScriptableObject> dataObjects = new List<ScriptableObject>();

    private bool isKnockedBack = false;

    private bool invincible = false;

    private Coroutine dashCooldown;

    private Vector2 velocityWorkspace;
    private static int id;

    public delegate void SwitchAction(Entity to, Entity from);
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
        health = entityData.health;
        maxHealth = health;

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        Target.OnTargetSet += OnTargetSet;

        DebugLogConsole.AddCommand("buddha", "Upon death sets player HP to 1, ensuring they can never die.", SetInvincibleFlag);
    }

    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        abilities = gameObject.GetComponent<EntityAbilities>();

        atsm = gameObject.GetComponent<AnimationToStateMachine>();

        healthSlider = GetComponentInChildren<EnemyHealthSlider>();
        if (type == EntityType.Player) healthSlider.Hide();

        stateMachine = new FiniteStateMachine();

        target = Target.instance.TargetTransform;

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
        for (int i = 0; i < hits.Length; i++) Debug.Log(gameObject.name + " hit " + hits[i].collider.gameObject.name + ", which is on layer " + hits[i].collider.gameObject.layer.ToString());

        bool hit = Physics2D.CircleCast(gameObject.transform.position, entityData.circleCastCheckRadius, direction, distance, entityData.whatIsWall);

        if (hit) CanSeePlayer = false;
        else CanSeePlayer = true;
    }

    public virtual bool CheckDanger()
    {
        List<Collider2D> dangerous = new List<Collider2D>();
        if (Physics2D.OverlapCircleAll(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger).Length != 0) 
        {
            dangerous = Physics2D.OverlapCircleAll(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger).ToList<Collider2D>();
            Debug.Log("CheckDanger: " + Physics2D.OverlapCircleAll(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger)[0].name);

            List<Collider2D> nonFriendly = new List<Collider2D>();
            foreach (Collider2D collider in dangerous)
            {
                if (!((entityData.whatIsFriendly | (1 << collider.transform.parent.gameObject.layer)) == entityData.whatIsFriendly)) nonFriendly.Add(collider);
            }
            if (nonFriendly.Count == 0) return false;

            return true; 
        }
        else return false;
    }

    public virtual GameObject GetDangerousObject()
    {
        Debug.Log("GetDangerousObject: " + Physics2D.OverlapCircle(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger).gameObject.name);
        return Physics2D.OverlapCircle(gameObject.transform.position, entityData.closeRangeDist, entityData.whatIsDanger).gameObject;
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

    private void SetInvincibleFlag()
    {
        if (type != EntityType.Player) return;

        invincible = !invincible;
        if (invincible) Debug.Log("Buddha Mode on...");
        else if (invincible) Debug.Log("Buddha Mode off...");
    }

    public void OnSwitchedTo(Entity from)
    {
        Debug.Log("OnSwitchedTo entity " + name + " from entity " + from.name);

        invincible = from.invincible;

        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";
        type = EntityType.Player;

        navMeshAgent.isStopped = true;

        if (dashCooldown != null) StopCoroutine(dashCooldown);
        isDashing = false;

        healthSlider.Hide();
    }

    public void OnSwitchedFrom(Entity to)
    {
        Debug.Log("OnSwitchedFrom entity " + name + " to entity " + to.name);

        invincible = false;

        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.tag = "Enemy";
        type = EntityType.Enemy;

        rb.velocity = Vector3.zero;
        healthSlider.Show();

        navMeshAgent.isStopped = false;

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