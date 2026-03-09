using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyMelee : MonoBehaviour
{
    public enum State
    {
        Chasing,
        Attacking,
        GotHit,
        Dead
    }

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Animator Parameters")]
    public string attackBoolName = "isAttacking";
    public string deadTriggerName = "Dead";
    public string hitTriggerName = "Hit";

    int attackHash;
    int deadHash;
    int hitHash;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Attack")]
    public float attackRange = 3.5f;
    public float stoppingDistance = 3f;

    [Header("Health")]
    public int health = 5;

    [Header("Hit Reaction")]
    public float hitRecoverTime = 1f;

    [Header("Death")]
    public bool destroyAfterDeath = true;
    public float destroyDelay = 5f;

    [Header("Events")]
    public UnityEvent onAttack;
    public UnityEvent onDead;

    [Header("Debug")]
    public bool drawAttackRange = true;

    State currentState = State.Chasing;

    void Start()
    {
        attackHash = Animator.StringToHash(attackBoolName);
        deadHash = Animator.StringToHash(deadTriggerName);
        hitHash = Animator.StringToHash(hitTriggerName);

        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.updateRotation = false;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

    void Update()
    {
        if (currentState == State.Dead) return;
        if (currentState == State.GotHit) return;
        if (player == null) return;

        FacePlayer();

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            EnterAttackState();
        }
        else
        {
            EnterChaseState();
        }
    }

    void EnterChaseState()
    {
        if (currentState == State.Chasing) return;

        currentState = State.Chasing;

        animator.SetBool(attackHash, false);
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void EnterAttackState()
    {
        if (currentState == State.Attacking) return;

        currentState = State.Attacking;

        agent.ResetPath();
        agent.isStopped = true;

        animator.SetBool(attackHash, true);
    }

    // Animation Event
    public void AttackAnimEvent()
    {
        if (currentState != State.Attacking) return;

        onAttack?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        if (currentState == State.Dead) return;

        health -= amount;

        if (health <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(GotHitRoutine());
    }

    IEnumerator GotHitRoutine()
    {
        currentState = State.GotHit;

        agent.ResetPath();
        agent.isStopped = true;

        animator.SetBool(attackHash, false);
        animator.SetTrigger(hitHash);

        yield return new WaitForSeconds(hitRecoverTime);

        agent.isStopped = false;
        currentState = State.Chasing;
    }

    public void Die()
    {
        if (currentState == State.Dead) return;

        currentState = State.Dead;

        agent.ResetPath();
        agent.isStopped = true;

        animator.SetBool(attackHash, false);
        animator.SetTrigger(deadHash);

        onDead?.Invoke();

        if (destroyAfterDeath)
            Destroy(gameObject, destroyDelay);
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void LateUpdate()
    {
        if (currentState == State.Chasing && player != null)
            agent.SetDestination(player.position);
    }

    void OnDrawGizmosSelected()
    {
        if (!drawAttackRange) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}