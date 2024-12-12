using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTAI;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

/*
 * Basic idea of the behavior of the zombie
 * - Root
 *   - Main Selector (Selector)
 *     - Offend Sequence
 *       - Determine whether the player is in sight (Condition)
 *       - Second Selector (Selector)
 *         - Attack Sequence (Sequence)
 *           - Determine whether the player is in attack range (Condition)
 *           - Do attack (Action / RunCoroutine)
 *           - Do follow (Action / RunCoroutine)
 *         - Follow Sequence
 *           - Determine whether the player is in follow range (Condition)
 *           - Do follow (Action / RunCoroutine)
 *     - Idle Sequence (Sequence)
 *       - Do idle (Action / RunCoroutine)
 */
public class ZombieBehavior : MonoBehaviour
{
    // GameObject Components
    Animator _animator;
    NavMeshAgent _agent;

    // behavior tree components
    private Root _root;
    private Selector _mainSelector;
    private Sequence _offendSeq;
    private BTNode _isInSightC;
    private Selector _secSelector;
    private Sequence _attackSeq;
    private BTNode _inAttackRangeC;
    private BTNode _attack;
    private BTNode _follow;
    private Sequence _followSeq;
    private BTNode _inFollowRangeC;
    private Sequence _idleSeq;
    private BTNode _idle;

    // other private variables
    private int _playerHealth;

    // public variables
    public float wanderRange;
    public float attackRange;
    public float followRange;
    public float observeRange;
    public int damage;
    public GameObject player;
    public Text health;

    // Start is called before the first frame update
    void Start()
    {
        // initialize nodes
        _root = BT.Root();
        _mainSelector = BT.Selector(false);
        _offendSeq = BT.Sequence();
        _isInSightC = BT.Condition(IsInSight);
        _secSelector = BT.Selector(false);
        _attackSeq = BT.Sequence();
        _inAttackRangeC = BT.Condition(IsInAttackRange);
        _attack = BT.RunCoroutine(Attack);
        _follow = BT.RunCoroutine(Follow);
        _followSeq = BT.Sequence();
        _inFollowRangeC = BT.Condition(IsInFollowRange);
        _idleSeq = BT.Sequence();
        _idle = BT.RunCoroutine(Idle);

        // link the nodes
        _attackSeq.OpenBranch(_inAttackRangeC, _attack, _follow);
        _followSeq.OpenBranch(_inFollowRangeC, _follow);
        _secSelector.OpenBranch(_attackSeq, _followSeq);
        _offendSeq.OpenBranch(_isInSightC, _secSelector);
        _idleSeq.OpenBranch(_idle);
        _mainSelector.OpenBranch(_offendSeq, _idleSeq);
        _root.OpenBranch(_mainSelector);

        // other initialization
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _playerHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        _root.Tick();
    }

    /// <summary>
    /// Attack always returns success after performing the attack, no matter
    /// the attack hits the player or not.
    /// </summary>
    /// <returns></returns>
    IEnumerator<BTState> Attack()
    {
        Debug.Log("Entered Attack Sequence");
        _animator.SetBool("attacking", true);
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        // determine whether the player is still close to the zombie
        if (GetDistanceFromPlayer() < attackRange + 1.5f)
        {
            // the player gets damage
            int.TryParse(health.text, out int currHealth);
            currHealth -= damage;
            health.text = currHealth.ToString();
        }
        _animator.SetBool("attacking", false);
        _animator.SetBool("targeting", false);
        time = 0;
        while (time < 2f)
        {
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        _animator.SetBool("targeting", true);
        yield return BTState.Success;
    }

    /// <summary>
    /// Update the target location of the zombie to the location of the player
    /// once every 2 seconds. Each update requires a check of whether the 
    /// player is in the follow range of the zombie.
    /// </summary>
    /// <returns>
    /// Follow returns Failure only when the player is out of the zombie's
    /// follow range. It returns success when it reaches the player.
    /// </returns>
    IEnumerator<BTState> Follow()
    {
        _animator.SetBool("targeting", true);
        _agent.speed = 4.5f;
        _agent.acceleration = 4f;
        _agent.SetDestination(player.transform.position);
        yield return BTState.Continue;
        float time;
        while (IsInFollowRange() && ! IsInAttackRange())
        {
            _agent.SetDestination(player.transform.position);
            time = 0;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                yield return BTState.Continue;
            }
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    /// <summary>
    /// The zombie stays stationary for 5 seconds, and then move to a position
    /// within wanderRange. Animation controll included.
    /// </summary>
    /// <returns></returns>
    IEnumerator<BTState> Idle()
    {
        // TESTED
        _agent.speed = 1f;
        _agent.acceleration = 0.5f;
        // rest for 5 seconds first
        _animator.SetBool("targeting", false);
        _animator.SetBool("walk", false);
        float time = 0;
        while (time < 5)
        {
            if (IsInSight()) // found player
            {
                yield return BTState.Failure;
            }
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        // move to a random position within wanderRange
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < 0.5f)
        {
            _animator.SetBool("walk1", true);
        }
        else
        {
            _animator.SetBool("walk1", false);
        }
        _animator.SetBool("walk", true);
        for (int i = 0; i < 10; i++)
        {
            yield return BTState.Continue;
        }
        RandomPoint(transform.position, wanderRange, 
            out Vector3 destination);
        _agent.SetDestination(destination);
        // ESSENTIAL! Wait for one frame after setting destination
        yield return BTState.Continue;
        while (_agent.remainingDistance > 0.1f)
        {
            if (IsInSight()) // found player
            {
                yield return BTState.Failure;
            }
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        _animator.SetBool("walk", false);
        for (int i = 0; i < 10; i++)
        {
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    bool IsInSight()
    {
        float verticalDist = player.transform.position.y -
            transform.position.y;
        if (verticalDist > 3)
        {
            return false;
        }
        float horizontalDist = Mathf.Sqrt(
            Mathf.Pow(player.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(player.transform.position.z - transform.position.z, 2)
            );
        if (horizontalDist <= observeRange)
        {
            return true;
        }
        return false;
    }

    bool IsInAttackRange()
    {
        if (GetDistanceFromPlayer() <= attackRange)
        {
            return true;
        }
        return false;
    }

    bool IsInFollowRange()
    {
        float verticalDist = player.transform.position.y -
            transform.position.y;
        if (verticalDist > 3.5)
        {
            return false;
        }
        float horizontalDist = Mathf.Sqrt(
            Mathf.Pow(player.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(player.transform.position.z - transform.position.z, 2)
            );
        if (horizontalDist <= followRange)
        {
            return true;
        }
        return false;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 
                    1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    float GetDistanceFromPlayer()
    {
        return Vector3.Magnitude(
            player.transform.position - transform.position);
    }
}
