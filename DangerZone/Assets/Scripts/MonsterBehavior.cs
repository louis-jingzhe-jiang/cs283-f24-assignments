using System.Collections;
using System.Collections.Generic;
using BTAI;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/*
 * Basic idea of the behavior of the monster
 * - Root
 *   - Main Selector (Selector)
 *     - Offend Sequence
 *       - Determine whether the player is in sight (Condition)
 *       - Second Selector (Selector)
 *         - Ranged attack sequence (Sequence)
 *           - Determine whether the player can be ranged attack (Condition)
 *           - Determine whether should perform (by chance) (Condition)
 *           - Do ranged attack (Action / RunCoroutine)
 *           - Do follow (Action / RunCoroutine)
 *         - Melee attack Sequence (Sequence)
 *           - Determine whether the player is in melee range (Condition)
 *           - Do melee attack (Action / RunCoroutine)
 *           - Do follow (Action / RunCoroutine)
 *         - Follow Sequence
 *           - Determine whether the player is in follow range (Condition)
 *           - Do follow (Action / RunCoroutine)
 *     - Idle Sequence (Sequence)
 *       - Do idle (Action / RunCoroutine)
 */
public class MonsterBehavior : MonoBehaviour
{
    // public variables
    public float wanderRange;
    public float meleeAttackRange;
    public float rangedAttackRange;
    public float followRange;
    public float observeRange;
    public int damage;
    public GameObject player;
    public Text health;

    // Behavior tree components
    private Root _root;
    private Selector _mainSelector;
    private Sequence _offendSeq;
    private BTNode _isInSightC;
    private Selector _secSelector;
    private Sequence _rangeSeq;
    private BTNode _inRangeRangeC;
    private BTNode _shouldRangeAttC;
    private BTNode _rangeAttack;
    private BTNode _follow;
    private Sequence _meleeSeq;
    private BTNode _inMeleeRangeC;
    private BTNode _meleeAttack;
    private Sequence _followSeq;
    private BTNode _inFollowRangeC;
    private Sequence _idleSeq;
    private BTNode _idle;

    // other private variables
    private Animator _animator;
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        // initialize nodes
        _root = BT.Root();
        _mainSelector = BT.Selector(false);
        _offendSeq = BT.Sequence();
        _isInSightC = BT.Condition(IsInSight);
        _secSelector = BT.Selector(false);
        _rangeSeq = BT.Sequence();
        _inRangeRangeC = BT.Condition(IsInRangeRange);
        _shouldRangeAttC = BT.Condition(ShouldRangeAtt);
        _rangeAttack = BT.RunCoroutine(RangeAttack);
        _follow = BT.RunCoroutine(Follow);
        _meleeSeq = BT.Sequence();
        _inMeleeRangeC = BT.Condition(IsInMeleeRange);
        _meleeAttack = BT.RunCoroutine(MeleeAttack);
        _followSeq = BT.Sequence();
        _inFollowRangeC = BT.Condition(IsInFollowRange);
        _idleSeq = BT.Sequence();
        _idle = BT.RunCoroutine(Idle);

        // link nodes
        _rangeSeq.OpenBranch(_inRangeRangeC, _shouldRangeAttC, _rangeAttack,
            _follow);
        _meleeSeq.OpenBranch(_inMeleeRangeC, _meleeAttack, _follow);
        _followSeq.OpenBranch(_inFollowRangeC, _follow);
        _secSelector.OpenBranch(_rangeSeq, _meleeSeq, _followSeq);
        _offendSeq.OpenBranch(_isInSightC, _secSelector);
        _idleSeq.OpenBranch(_idle);
        _mainSelector.OpenBranch(_offendSeq, _idleSeq);
        _root.OpenBranch(_mainSelector);

