using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTAI;
using UnityEngine.AI;
/*
 * Basic idea of the behavior of the skeleton minion:
 * - Root
 *   - Main Selector (Selector)
 *     - Follow and Attack Sequence (Sequence)
 *       - Determine whether the player is in sight (Condition)
 *       - Second Selector (Selector)
 *         - Attack Sequence (Sequence)
 *           - Determine whether the player is in attack range (Condition)
 *           - Do attack (Action / RunCoroutine)
 *           - Do follow (Action / RunCoroutine)
 *         - Follow Sequence (Sequence)
 *           - Do follow (Action / RunCoroutine)
 *     - Retreat Sequence (Sequence)
 *       - Determine whether the player is in its home area (Condition)
 *       - Do retreat (Action / RunCoroutine)
 *     - Idle Sequence (Sequence)
 *       - Do idle (Action / RunCoroutine)
 */
public class BehaviorMinion : MonoBehaviour
{
    public GameObject playerHome; // home area of player
    public GameObject minionHome; // home are area of the minion
    public GameObject player;
    
    // below are the private variables regarding the structure of the behavior
    // tree's structure
    private Root treeRoot; // root
    private Selector mainSelector; // Main Selector
    private Sequence followAttack; // Follow and Attack Sequence
    // Determine whether the player is in sight
    private bool isInSight;
    private BTNode isInSightC;
    private Selector secondSelector; // Second Selector
    private Sequence attackSequence; // Attack Sequence
    // Determine whether the player is in attack range
    private bool shouldAttack;
    private BTNode shouldAttackC;
    private BTNode attack; // Do Attack
    private BTNode follow; // Do Follow
    private Sequence followSequence; // Follow Sequence
    private Sequence retreatSequence; // Retreat Sequence
    // Determine whether the player is in its home area
    private bool shouldRetreat;
    private BTNode shouldRetreatC;
    private BTNode retreat; // Do Retreat
    private Sequence idleSequence; // Idle Sequence
    private BTNode idle; // Do Idle


    // Start is called before the first frame update
    void Start()
    {
        // initialize
        treeRoot = BT.Root();
        mainSelector = BT.Selector();
        followAttack = BT.Sequence();
        isInSightC = BT.Condition(IsInSightM);
        secondSelector = BT.Selector();
        attackSequence = BT.Sequence();
        shouldAttackC = BT.Condition(ShouldAttackM);
        attack = BT.RunCoroutine(Attack);
        follow = BT.RunCoroutine(Follow);
        followSequence = BT.Sequence();
        retreatSequence = BT.Sequence();
        shouldRetreatC = BT.Condition(ShouldRetreatM);
        retreat = BT.RunCoroutine(Retreat);
        idleSequence = BT.Sequence();
        idle = BT.RunCoroutine(Idle);
        isInSight = false;
        shouldAttack = false;
        // link
        idleSequence.OpenBranch(idle);
        followSequence.OpenBranch(follow);
        retreatSequence.OpenBranch(shouldRetreatC, retreat);
        attackSequence.OpenBranch(shouldAttackC, attack, follow);
        secondSelector.OpenBranch(attackSequence, followSequence);
        followAttack.OpenBranch(isInSightC, secondSelector);
        mainSelector.OpenBranch(followAttack, retreatSequence, idleSequence);
        treeRoot.OpenBranch(mainSelector);
    }

    // Update is called once per frame
    void Update()
    {
        treeRoot.Tick();
    }

    /// <summary>
    /// The skeleton minion randomly moves to a location in the range determined
    /// by the GameObject minionHome, and then stays at the location for a few 
    /// seconds. The minion should play moving animation when moving, and rest 
    /// animation when it is at rest.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    IEnumerator<BTState> Idle() 
    {   // tested
        // TODO: interruptable by player's closing
        Debug.Log("Entered Idle Action");
        // go to a random position within minionHome on the NavMesh
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        RandomPoint(minionHome.transform.position, 5.0f, 
            out Vector3 destination);
        agent.SetDestination(destination);
        // ESSENTIAL! Wait for one frame after setting destination
        yield return BTState.Continue;
        // set speed in animator to play walk motion
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", agent.speed);
        while (agent.remainingDistance > 0.1f)
        {
            yield return BTState.Continue;
        }
        // set speed in animator back to 0 to play idle motion
        animator.SetFloat("speed", 0);
        // rest for 4 seconds before returning success
        float time = 0;
        while (time < 4)
        {
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    IEnumerator<BTState> Follow()
    {
        // tested
        Debug.Log("Entered Follow Action");
        // go towards the player's current location
        // version 1: the minion will NOT always run towards player
        // instead, it will run to the location of the player at the moment 
        // when this function is called
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);
        yield return BTState.Continue;
        // set speed in animator to play walk motion
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("speed", agent.speed);
        while (agent.remainingDistance > 0.1f)
        {
            yield return BTState.Continue;
        }
        yield return BTState.Success;
    }

    /// <summary>
    /// The minion's animator switches from moving animation to attack 
    /// animation. After playing the attack animation, the minion's animator
    /// should switch to rest animation.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    IEnumerator<BTState> Attack()
    {
        Debug.Log("Entered Attack Action");
        // To Be Implemented
        Animator animator = GetComponent<Animator>();
        animator.SetBool("attack", true);
        // wait for the animation clip to finish
        float duration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return BTState.Continue;
        }
        // animation clip finished, make the transition of animation
        animator.SetBool("attack", false);
        yield return BTState.Success;
    }

    /// <summary>
    /// The minion returns to the center of its spawn area (minionHome). This
    /// process cannot be interrupted by any action of the player. Moving 
    /// animation should be played at all times.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    IEnumerator<BTState> Retreat()
    {
        Debug.Log("Entered Retreat Action");
        // To Be Implemented
        yield return BTState.Success;
    }

    bool IsInSightM()
    {
        // To Be Implemented
        isInSight = ! isInSight;
        return true;
    }

    bool ShouldAttackM()
    {
        // To Be Implemented
        return true;
    }

    bool ShouldRetreatM()
    {
        // To Be Implemented
        return false;
    }

    /// <summary>
    /// Helper function to find a random point within a spherical range.
    /// The point is stored in parameter result as Vector3
    /// </summary>
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 
                    1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
