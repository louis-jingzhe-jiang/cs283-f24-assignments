using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTAI;
using UnityEngine.AI;

public class BehaviorUnique : MonoBehaviour
{
    private Root _treeRoot;

    /** 
     * The home area for the enemy minion
     */
    public GameObject restArea; // set this to a sphere

    /**
     * The attack range of the enemy minion
     */
    public float attackRange;

    /**
     * The follow range of the enemy minion
     */
    public float followRange; // probably no use by now

    /**
     * The distance when the player will be seen by the enemy minion
     */
    public float observeRange;

    /**
     * The attack and follow target of the enemy
     */
    public GameObject target;

    /**
     * The home area for the player
     */
    public GameObject playerHome;

    /**
     * whether the enemy minion is in targetting mode
     */
    private bool _foundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _foundPlayer = false;
        // root
        _treeRoot = BT.Root();
        // conditions
        BTNode foundPlayer = BT.Condition(FoundPlayer);
        BTNode shouldRetreat = BT.Condition(ShouldRetreat);
        // behaviors
        BTNode retreat = BT.RunCoroutine(Retreat);
        BTNode attack = BT.RunCoroutine(Attack);
        BTNode follow = BT.RunCoroutine(Follow);
        BTNode wander = BT.RunCoroutine(Wander);
        BTNode rest = BT.RunCoroutine(Rest);
        // sequences
        Sequence attSeq = new Sequence();
        Sequence retreatSeq = new Sequence();
        Sequence idleSeq = new Sequence();
        // add branches to attack sequence (attSeq)
        attSeq.OpenBranch(foundPlayer);
        attSeq.OpenBranch(follow);
        attSeq.OpenBranch(attack);
        // add branches to retreat sequence (retreatSeq)
        retreatSeq.OpenBranch(shouldRetreat);
        retreatSeq.OpenBranch(retreat);
        // add branches to idle sequence (idleSeq)
        idleSeq.OpenBranch(wander);
        idleSeq.OpenBranch(rest);
        // add branches to selector
        Selector selector = BT.Selector();
        selector.OpenBranch(attSeq);
        selector.OpenBranch(retreatSeq);
        selector.OpenBranch(idleSeq);
        // add branch to root
        _treeRoot.OpenBranch(selector);
    }

    // Update is called once per frame
    void Update()
    {
        _treeRoot.Tick();
    }

    /**
     * When the player returns to home area, the enemy minion should retreat
     * to its home area
     */
    IEnumerator<BTState> Retreat()
    {
        Debug.Log("Entered Retreat");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 destination = restArea.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(destination, out hit, 2.0f, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
        // wait for the enemy to reach destination
        while (agent.remainingDistance > 0.1f)
        {
           yield return BTState.Continue;
        }

        yield return BTState.Success;
    }

    /**
     * The player should follow the player if the player runs away
     */
    IEnumerator<BTState> Follow()
    {
        Debug.Log("Entered Follow");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 destination = target.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(destination, out hit, 2.0f, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
        // wait for the enemy to reach player
        while (agent.remainingDistance > .1f)
        {
            NavMesh.SamplePosition(destination, out hit, 2.0f, NavMesh.AllAreas);
                agent.SetDestination(hit.position);
            yield return BTState.Continue;
        }

        yield return BTState.Success;
    }
    
    /** 
     * The minion should attack the player if the player is in its attack range
     */
    IEnumerator<BTState> Attack()
    {
        Debug.Log("Entered Attack");
        // play animation
        Animator animator = GetComponent<Animator>();
        animator.SetBool("attack", true);
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        animator.SetBool("attack", false);
        // after playing animation
        // calculate the distance between the player and the minion
        float distance = Vector3.Distance(transform.position, 
            target.transform.position);
        // if the player is within reach of the minion, then
        // the attack is successful
        if (distance < attackRange) // successful 
        {
            yield return BTState.Success;
        }
        else // did not hit the player
        {
            yield return BTState.Failure;
            yield break;
        }
    }

    /** 
     * The wander behavior of the minion when no player detected
     */
    IEnumerator<BTState> Wander()
    {
        Debug.Log("Entered Wander");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 target;
        _RandomPoint(restArea.transform.position, 
            restArea.transform.localScale.x, out target);
        agent.SetDestination(target);

       // wait for agent to reach destination
        while (agent.remainingDistance > 0.1f)
        {   // abort the wander behavior when found player in range
            if (FoundPlayer()) {
                agent.ResetPath(); // Clears the path and stops the agent.
                //agent.velocity = Vector3.zero; // Stops any residual movement.
                yield return BTState.Abort;
                yield break;
            }
            // otherwise continue wandering
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    /** 
     * The rest behavior of the minion when no player detected
     */
    IEnumerator<BTState> Rest()
    {
        Debug.Log("Entered Rest");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //agent.ResetPath(); // Clears the path and stops the agent.
        float time = 0f;
        while (time < 4f) 
        {   // abort the idle behavior when found player in range
            if (FoundPlayer()) {
                yield return BTState.Abort;
                yield break;
            }
            yield return BTState.Continue;
            time += Time.deltaTime;
        }
        yield return BTState.Success;
    }

    bool FoundPlayer()
    {
        float distance = Vector3.Distance(transform.position, 
            target.transform.position);
        if (distance < observeRange) 
        {
            _foundPlayer = true;
            Debug.Log("FoundPlayer() returned true");
            return true;
        }
        _foundPlayer = false;
        Debug.Log("FoundPlayer() returned false");
        return false;
    }

    bool ShouldRetreat()
    {
        Debug.Log("ShouldRetreat() called");
        // check whether the player is in home area
        Collider player = target.GetComponent<Collider>();
        Collider home = playerHome.GetComponent<Collider>();
        if (player.bounds.Intersects(home.bounds))
        {
            _foundPlayer = false;
            return true;
        }
        return false;
    }

    /**
     * Find a random point on the NavMesh within a certain range
     */
    private bool _RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}