        // initialize other private vars
        GameObject demon = GameObject.Find("/Demon_default/Demon");
        _animator = demon.GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _root.Tick();
    }

    IEnumerator<BTState> Idle()
    {
        Debug.Log("Entered idle");
        _agent.speed = 1.5f;
        _agent.acceleration = 0.5f;
        // rest for 10 seconds first
        _animator.SetBool("chase", false);
        _animator.SetBool("walk", false);
        float time = 0;
        while (time < 10f)
        {
            if (IsInSight()) // found player
            {
                yield return BTState.Failure;
            }
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        // move to a random position within wanderRange
        _animator.SetBool("walk", true);
        for (int i = 0; i < 10; i++) // wait 10 frames for animation
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
        for (int i = 0; i < 10; i++) // wait 10 frames for animation
        {
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    IEnumerator<BTState> Follow()
    {
        Debug.Log("Entered follow");
        _animator.SetBool("chase", true);
        _agent.speed = 4.8f;
        _agent.acceleration = 3f;
        _agent.SetDestination(player.transform.position);
        yield return BTState.Continue;
        float time;
        float time1 = 0;
        while (time1 < 2f && IsInFollowRange() && ! IsInMeleeRange())
        {
            _agent.SetDestination(player.transform.position);
            time = 0;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                yield return BTState.Continue;
            }
            time1 += time;
            //_agent.SetDestination(transform.position);
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    /// <summary>
    /// Health deduct is handled by BallCollisionControl script attached on
    /// the ball. 
    /// </summary>
    /// <returns></returns>
    IEnumerator<BTState> RangeAttack()
    {
        Debug.Log("Entered ranged attack");
        _animator.SetBool("throw", true);
        _animator.SetBool("chase", true);
        _animator.SetBool("throwFail", false);
        // stop the movement by setting the destination to its position
        _agent.SetDestination(transform.position); 
        // wait for 1 frame
        yield return BTState.Continue;
        // make sure the monster faces the player
        Vector3 facing = new Vector3(
            player.transform.position.x - transform.position.x, 
            0, 
            player.transform.position.z - transform.position.z
            );
        transform.rotation = Quaternion.LookRotation(facing, transform.up);
        // wait until the 2 clips of animation finish
        float time = 0;
        while (time < 2f)
        {
            time += Time.deltaTime;
            if (_animator.GetBool("returnToChase"))
            {
                _animator.SetBool("throw", false);
                yield return BTState.Failure;
            }
            yield return BTState.Continue;
        }
        // get the ball back
        _animator.SetBool("transitNext", true);
        _animator.SetBool("throw", false);
        while (time < 4f)
        {
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        _animator.SetBool("transitNext", false);
        _animator.SetBool("returnToChase", true);
        for (int i = 0; i < 10; i++)
        {
            yield return BTState.Continue;
        }
        _animator.SetBool("returnToChase", false);
        yield return BTState.Success;
    }

    IEnumerator<BTState> MeleeAttack()
    {
        Debug.Log("Entered melee attack");
        float randomNumber = Random.Range(0f, 3f);
        if (randomNumber < 1f)
        {
            _animator.SetBool("punch1", true);
            float time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                yield return BTState.Continue;
            }
            // determine whether the player is still close to the zombie
            if (GetDistanceFromPlayer() < meleeAttackRange + 1.5f)
            {
                // the player gets damage
                int.TryParse(health.text, out int currHealth);
                currHealth -= damage;
                health.text = currHealth.ToString();
            }
            _animator.SetBool("punch1", false);
        }
        else if (randomNumber < 2f)
        {
            _animator.SetBool("punch2", true);
            float time = 0;
            while (time < 1f)
            {
                time += Time.deltaTime;
                yield return BTState.Continue;
            }
            // determine whether the player is still close to the zombie
            if (GetDistanceFromPlayer() < meleeAttackRange + 1.5f)
            {
                // the player gets damage
                int.TryParse(health.text, out int currHealth);
                currHealth -= damage;
                health.text = currHealth.ToString();
            }
            _animator.SetBool("punch2", false);
        }
        else {
            _animator.SetBool("punch3", true);
            float time = 0;
            while (time < 1.6f)
            {
                time += Time.deltaTime;
                yield return BTState.Continue;
            }
            // determine whether the player is still close to the zombie
            if (GetDistanceFromPlayer() < meleeAttackRange + 2f)
            {
                // the player gets damage
                int.TryParse(health.text, out int currHealth);
                currHealth -= damage;
                health.text = currHealth.ToString();
            }
            _animator.SetBool("punch3", false);
        }
        _animator.SetBool("transitNext", true);
        float time1 = 0;
        while (time1 < 2f)
        {
            time1 += Time.deltaTime;
            yield return BTState.Continue;
        }
        _animator.SetBool("transitNext", false);
        _animator.SetBool("returnToChase", true);
        for (int i = 0; i < 10; i++)
        {
            yield return BTState.Continue;
        }
        _animator.SetBool("returnToChase", false);
        yield return BTState.Success;
    }

    bool IsInSight()
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
        if (horizontalDist <= observeRange)
        {
            return true;
        }
        return false;
    }

    bool IsInRangeRange()
    {
        if (GetDistanceFromPlayer() <= rangedAttackRange)
        {
            return true;
        }
        return false;
    }

    bool ShouldRangeAtt()
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber > 0.8f)
        {
            return true;
        }
        return false;
    }

    bool IsInMeleeRange()
    {
        if (GetDistanceFromPlayer() <= meleeAttackRange)
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
